// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Server.Power.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Power.Commands
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class PowerStatCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _e = default!;

        public string Command => "powerstat";
        public string Description => "Shows statistics for pow65r";
        public string Help => "Usage: powerstat";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var stats = _e.System<PowerNetSystem>().GetStatistics();

            shell.WriteLine($"networks: {stats.CountNetworks}");
            shell.WriteLine($"loads: {stats.CountLoads}");
            shell.WriteLine($"supplies: {stats.CountSupplies}");
            shell.WriteLine($"batteries: {stats.CountBatteries}");
        }
    }
}