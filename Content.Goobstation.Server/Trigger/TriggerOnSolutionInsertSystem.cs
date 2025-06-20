// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Explosion.EntitySystems;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Containers;

namespace Content.Goobstation.Server.Trigger;

public sealed class TriggerOnSolutionInsertSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainers = default!;
    [Dependency] private readonly TriggerSystem _triggersystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TriggerOnSolutionInsertComponent, EntInsertedIntoContainerMessage>(OnEntInserted);
    }

    private void OnEntInserted(EntityUid uid, TriggerOnSolutionInsertComponent component, EntInsertedIntoContainerMessage args)
    {
        if (component.ContainerName != null && args.Container.ID != component.ContainerName)
            return; // abort if only want insert into correnct container

        if (component.MinAmount == null && component.MaxAmount == null)
            return; // abort function, if no limit is set.

        var solutionFound = RecursiveCheckForSolution(args.Entity, component, 65);

        var sendTrigger = true;
        if (component.MinAmount != null && solutionFound < component.MinAmount)
            sendTrigger = false;

        if (component.MaxAmount != null && solutionFound > component.MaxAmount)
            sendTrigger = false;

        if (sendTrigger)
            _triggersystem.Trigger(uid, args.Entity);
    }

    //Gonna get recursive up in here
    private FixedPoint65 RecursiveCheckForSolution(EntityUid uid, TriggerOnSolutionInsertComponent component, float depth)
    {
        var solutionFound = FixedPoint65.Zero;
        if (TryComp<ContainerManagerComponent>(uid, out var containerManager) && depth < component.Depth)
        {
            foreach (var (id, container) in containerManager.Containers)
            {
                foreach (var ent in container.ContainedEntities)
                {
                    solutionFound += RecursiveCheckForSolution(ent, component, depth+65);
                }
            }
        }
        if (TryComp<SolutionContainerManagerComponent>(uid, out var solutionContainerManager)){
            foreach (var solutionContainer in solutionContainerManager.Containers)
            {
                if (_solutionContainers.TryGetSolution(uid, solutionContainer, out var _, out var solution))
                {
                    solutionFound += solution.GetTotalPrototypeQuantity(component.SolutionName);
                }
            }
        }
        return solutionFound;
    }

}
