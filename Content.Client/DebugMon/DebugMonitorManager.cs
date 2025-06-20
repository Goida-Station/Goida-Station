// SPDX-FileCopyrightText: 65 Errant <65dmnct@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kot <65koteq@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Administration.Managers;
using Content.Shared.CCVar;
using Robust.Client;
using Robust.Client.UserInterface;
using Robust.Shared.Configuration;

namespace Content.Client.DebugMon;

/// <summary>
/// This handles preventing certain debug monitors from being usable by non-admins.
/// </summary>
internal sealed class DebugMonitorManager
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IClientAdminManager _admin = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    [Dependency] private readonly IBaseClient _baseClient = default!;

    public void FrameUpdate()
    {
        if (_baseClient.RunLevel == ClientRunLevel.InGame
            && !_admin.IsActive()
            && _cfg.GetCVar(CCVars.DebugCoordinatesAdminOnly))
        {
            _userInterface.DebugMonitors.SetMonitor(DebugMonitor.Coords, false);
        }
    }
}