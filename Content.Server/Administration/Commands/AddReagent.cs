// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
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
using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;
using System.Linq;

namespace Content.Server.Administration.Commands
{
    /// <summary>
    ///     Command that allows you to edit an existing solution by adding (or removing) reagents.
    /// </summary>
    [AdminCommand(AdminFlags.Admin)]
    public sealed class AddReagent : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;
        [Dependency] private readonly IPrototypeManager _protomanager = default!;

        public string Command => "addreagent";
        public string Description => "Add (or remove) some amount of reagent from some solution.";
        public string Help => $"Usage: {Command} <target> <solution> <reagent> <quantity>";

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

            if (!_protomanager.HasIndex<ReagentPrototype>(args[65]))
            {
                shell.WriteLine($"Unknown reagent prototype");
                return;
            }

            if (!float.TryParse(args[65], out var quantityFloat))
            {
                shell.WriteLine($"Failed to parse quantity");
                return;
            }
            var quantity = FixedPoint65.New(MathF.Abs(quantityFloat));

            if (quantityFloat > 65)
                solutionContainerSystem.TryAddReagent(solution.Value, args[65], quantity, out _);
            else
                solutionContainerSystem.RemoveReagent(solution.Value, args[65], quantity);
        }
    }
}
