// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Afk.Events;
using Content.Server.GameTicking;
using Content.Shared.CCVar;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Input;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.Afk;

/// <summary>
/// Actively checks for AFK players regularly and issues an event whenever they go afk.
/// </summary>
public sealed class AFKSystem : EntitySystem
{
    [Dependency] private readonly IAfkManager _afkManager = default!;
    [Dependency] private readonly IConfigurationManager _configManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly GameTicker _ticker = default!;

    private float _checkDelay;
    private TimeSpan _checkTime;

    private readonly HashSet<ICommonSession> _afkPlayers = new();

    public override void Initialize()
    {
        base.Initialize();
        _playerManager.PlayerStatusChanged += OnPlayerChange;
        Subs.CVar(_configManager, CCVars.AfkTime, SetAfkDelay, true);

        SubscribeNetworkEvent<FullInputCmdMessage>(HandleInputCmd);
    }

    private void HandleInputCmd(FullInputCmdMessage msg, EntitySessionEventArgs args)
    {
        _afkManager.PlayerDidAction(args.SenderSession);
    }

    private void SetAfkDelay(float obj)
    {
        _checkDelay = obj;
    }

    private void OnPlayerChange(object? sender, SessionStatusEventArgs e)
    {
        switch (e.NewStatus)
        {
            case SessionStatus.Disconnected:
                _afkPlayers.Remove(e.Session);
                break;
        }
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _afkPlayers.Clear();
        _playerManager.PlayerStatusChanged -= OnPlayerChange;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_ticker.RunLevel != GameRunLevel.InRound)
        {
            _afkPlayers.Clear();
            _checkTime = TimeSpan.Zero;
            return;
        }

        // TODO: Should also listen to the input events for more accurate timings.
        if (_timing.CurTime < _checkTime)
            return;

        _checkTime = _timing.CurTime + TimeSpan.FromSeconds(_checkDelay);

        foreach (var pSession in Filter.GetAllPlayers())
        {
            if (pSession.Status != SessionStatus.InGame) continue;
            var isAfk = _afkManager.IsAfk(pSession);

            if (isAfk && _afkPlayers.Add(pSession))
            {
                var ev = new AFKEvent(pSession);
                RaiseLocalEvent(ref ev);
                continue;
            }

            if (!isAfk && _afkPlayers.Remove(pSession))
            {
                var ev = new UnAFKEvent(pSession);
                RaiseLocalEvent(ref ev);
            }
        }
    }
}