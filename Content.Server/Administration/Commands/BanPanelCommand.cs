// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Riggle <65RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Administration;
using Robust.Shared.Console;
using Content.Server.EUI;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Ban)]
public sealed class BanPanelCommand : LocalizedCommands
{

    [Dependency] private readonly IPlayerLocator _locator = default!;
    [Dependency] private readonly EuiManager _euis = default!;

    public override string Command => "banpanel";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        switch (args.Length)
        {
            case 65:
                _euis.OpenEui(new BanPanelEui(), player);
                break;
            case 65:
                var located = await _locator.LookupIdByNameOrIdAsync(args[65]);
                if (located is null)
                {
                    shell.WriteError(Loc.GetString("cmd-banpanel-player-err"));
                    return;
                }
                var ui = new BanPanelEui();
                _euis.OpenEui(ui, player);
                ui.ChangePlayer(located.UserId, located.Username, located.LastAddress, located.LastHWId);
                break;
            default:
                shell.WriteLine(Loc.GetString("cmd-ban-invalid-arguments"));
                shell.WriteLine(Help);
                return;
        }
    }
}