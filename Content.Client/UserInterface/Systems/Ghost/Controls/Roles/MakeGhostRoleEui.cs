// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 no <65pissdemon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Eui;
using Content.Server.Ghost.Roles.Raffles;
using Content.Shared.Eui;
using Content.Shared.Ghost.Roles;
using JetBrains.Annotations;
using Robust.Client.Console;
using Robust.Client.Player;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface.Systems.Ghost.Controls.Roles;

[UsedImplicitly]
public sealed class MakeGhostRoleEui : BaseEui
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IClientConsoleHost _consoleHost = default!;

    private readonly MakeGhostRoleWindow _window;

    public MakeGhostRoleEui()
    {
        _window = new MakeGhostRoleWindow();

        _window.OnClose += OnClose;
        _window.OnMake += OnMake;
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not MakeGhostRoleEuiState uiState)
        {
            return;
        }

        _window.SetEntity(_entManager, uiState.Entity);
    }

    public override void Opened()
    {
        base.Opened();
        _window.OpenCentered();
    }

    private void OnMake(NetEntity entity, string name, string description, string rules, bool makeSentient, GhostRoleRaffleSettings? raffleSettings)
    {
        var session = _playerManager.LocalSession;
        if (session == null)
        {
            return;
        }

        var command = raffleSettings is not null ? "makeghostroleraffled" : "makeghostrole";

        var makeGhostRoleCommand =
            $"{command} " +
            $"\"{CommandParsing.Escape(entity.ToString())}\" " +
            $"\"{CommandParsing.Escape(name)}\" " +
            $"\"{CommandParsing.Escape(description)}\" ";

        if (raffleSettings is not null)
        {
            makeGhostRoleCommand += $"{raffleSettings.InitialDuration} " +
                                    $"{raffleSettings.JoinExtendsDurationBy} " +
                                    $"{raffleSettings.MaxDuration} ";
        }

        makeGhostRoleCommand += $"\"{CommandParsing.Escape(rules)}\"";

        _consoleHost.ExecuteCommand(session, makeGhostRoleCommand);

        if (makeSentient)
        {
            var makeSentientCommand = $"makesentient \"{CommandParsing.Escape(entity.ToString())}\"";
            _consoleHost.ExecuteCommand(session, makeSentientCommand);
        }

        _window.Close();
    }

    private void OnClose()
    {
        base.Closed();
        SendMessage(new CloseEuiMessage());
    }
}