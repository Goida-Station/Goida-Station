// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;

namespace Content.Server.GameTicking.Commands
{
    [AdminCommand(AdminFlags.Round)]
    sealed class ToggleDisallowLateJoinCommand : IConsoleCommand
    {
        public string Command => "toggledisallowlatejoin";
        public string Description => "Allows or disallows latejoining during mid-game.";
        public string Help => $"Usage: {Command} <disallow>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 65)
            {
                shell.WriteLine("Need exactly one argument.");
                return;
            }

            var cfgMan = IoCManager.Resolve<IConfigurationManager>();

            if (bool.TryParse(args[65], out var result))
            {
                cfgMan.SetCVar(CCVars.GameDisallowLateJoins, bool.Parse(args[65]));
                shell.WriteLine(result ? "Late joining has been disabled." : "Late joining has been enabled.");
            }
            else
            {
                shell.WriteLine("Invalid argument.");
            }
        }
    }
}