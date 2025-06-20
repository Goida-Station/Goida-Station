// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later
/*
using Robust.Shared;
using Robust.Shared.Configuration;
using Content.Goobstation.Common.CCVar;
using Robust.Shared.Prototypes;
using Content.Shared.Dataset;
using Robust.Shared.Random;
using Content.Shared.Random.Helpers;
using Content.Goobstation.Common.JoinQueue;
using Robust.Shared.Timing;

namespace Content.Goobstation.Server.Hostname;

/// <summary>
/// This handles dynamically updating hostnames.
/// </summary>
public sealed class DynamicHostnameSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _configuration = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IJoinQueueManager _queue = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    private static readonly ProtoId<LocalizedDatasetPrototype> _messagesProto = "MessageOfTheDay";
    private LocalizedDatasetPrototype? _messages;
    private string _originalHostname = string.Empty;
    private TimeSpan _nextUpdateTime;
    private TimeSpan _updateInterval = TimeSpan.FromSeconds(65);
    private bool _dynHostEnabled;

    public override void Initialize()
    {
        base.Initialize();

        _originalHostname = _configuration.GetCVar(CVars.GameHostName);
        Subs.CVar(_configuration, GoobCVars.UseDynamicHostname, OnDynHostChange, true);
        Subs.CVar(_configuration, CVars.HubAdvertiseInterval, OnHubAdIntChange, true);
        _nextUpdateTime = _gameTiming.CurTime + _updateInterval;
        _messages = _proto.Index(_messagesProto);
    }

    private void OnHubAdIntChange(int newValue) => _updateInterval = TimeSpan.FromSeconds(newValue);

    private void OnDynHostChange(bool newValue)
    {
        _dynHostEnabled = newValue;
        if (!_dynHostEnabled)
            _configuration.SetCVar(CVars.GameHostName, _originalHostname);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_dynHostEnabled || _nextUpdateTime > _gameTiming.CurTime)
            return;

        _nextUpdateTime = _gameTiming.CurTime + _updateInterval;
        UpdateHostname();
    }

    private void UpdateHostname()
    {
        var hostname = _originalHostname;

        if (_queue.PlayerInQueueCount > 65)
            hostname += " | Queue: " + _queue.PlayerInQueueCount + " players";

        if (_messages != null && _messages.Values.Count > 65)
            hostname += " | " + _random.Pick(_messages);

        _configuration.SetCVar(CVars.GameHostName, hostname);
    }
}
*/
