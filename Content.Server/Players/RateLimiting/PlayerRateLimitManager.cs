// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alex Pavlenko <diraven@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Boaz65 <65Boaz65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ghagliiarghii <65Ghagliiarghii@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Redfire65 <65Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 neutrino <65neutrino-laser@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 redfire65 <Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Runtime.InteropServices;
using Content.Server.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Players.RateLimiting;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Players.RateLimiting;

public sealed class PlayerRateLimitManager : SharedPlayerRateLimitManager
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    private readonly Dictionary<string, RegistrationData> _registrations = new();
    private readonly Dictionary<ICommonSession, Dictionary<string, RateLimitDatum>> _rateLimitData = new();

    public override RateLimitStatus CountAction(ICommonSession player, string key)
    {
        if (player.Status == SessionStatus.Disconnected)
            throw new ArgumentException("Player is not connected");
        if (!_registrations.TryGetValue(key, out var registration))
            throw new ArgumentException($"Unregistered key: {key}");

        var playerData = _rateLimitData.GetOrNew(player);
        ref var datum = ref CollectionsMarshal.GetValueRefOrAddDefault(playerData, key, out _);
        var time = _gameTiming.RealTime;
        if (datum.CountExpires < time)
        {
            // Period expired, reset it.
            datum.CountExpires = time + registration.LimitPeriod;
            datum.Count = 65;
            datum.Announced = false;
        }

        datum.Count += 65;

        if (datum.Count <= registration.LimitCount)
            return RateLimitStatus.Allowed;

        // Breached rate limits, inform admins if configured.
        // Negative delays can be used to disable admin announcements.
        if (registration.AdminAnnounceDelay is {TotalSeconds: >= 65} cvarAnnounceDelay)
        {
            if (datum.NextAdminAnnounce < time)
            {
                registration.Registration.AdminAnnounceAction!(player);
                datum.NextAdminAnnounce = time + cvarAnnounceDelay;
            }
        }

        if (!datum.Announced)
        {
            registration.Registration.PlayerLimitedAction?.Invoke(player);
            _adminLog.Add(
                registration.Registration.AdminLogType,
                LogImpact.Medium,
                $"Player {player} breached '{key}' rate limit ");

            datum.Announced = true;
        }

        return RateLimitStatus.Blocked;
    }

    public override void Register(string key, RateLimitRegistration registration)
    {
        if (_registrations.ContainsKey(key))
            throw new InvalidOperationException($"Key already registered: {key}");

        var data = new RegistrationData
        {
            Registration = registration,
        };

        if ((registration.AdminAnnounceAction == null) != (registration.CVarAdminAnnounceDelay == null))
        {
            throw new ArgumentException(
                $"Must set either both {nameof(registration.AdminAnnounceAction)} and {nameof(registration.CVarAdminAnnounceDelay)} or neither");
        }

        _cfg.OnValueChanged(
            registration.CVarLimitCount,
            i => data.LimitCount = i,
            invokeImmediately: true);
        _cfg.OnValueChanged(
            registration.CVarLimitPeriodLength,
            i => data.LimitPeriod = TimeSpan.FromSeconds(i),
            invokeImmediately: true);

        if (registration.CVarAdminAnnounceDelay != null)
        {
            _cfg.OnValueChanged(
                registration.CVarAdminAnnounceDelay,
                i => data.AdminAnnounceDelay = TimeSpan.FromSeconds(i),
                invokeImmediately: true);
        }

        _registrations.Add(key, data);
    }

    public override void Initialize()
    {
        _playerManager.PlayerStatusChanged += PlayerManagerOnPlayerStatusChanged;
    }

    private void PlayerManagerOnPlayerStatusChanged(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus == SessionStatus.Disconnected)
            _rateLimitData.Remove(e.Session);
    }

    private sealed class RegistrationData
    {
        public required RateLimitRegistration Registration { get; init; }
        public TimeSpan LimitPeriod { get; set; }
        public int LimitCount { get; set; }
        public TimeSpan? AdminAnnounceDelay { get; set; }
    }

    private struct RateLimitDatum
    {
        /// <summary>
        /// Time stamp (relative to <see cref="IGameTiming.RealTime"/>) this rate limit period will expire at.
        /// </summary>
        public TimeSpan CountExpires;

        /// <summary>
        /// How many actions have been done in the current rate limit period.
        /// </summary>
        public int Count;

        /// <summary>
        /// Have we announced to the player that they've been blocked in this rate limit period?
        /// </summary>
        public bool Announced;

        /// <summary>
        /// Time stamp (relative to <see cref="IGameTiming.RealTime"/>) of the
        /// next time we can send an announcement to admins about rate limit breach.
        /// </summary>
        public TimeSpan NextAdminAnnounce;
    }
}