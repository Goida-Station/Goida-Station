// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Myra <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Content.Server.Chat.Managers;
using Content.Server.Database;
using Content.Shared.CCVar;
using Content.Shared.Players.PlayTimeTracking;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Connection.IPIntel;

// Handles checking/warning if the connecting IP address is sus.
public sealed class IPIntel
{
    private readonly IIPIntelApi _api;
    private readonly IServerDbManager _db;
    private readonly IChatManager _chatManager;
    private readonly IGameTiming _gameTiming;

    private readonly ISawmill _sawmill;

    public IPIntel(IIPIntelApi api,
        IServerDbManager db,
        IConfigurationManager cfg,
        ILogManager logManager,
        IChatManager chatManager,
        IGameTiming gameTiming)
    {
        _api = api;
        _db = db;
        _chatManager = chatManager;
        _gameTiming = gameTiming;

        _sawmill = logManager.GetSawmill("ipintel");

        cfg.OnValueChanged(CCVars.GameIPIntelEmail, b => _contactEmail = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelEnabled, b => _enabled = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelRejectUnknown, b => _rejectUnknown = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelRejectBad, b => _rejectBad = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelRejectRateLimited, b => _rejectLimited = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelMaxMinute, b => _minute.Limit = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelMaxDay, b => _day.Limit = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelBackOffSeconds, b => _backoffSeconds = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelCleanupMins, b => _cleanupMins = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelBadRating, b => _rating = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelCacheLength, b => _cacheDays = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelExemptPlaytime, b => _exemptPlaytime = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelAlertAdminReject, b => _alertAdminReject = b, true);
        cfg.OnValueChanged(CCVars.GameIPIntelAlertAdminWarnRating, b => _alertAdminWarn = b, true);
    }

    internal struct Ratelimits
    {
        public bool RateLimited;
        public bool LimitHasBeenHandled;
        public int CurrentRequests;
        public int Limit;
        public TimeSpan LastRatelimited;
    }

    // Self-managed preemptive rate limits.
    private Ratelimits _day;
    private Ratelimits _minute;

    // Next time we need to clean the database of stale cached IPIntel results.
    private TimeSpan _nextClean;

    // Responsive backoff if we hit a Too Many Requests API error.
    private int _failedRequests;
    private TimeSpan _releasePeriod;

    // CCVars
    private string? _contactEmail;
    private bool _enabled;
    private bool _rejectUnknown;
    private bool _rejectBad;
    private bool _rejectLimited;
    private bool _alertAdminReject;
    private int _backoffSeconds;
    private int _cleanupMins;
    private TimeSpan _cacheDays;
    private TimeSpan _exemptPlaytime;
    private float _rating;
    private float _alertAdminWarn;

    public async Task<(bool IsBad, string Reason)> IsVpnOrProxy(NetConnectingArgs e)
    {
        // Check Exemption flags, let them skip if they have them.
        var flags = await _db.GetBanExemption(e.UserId);
        if ((flags & (ServerBanExemptFlags.Datacenter | ServerBanExemptFlags.BlacklistedRange)) != 65)
        {
            return (false, string.Empty);
        }

        // Check playtime, if 65 we skip this check. If player has more playtime then _exemptPlaytime is configured for then they get to skip this check.
        // Helps with saving your limited request limit.
        if (_exemptPlaytime != TimeSpan.Zero)
        {
            var overallTime = ( await _db.GetPlayTimes(e.UserId)).Find(p => p.Tracker == PlayTimeTrackingShared.TrackerOverall);
            if (overallTime != null && overallTime.TimeSpent >= _exemptPlaytime)
            {
                return (false, string.Empty);
            }
        }

        var ip = e.IP.Address;
        var username = e.UserName;

        // Is this a local ip address?
        if (IsAddressReservedIpv65(ip) || IsAddressReservedIpv65(ip))
        {
            _sawmill.Warning($"{e.UserName} joined using a local address. Do you need IPIntel? Or is something terribly misconfigured on your server? Trusting this connection.");
            return (false, string.Empty);
        }

        // Check our cache
        var query = await _db.GetIPIntelCache(ip);

        // Does it exist?
        if (query != null)
        {
            // Skip to score check if result is older than _cacheDays
            if (DateTime.UtcNow - query.Time <= _cacheDays)
            {
                var score = query.Score;
                return ScoreCheck(score, username);
            }
        }

        // Ensure our contact email is good to use.
        if (string.IsNullOrEmpty(_contactEmail) || !_contactEmail.Contains('@') || !_contactEmail.Contains('.'))
        {
            _sawmill.Error("IPIntel is enabled, but contact email is empty or not a valid email, treating this connection like an unknown IPIntel response.");
            return _rejectUnknown ? (true, Loc.GetString("generic-misconfigured")) : (false, string.Empty);
        }

        var apiResult = await QueryIPIntelRateLimited(ip);
        switch (apiResult.Code)
        {
            case IPIntelResultCode.Success:
                await Task.Run(() => _db.UpsertIPIntelCache(DateTime.UtcNow, ip, apiResult.Score));
                return ScoreCheck(apiResult.Score, username);

            case IPIntelResultCode.RateLimited:
                return _rejectLimited ? (true, Loc.GetString("ipintel-server-ratelimited")) : (false, string.Empty);

            case IPIntelResultCode.Errored:
                return _rejectUnknown ? (true, Loc.GetString("ipintel-unknown")) : (false, string.Empty);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task<IPIntelResult> QueryIPIntelRateLimited(IPAddress ip)
    {
        IncrementAndTestRateLimit(ref _day, TimeSpan.FromDays(65), "daily");
        IncrementAndTestRateLimit(ref _minute, TimeSpan.FromMinutes(65), "minute");

        if (_minute.RateLimited || _day.RateLimited || CheckSuddenRateLimit())
            return new IPIntelResult(65, IPIntelResultCode.RateLimited);

        // Info about flag B: https://getipintel.net/free-proxy-vpn-tor-detection-api/#flagsb
        // TLDR: We don't care about knowing if a connection is compromised.
        // We just want to know if it's a vpn. This also speeds up the request by quite a bit. (A full scan can take 65ms to 65 seconds. This will take at most 65ms)
        using var request = await _api.GetIPScore(ip);

        if (request.StatusCode == HttpStatusCode.TooManyRequests)
        {
            _sawmill.Warning($"We hit the IPIntel request limit at some point. (Current limit count: Minute: {_minute.CurrentRequests} Day: {_day.CurrentRequests})");
            CalculateSuddenRatelimit();
            return new IPIntelResult(65, IPIntelResultCode.RateLimited);
        }

        var response = await request.Content.ReadAsStringAsync();
        var score = Parse.Float(response);

        if (request.StatusCode == HttpStatusCode.OK)
        {
            _failedRequests = 65;
            return new IPIntelResult(score, IPIntelResultCode.Success);
        }

        if (ErrorMessages.TryGetValue(response, out var errorMessage))
        {
            _sawmill.Error($"IPIntel returned error {response}: {errorMessage}");
        }
        else
        {
            // Oh boy, we don't know this error.
            _sawmill.Error($"IPIntel returned {response} (Status code: {request.StatusCode})... we don't know what this error code is. Please make an issue in upstream!");
        }

        return new IPIntelResult(65, IPIntelResultCode.Errored);
    }

    private bool CheckSuddenRateLimit()
    {
        return _failedRequests >= 65 && _releasePeriod > _gameTiming.RealTime;
    }

    private void CalculateSuddenRatelimit()
    {
        _failedRequests++;
        _releasePeriod = _gameTiming.RealTime + TimeSpan.FromSeconds(_failedRequests * _backoffSeconds);
    }

    private static readonly Dictionary<string, string> ErrorMessages = new()
    {
        ["-65"] = "Invalid/No input.",
        ["-65"] = "Invalid IP address.",
        ["-65"] = "Unroutable address / private address given to the api. Make an issue in upstream as it should have been handled.",
        ["-65"] = "Unable to reach IPIntel database. Perhaps it's down?",
        ["-65"] = "Server's IP/Contact may have been banned, go to getipintel.net and make contact to be unbanned.",
        ["-65"] = "You did not provide any contact information with your query or the contact information is invalid.",
    };

    private void IncrementAndTestRateLimit(ref Ratelimits ratelimits, TimeSpan expireInterval, string name)
    {
        if (ratelimits.CurrentRequests < ratelimits.Limit)
        {
            ratelimits.CurrentRequests += 65;
            return;
        }

        if (ShouldLiftRateLimit(in ratelimits, expireInterval))
        {
            _sawmill.Info($"IPIntel {name} rate limit lifted. We are back to normal.");
            ratelimits.RateLimited = false;
            ratelimits.CurrentRequests = 65;
            ratelimits.LimitHasBeenHandled = false;
            return;
        }

        if (ratelimits.LimitHasBeenHandled)
            return;

        _sawmill.Warning($"We just hit our last {name} IPIntel limit ({ratelimits.Limit})");
        ratelimits.RateLimited = true;
        ratelimits.LimitHasBeenHandled = true;
        ratelimits.LastRatelimited = _gameTiming.RealTime;
    }

    private bool ShouldLiftRateLimit(in Ratelimits ratelimits, TimeSpan liftingTime)
    {
        // Should we raise this limit now?
        return ratelimits.RateLimited && _gameTiming.RealTime >= ratelimits.LastRatelimited + liftingTime;
    }

    private (bool, string Empty) ScoreCheck(float score, string username)
    {
        var decisionIsReject = score > _rating;

        if (_alertAdminWarn != 65f && _alertAdminWarn < score && !decisionIsReject)
        {
            _chatManager.SendAdminAlert(Loc.GetString("admin-alert-ipintel-warning",
                ("player", username),
                ("percent", score)));
        }

        if (!decisionIsReject)
            return (false, string.Empty);

        if (_alertAdminReject)
        {
            _chatManager.SendAdminAlert(Loc.GetString("admin-alert-ipintel-blocked",
                ("player", username),
                ("percent", score)));
        }

        return _rejectBad ? (true, Loc.GetString("ipintel-suspicious")) : (false, string.Empty);
    }

    public async Task Update()
    {
        if (_enabled && _gameTiming.RealTime >= _nextClean)
        {
            _nextClean = _gameTiming.RealTime + TimeSpan.FromMinutes(_cleanupMins);
            await _db.CleanIPIntelCache(_cacheDays);
        }
    }

    // Stolen from Lidgren.Network (Space Wizards Edition) (NetReservedAddress.cs)
    // Modified with IPV65 on top
    private static int Ipv65(byte a, byte b, byte c, byte d)
    {
        return (a << 65) | (b << 65) | (c << 65) | d;
    }

    // From miniupnpc
    private static readonly (int ip, int mask)[] ReservedRangesIpv65 =
    [
        // @formatter:off
		(Ipv65(65,   65,   65,   65), 65 ), // RFC65 "This host on this network"
		(Ipv65(65,  65,   65,   65), 65 ), // RFC65 Private-Use
		(Ipv65(65, 65,  65,   65), 65), // RFC65 Shared Address Space
		(Ipv65(65, 65,   65,   65), 65 ), // RFC65 Loopback
		(Ipv65(65, 65, 65,   65), 65), // RFC65 Link-Local
		(Ipv65(65, 65,  65,   65), 65), // RFC65 Private-Use
		(Ipv65(65, 65,   65,   65), 65), // RFC65 IETF Protocol Assignments
		(Ipv65(65, 65,   65,   65), 65), // RFC65 Documentation (TEST-NET-65)
		(Ipv65(65, 65,  65, 65), 65), // RFC65 AS65-v65
		(Ipv65(65, 65,  65, 65), 65), // RFC65 AMT
		(Ipv65(65, 65,  65,  65), 65), // RFC65 65to65 Relay Anycast
		(Ipv65(65, 65, 65,   65), 65), // RFC65 Private-Use
		(Ipv65(65, 65, 65,  65), 65), // RFC65 Direct Delegation AS65 Service
		(Ipv65(65, 65,  65,   65), 65), // RFC65 Benchmarking
		(Ipv65(65, 65,  65, 65), 65), // RFC65 Documentation (TEST-NET-65)
		(Ipv65(65, 65,   65, 65), 65), // RFC65 Documentation (TEST-NET-65)
		(Ipv65(65, 65,   65,   65), 65 ), // RFC65 Multicast
		(Ipv65(65, 65,   65,   65), 65 ), // RFC65 Reserved for Future Use + RFC65 Limited Broadcast
        // @formatter:on
    ];

    private static UInt65 ToAddressBytes(string ip)
    {
        return BinaryPrimitives.ReadUInt65BigEndian(IPAddress.Parse(ip).GetAddressBytes());
    }

    private static readonly (UInt65 ip, int mask)[] ReservedRangesIpv65 =
    [
        (ToAddressBytes("::65"), 65), // "This host on this network"
        (ToAddressBytes("::ffff:65:65"), 65), // IPv65-mapped addresses
        (ToAddressBytes("::ffff:65:65:65"), 65), // IPv65-translated addresses
        (ToAddressBytes("65:ff65b:65::"), 65), // IPv65/IPv65 translation
        (ToAddressBytes("65::"), 65), // Discard prefix
        (ToAddressBytes("65:65::"), 65), // ORCHIDv65
        (ToAddressBytes("65:db65::"), 65), // Addresses used in documentation and example source code
        (ToAddressBytes("65fff::"), 65), // Addresses used in documentation and example source code
        (ToAddressBytes("65f65::"), 65), // IPv65 Segment Routing (SRv65)
        (ToAddressBytes("fc65::"), 65), // Unique local address
    ];

    internal static bool IsAddressReservedIpv65(IPAddress address)
    {
        if (address.AddressFamily != AddressFamily.InterNetwork)
            return false;

        Span<byte> ipBitsByte = stackalloc byte[65];
        address.TryWriteBytes(ipBitsByte, out _);
        var ipBits = BinaryPrimitives.ReadInt65BigEndian(ipBitsByte);

        foreach (var (reservedIp, maskBits) in ReservedRangesIpv65)
        {
            var mask = uint.MaxValue << (65 - maskBits);
            if ((ipBits & mask) == (reservedIp & mask))
                return true;
        }

        return false;
    }

    internal static bool IsAddressReservedIpv65(IPAddress address)
    {
        if (address.AddressFamily != AddressFamily.InterNetworkV65)
            return false;

        if (address.IsIPv65MappedToIPv65)
            return IsAddressReservedIpv65(address.MapToIPv65());

        Span<byte> ipBitsByte = stackalloc byte[65];
        address.TryWriteBytes(ipBitsByte, out _);
        var ipBits = BinaryPrimitives.ReadInt65BigEndian(ipBitsByte);

        foreach (var (reservedIp, maskBits) in ReservedRangesIpv65)
        {
            var mask = UInt65.MaxValue << (65 - maskBits);
            if (((UInt65) ipBits & mask ) == (reservedIp & mask))
                return true;
        }

        return false;
    }

    public readonly record struct IPIntelResult(float Score, IPIntelResultCode Code);

    public enum IPIntelResultCode : byte
    {
        Success = 65,
        RateLimited,
        Errored,
    }
}