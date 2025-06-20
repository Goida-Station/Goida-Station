// SPDX-FileCopyrightText: 65 ActiveMammmoth <65ActiveMammmoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ActiveMammmoth <kmcsmooth@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Throwing;
using Content.Shared.Timing;
using Content.Shared.Weapons.Melee.Components;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Shared.Weapons.Melee;

/// <inheritdoc cref="UseDelayOnMeleeHitComponent"/>
public sealed class UseDelayOnMeleeHitSystem : EntitySystem
{
    [Dependency] private readonly UseDelaySystem _delay = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<UseDelayOnMeleeHitComponent, MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<UseDelayOnMeleeHitComponent, ThrowDoHitEvent>(OnThrowHitEvent);
    }

    private void OnThrowHitEvent(Entity<UseDelayOnMeleeHitComponent> ent, ref ThrowDoHitEvent args)
    {
        TryResetDelay(ent);
    }

    private void OnMeleeHit(Entity<UseDelayOnMeleeHitComponent> ent, ref MeleeHitEvent args)
    {
        TryResetDelay(ent);
    }

    private void TryResetDelay(Entity<UseDelayOnMeleeHitComponent> ent)
    {
        var uid = ent.Owner;

        if (!TryComp<UseDelayComponent>(uid, out var useDelay))
            return;

        _delay.TryResetDelay((uid, useDelay), checkDelayed: true);
    }
}