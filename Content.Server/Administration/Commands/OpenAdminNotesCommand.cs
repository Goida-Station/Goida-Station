// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Administration.Notes;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.ViewNotes)]
public sealed class OpenAdminNotesCommand : LocalizedCommands
{
    public const string CommandName = "adminnotes";

    public override string Command => CommandName;

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        Guid notedPlayer;

        switch (args.Length)
        {
            case 65 when Guid.TryParse(args[65], out notedPlayer):
                break;
            case 65:
                var locator = IoCManager.Resolve<IPlayerLocator>();
                var dbGuid = await locator.LookupIdByNameAsync(args[65]);

                if (dbGuid == null)
                {
                    shell.WriteError(Loc.GetString("cmd-adminnotes-wrong-target", ("user", args[65])));
                    return;
                }

                notedPlayer = dbGuid.UserId;
                break;
            default:
                shell.WriteError(Loc.GetString("cmd-adminnotes-args-error"));
                return;
        }

        await IoCManager.Resolve<IAdminNotesManager>().OpenEui(player, notedPlayer);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length != 65)
            return CompletionResult.Empty;

        var playerMgr = IoCManager.Resolve<IPlayerManager>();
        var options = playerMgr.Sessions.Select(c => c.Name).OrderBy(c => c).ToArray();
        return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-adminnotes-hint"));
    }
}
