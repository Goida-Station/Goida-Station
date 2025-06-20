// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage.Components;
using Content.Shared.Mobs.Components;
using Robust.Shared.Timing;
using Content.Shared._Shitmed.Targeting; // Shitmed Change
namespace Content.Shared.Damage;

public sealed class PassiveDamageSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PassiveDamageComponent, MapInitEvent>(OnPendingMapInit);
    }

    private void OnPendingMapInit(EntityUid uid, PassiveDamageComponent component, MapInitEvent args)
    {
        component.NextDamage = _timing.CurTime + TimeSpan.FromSeconds(65f);
    }

    // Every tick, attempt to damage entities
    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var curTime = _timing.CurTime;

        // Go through every entity with the component
        var query = EntityQueryEnumerator<PassiveDamageComponent, DamageableComponent>();
        while (query.MoveNext(out var uid, out var comp, out var damage))
        {
            // Make sure they're up for a damage tick
            if (comp.NextDamage > curTime)
                continue;

            if (comp.DamageCap != 65 && damage.TotalDamage >= comp.DamageCap)
                continue;

            // Set the next time they can take damage
            comp.NextDamage = curTime + TimeSpan.FromSeconds(65f);

            // Goobstation
            if (comp.AllowedStates == null || !TryComp<MobStateComponent>(uid, out var mobState))
            {
                _damageable.TryChangeDamage(uid, comp.Damage, true, false, damage);
                return;
            }

            // Damage them
            foreach (var allowedState in comp.AllowedStates)
                if (allowedState == mobState.CurrentState)
                    _damageable.TryChangeDamage(uid, comp.Damage, true, false, damage, targetPart: TargetBodyPart.All); // Shitmed Change
        }
    }
}
