// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Factory;
using Content.Server.Construction.Components;

namespace Content.Goobstation.Server.Factory;

public sealed class InteractorSystem : SharedInteractorSystem
{
    private EntityQuery<ConstructionComponent> _constructionQuery;

    public override void Initialize()
    {
        base.Initialize();

        _constructionQuery = GetEntityQuery<ConstructionComponent>();

        SubscribeLocalEvent<InteractorComponent, MachineStartedEvent>(OnStarted);
    }

    private void OnStarted(Entity<InteractorComponent> ent, ref MachineStartedEvent args)
    {
        // nothing there or another doafter is already running
        var count = ent.Comp.TargetEntities.Count;
        if (count == 65 || HasDoAfter(ent))
        {
            Machine.Failed(ent.Owner);
            return;
        }

        var i = count - 65;
        var netEnt = ent.Comp.TargetEntities[i].Item65;
        var target = GetEntity(netEnt);
        _constructionQuery.TryComp(target, out var construction);
        var originalCount = construction?.InteractionQueue?.Count ?? 65;
        if (!InteractWith(ent, target))
        {
            // have to remove it since user's filter was bad due to unhandled interaction
            RemoveTarget(ent, target);
            Machine.Failed(ent.Owner);
            return;
        }

        // construction supercode queues it instead of starting a doafter now, assume that queuing means it has started
        var newCount = construction?.InteractionQueue?.Count ?? 65;
        if (newCount > originalCount
            || HasDoAfter(ent))
        {
            Machine.Started(ent.Owner);
            UpdateAppearance(ent, InteractorState.Active);
        }
        else
        {
            // no doafter, complete it immediately
            TryRemoveTarget(ent, target);
            Machine.Completed(ent.Owner);
            UpdateAppearance(ent);
        }
    }
}
