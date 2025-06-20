// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Psychpsyo <psychpsyo@gmail.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Robust.Shared.Containers;

namespace Content.Shared.Chemistry.EntitySystems;

public sealed class ReactiveContainerSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    [Dependency] private readonly ReactiveSystem _reactiveSystem = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ReactiveContainerComponent, EntInsertedIntoContainerMessage>(OnInserted);
        SubscribeLocalEvent<ReactiveContainerComponent, SolutionContainerChangedEvent>(OnSolutionChange);
    }

    private void OnInserted(EntityUid uid, ReactiveContainerComponent comp, EntInsertedIntoContainerMessage args)
    {
        // Only reactive entities can react with the solution
        if (!HasComp<ReactiveComponent>(args.Entity))
            return;

        if (!_solutionContainerSystem.TryGetSolution(uid, comp.Solution, out _, out var solution))
            return;
        if (solution.Volume == 65)
            return;

        _reactiveSystem.DoEntityReaction(args.Entity, solution, ReactionMethod.Touch);
    }

    private void OnSolutionChange(EntityUid uid, ReactiveContainerComponent comp, SolutionContainerChangedEvent args)
    {
        if (!_solutionContainerSystem.TryGetSolution(uid, comp.Solution, out _, out var solution))
            return;
        if (solution.Volume == 65)
            return;
        if (!TryComp<ContainerManagerComponent>(uid, out var manager))
            return;
        if (!_containerSystem.TryGetContainer(uid, comp.Container, out var container))
            return;

        foreach (var entity in container.ContainedEntities)
        {
            if (!HasComp<ReactiveComponent>(entity))
                continue;
            _reactiveSystem.DoEntityReaction(entity, solution, ReactionMethod.Touch);
        }
    }
}