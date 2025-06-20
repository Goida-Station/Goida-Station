// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Riggle <65RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Notes;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands;

[AnyCommand]
public sealed class OpenUserVisibleNotesCommand : IConsoleCommand
{
    [Dependency] private readonly IConfigurationManager _configuration = default!;
    [Dependency] private readonly IAdminNotesManager _notes = default!;

    public const string CommandName = "adminremarks";

    public string Command => CommandName;
    public string Description => Loc.GetString("admin-remarks-command-description");
    public string Help => $"Usage: {Command}";

    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (!_configuration.GetCVar(CCVars.SeeOwnNotes))
        {
            shell.WriteError(Loc.GetString("admin-remarks-command-error"));
            return;
        }

        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        await _notes.OpenUserNotesEui(player);
    }
}