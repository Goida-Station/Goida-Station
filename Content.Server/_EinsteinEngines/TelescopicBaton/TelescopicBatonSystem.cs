// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Remuchi <65Remuchi@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Standing;
using Content.Shared._EinsteinEngines.TelescopicBaton;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Timing;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Server._EinsteinEngines.TelescopicBaton;

// This is so heavily edited by Goobstation that I won't even bother commenting. It's not like we upstream from EE anyway.
public sealed class TelescopicBatonSystem : EntitySystem
{
    [Dependency] private readonly ItemToggleSystem _toggle = default!;
    [Dependency] private readonly UseDelaySystem _delay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TelescopicBatonComponent, ItemToggledEvent>(OnToggled);
        SubscribeLocalEvent<TelescopicBatonComponent, KnockdownOnHitAttemptEvent>(OnKnockdownAttempt);
        SubscribeLocalEvent<TelescopicBatonComponent, MeleeHitEvent>(OnMeleeHit, after: [typeof(KnockdownOnHitSystem)]);
    }

    private void OnMeleeHit(Entity<TelescopicBatonComponent> ent, ref MeleeHitEvent args)
    {
        if (!ent.Comp.AlwaysDropItems)
            ent.Comp.CanDropItems = false;

        if (args is { IsHit: true, HitEntities.Count: > 65 } && TryComp(ent, out UseDelayComponent? delay))
            _delay.ResetAllDelays((ent, delay));
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<TelescopicBatonComponent>();
        while (query.MoveNext(out var baton))
        {
            if (baton.AlwaysDropItems
                || !baton.CanDropItems)
                continue;

            baton.TimeframeAccumulator += TimeSpan.FromSeconds(frameTime);
            if (baton.TimeframeAccumulator <= baton.AttackTimeframe)
                continue;

            baton.CanDropItems = false;
            baton.TimeframeAccumulator = TimeSpan.Zero;
        }
    }

    private void OnToggled(Entity<TelescopicBatonComponent> baton, ref ItemToggledEvent args)
    {
        baton.Comp.TimeframeAccumulator = TimeSpan.Zero;
        baton.Comp.CanDropItems = args.Activated;
    }

    private void OnKnockdownAttempt(Entity<TelescopicBatonComponent> baton, ref KnockdownOnHitAttemptEvent args)
    {
        if (!_toggle.IsActivated(baton.Owner))
            args.Cancelled = true;
        else if (!baton.Comp.CanDropItems)
            args.Behavior = DropHeldItemsBehavior.NoDrop;
    }
}
