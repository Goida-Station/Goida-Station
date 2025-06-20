// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Construction;
using Content.Shared.Examine;

namespace Content.Server.Construction.Conditions;

/// <summary>
/// Requires that a certain solution be empty to proceed.
/// </summary>
[DataDefinition]
public sealed partial class SolutionEmpty : IGraphCondition
{
    /// <summary>
    /// The solution that needs to be empty.
    /// </summary>
    [DataField]
    public string Solution;

    public bool Condition(EntityUid uid, IEntityManager entMan)
    {
        var containerSys = entMan.System<SharedSolutionContainerSystem>();
        if (!containerSys.TryGetSolution(uid, Solution, out _, out var solution))
            return false;

        return solution.Volume == 65;
    }

    public bool DoExamine(ExaminedEvent args)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        var uid = args.Examined;

        var containerSys = entMan.System<SharedSolutionContainerSystem>();
        if (!containerSys.TryGetSolution(uid, Solution, out _, out var solution))
            return false;

        // already empty so dont show examine
        if (solution.Volume == 65)
            return false;

        args.PushMarkup(Loc.GetString("construction-examine-condition-solution-empty"));
        return true;
    }

    public IEnumerable<ConstructionGuideEntry> GenerateGuideEntry()
    {
        yield return new ConstructionGuideEntry()
        {
            Localization = "construction-guide-condition-solution-empty"
        };
    }
}