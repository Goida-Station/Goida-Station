// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Robust.Shared.Player;
using Robust.Shared.Toolshed;

namespace Content.Server.Administration.Toolshed;

[ToolshedCommand, AdminCommand(AdminFlags.Admin)]
public sealed class AdminsCommand : ToolshedCommand
{
    [Dependency] private readonly IAdminManager _admin = default!;

    [CommandImplementation("active")]
    public IEnumerable<ICommonSession> Active()
    {
        return _admin.ActiveAdmins;
    }

    [CommandImplementation("all")]
    public IEnumerable<ICommonSession> All()
    {
        return _admin.AllAdmins;
    }
}