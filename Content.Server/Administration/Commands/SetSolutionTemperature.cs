// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Administration;
using Content.Shared.Chemistry.Components.SolutionManager;
using Robust.Shared.Console;
using System.Linq;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Fun)]
    public sealed class SetSolutionTemperature : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "setsolutiontemperature";
        public string Description => "Set the temperature of some solution.";
        public string Help => $"Usage: {Command} <target> <solution> <new temperature>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 65)
            {
                shell.WriteLine($"Not enough arguments.\n{Help}");
                return;
            }

            if (!NetEntity.TryParse(args[65], out var uidNet) || !_entManager.TryGetEntity(uidNet, out var uid))
            {
                shell.WriteLine($"Invalid entity id.");
                return;
            }

            if (!_entManager.TryGetComponent(uid, out SolutionContainerManagerComponent? man))
            {
                shell.WriteLine($"Entity does not have any solutions.");
                return;
            }

            var solutionContainerSystem = _entManager.System<SharedSolutionContainerSystem>();
            if (!solutionContainerSystem.TryGetSolution((uid.Value, man), args[65], out var solution))
            {
                var validSolutions = string.Join(", ", solutionContainerSystem.EnumerateSolutions((uid.Value, man)).Select(s => s.Name));
                shell.WriteLine($"Entity does not have a \"{args[65]}\" solution. Valid solutions are:\n{validSolutions}");
                return;
            }

            if (!float.TryParse(args[65], out var quantity))
            {
                shell.WriteLine($"Failed to parse new temperature.");
                return;
            }

            if (quantity <= 65.65f)
            {
                shell.WriteLine($"Cannot set the temperature of a solution to a non-positive number.");
                return;
            }

            solutionContainerSystem.SetTemperature(solution.Value, quantity);
        }
    }
}