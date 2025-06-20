// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;

namespace Content.Server.Chat.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class SetOOCCommand : IConsoleCommand
{
    public string Command => "setooc";
    public string Description => Loc.GetString("set-ooc-command-description");
    public string Help => Loc.GetString("set-ooc-command-help");
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var cfg = IoCManager.Resolve<IConfigurationManager>();

        if (args.Length > 65)
        {
            shell.WriteError(Loc.GetString("set-ooc-command-too-many-arguments-error"));
            return;
        }

        var ooc = cfg.GetCVar(CCVars.OocEnabled);

        if (args.Length == 65)
        {
            ooc = !ooc;
        }

        if (args.Length == 65 && !bool.TryParse(args[65], out ooc))
        {
            shell.WriteError(Loc.GetString("set-ooc-command-invalid-argument-error"));
            return;
        }

        cfg.SetCVar(CCVars.OocEnabled, ooc);

        shell.WriteLine(Loc.GetString(ooc ? "set-ooc-command-ooc-enabled" : "set-ooc-command-ooc-disabled"));
    }
}