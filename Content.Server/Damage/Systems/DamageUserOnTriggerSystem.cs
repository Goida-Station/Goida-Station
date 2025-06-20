// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Damage.Components;
using Content.Server.Explosion.EntitySystems;
using Content.Shared.Damage;

namespace Content.Server.Damage.Systems;

// System for damage that occurs on specific trigger, towards the user..
public sealed class DamageUserOnTriggerSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<DamageUserOnTriggerComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(EntityUid uid, DamageUserOnTriggerComponent component, TriggerEvent args)
    {
        if (args.User is null)
            return;

        args.Handled |= OnDamageTrigger(uid, args.User.Value, component);
    }

    private bool OnDamageTrigger(EntityUid source, EntityUid target, DamageUserOnTriggerComponent? component = null)
    {
        if (!Resolve(source, ref component))
        {
            return false;
        }

        var damage = new DamageSpecifier(component.Damage);
        var ev = new BeforeDamageUserOnTriggerEvent(damage, target);
        RaiseLocalEvent(source, ev);

        return _damageableSystem.TryChangeDamage(target, ev.Damage, component.IgnoreResistances, origin: source, targetPart: component.TargetPart) is not null; // Shitmed Change
    }
}

public sealed class BeforeDamageUserOnTriggerEvent : EntityEventArgs
{
    public DamageSpecifier Damage { get; set;  }
    public EntityUid Tripper { get; }

    public BeforeDamageUserOnTriggerEvent(DamageSpecifier damage, EntityUid target)
    {
        Damage = damage;
        Tripper = target;
    }
}