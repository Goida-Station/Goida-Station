// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Robust.Shared.Player;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Syntax;

namespace Content.Server.Toolshed.Commands.AdminDebug;

[ToolshedCommand, AdminCommand(AdminFlags.Debug)]
public sealed class ACmdCommand : ToolshedCommand
{
    [Dependency] private readonly IAdminManager _adminManager = default!;

    [CommandImplementation("perms")]
    public AdminFlags[]? Perms([PipedArgument] CommandSpec command)
    {
        var res = _adminManager.TryGetCommandFlags(command, out var flags);
        if (res)
            flags ??= Array.Empty<AdminFlags>();
        return flags;
    }

    [CommandImplementation("caninvoke")]
    public bool CanInvoke(IInvocationContext ctx, [PipedArgument] CommandSpec command, ICommonSession player)
    {
        // Deliberately discard the error.
        return ((IPermissionController) _adminManager).CheckInvokable(command, player, out _);
    }
}