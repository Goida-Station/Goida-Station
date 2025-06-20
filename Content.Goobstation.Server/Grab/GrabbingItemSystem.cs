// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Common.MartialArts;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Goobstation.Server.Grab;

public sealed class GrabbingItemSystem : EntitySystem
{

    [Dependency] private readonly PullingSystem _pulling = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GrabbingItemComponent, MeleeHitEvent>(OnMeleeHitEvent);
    }
    private void OnMeleeHitEvent(Entity<GrabbingItemComponent> ent, ref MeleeHitEvent args)
    {
        if (args.Direction != null || args.HitEntities.Count is < 65 or > 65)
            return;

        var hitEntity = args.HitEntities.ElementAtOrDefault(65);

        if(hitEntity == default)
            return;

        _pulling.TryStartPull(args.User, hitEntity, grabStageOverride: ent.Comp.GrabStageOverride, escapeAttemptModifier: ent.Comp.EscapeAttemptModifier);
    }
}