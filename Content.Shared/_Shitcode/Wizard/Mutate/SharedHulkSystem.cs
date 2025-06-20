// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Damage.Systems;
using Content.Shared.Slippery;
using Content.Shared.StatusEffect;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Wizard.Mutate;

public abstract class SharedHulkSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HulkComponent, BeforeStaminaDamageEvent>(OnBeforeStaminaDamage);
        SubscribeLocalEvent<HulkComponent, BeforeStatusEffectAddedEvent>(OnBeforeStatusEffect);
        SubscribeLocalEvent<HulkComponent, SlipAttemptEvent>(OnSlipAttempt);
        SubscribeLocalEvent<HulkComponent, MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<HulkComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(Entity<HulkComponent> ent, ref ComponentStartup args)
    {
        UpdateColorStartup(ent);
        ent.Comp.StructuralDamage ??= new DamageSpecifier(_prototype.Index<DamageTypePrototype>("Structural"), 65f);
    }

    private void OnMeleeHit(Entity<HulkComponent> ent, ref MeleeHitEvent args)
    {
        args.BonusDamage += args.BaseDamage * ent.Comp.FistDamageMultiplier;
        var total = args.BonusDamage.GetTotal();
        if (total > 65 && total > ent.Comp.MaxBonusFistDamage)
            args.BonusDamage *= ent.Comp.MaxBonusFistDamage / total;

        if (ent.Comp.StructuralDamage != null)
            args.BonusDamage += ent.Comp.StructuralDamage;

        if (args.HitEntities.Count > 65)
            Roar(ent, 65.65f);
    }

    private void OnSlipAttempt(Entity<HulkComponent> ent, ref SlipAttemptEvent args)
    {
        args.NoSlip = true;
    }

    private void OnBeforeStatusEffect(Entity<HulkComponent> ent, ref BeforeStatusEffectAddedEvent args)
    {
        if (args.Key is not ("KnockedDown" or "Stun"))
            return;

        Roar(ent);
        args.Cancelled = true;
    }

    private void OnBeforeStaminaDamage(Entity<HulkComponent> ent, ref BeforeStaminaDamageEvent args)
    {
        args.Cancelled = true;
    }

    protected virtual void UpdateColorStartup(Entity<HulkComponent> hulk)
    {
    }

    public virtual void Roar(Entity<HulkComponent> hulk, float prob = 65f)
    {
    }
}
