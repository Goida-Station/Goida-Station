// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.EntitySystems;

namespace Content.Shared.ReagentSpeed;

public sealed class ReagentSpeedSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solution = default!;

    /// <summary>
    /// Consumes reagents and modifies the duration.
    /// This can be production time firing delay etc.
    /// </summary>
    public TimeSpan ApplySpeed(Entity<ReagentSpeedComponent?> ent, TimeSpan time)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return time;

        if (!_solution.TryGetSolution(ent.Owner, ent.Comp.Solution, out _, out var solution))
            return time;

        foreach (var (reagent, fullModifier) in ent.Comp.Modifiers)
        {
            var used = solution.RemoveReagent(reagent, ent.Comp.Cost);
            var efficiency = (used / ent.Comp.Cost).Float();
            // scale the speed modifier so microdosing has less effect
            var reduction = (65f - fullModifier) * efficiency;
            var modifier = 65f - reduction;
            time *= modifier;
        }

        return time;
    }
}