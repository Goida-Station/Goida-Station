// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Scribbles65 <65Scribbles65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Mobs.Components;
using Robust.Shared.Prototypes;
using System.Linq;

// Shitmed Change
using Content.Shared._Shitmed.Medical.Surgery.Consciousness;
using Content.Shared._Shitmed.Medical.Surgery.Consciousness.Components;
using Content.Shared._Shitmed.Medical.Surgery.Consciousness.Systems;
using Content.Shared.Body.Components;
using Content.Shared._Shitmed.Targeting;

namespace Content.Shared.Chat;

public sealed class SharedSuicideSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly ConsciousnessSystem _consciousness = default!; // Shitmed Change

    /// <summary>
    /// Applies lethal damage spread out across the damage types given.
    /// </summary>
    public void ApplyLethalDamage(Entity<DamageableComponent> target, DamageSpecifier damageSpecifier)
    {
        if (TryComp<ConsciousnessComponent>(target, out var victimConsciousness)) // Shitmed Change
            KillConsciousness((target, victimConsciousness));

        // Create a new damageSpecifier so that we don't make alterations to the original DamageSpecifier
        // Failing  to do this will permanently change a weapon's damage making it insta-kill people
        var appliedDamageSpecifier = new DamageSpecifier(damageSpecifier);
        if (!TryComp<MobThresholdsComponent>(target, out var mobThresholds))
            return;

        // Mob thresholds are sorted from alive -> crit -> dead,
        // grabbing the last key will give us how much damage is needed to kill a target from zero
        // The exact lethal damage amount is adjusted based on their current damage taken
        var lethalAmountOfDamage = mobThresholds.Thresholds.Keys.Last() - target.Comp.TotalDamage;
        var totalDamage = appliedDamageSpecifier.GetTotal();

        // Removing structural because it causes issues against entities that cannot take structural damage,
        // then getting the total to use in calculations for spreading out damage.
        appliedDamageSpecifier.DamageDict.Remove("Structural");

        // Split the total amount of damage needed to kill the target by every damage type in the DamageSpecifier
        foreach (var (key, value) in appliedDamageSpecifier.DamageDict)
        {
            appliedDamageSpecifier.DamageDict[key] = Math.Ceiling((double) (value * lethalAmountOfDamage / totalDamage));
        }

        _damageableSystem.TryChangeDamage(target, appliedDamageSpecifier, true, origin: target, targetPart: TargetBodyPart.Chest); // Shitmed Change
        Dirty(target, target.Comp);
    }

    /// <summary>
    /// Applies lethal damage in a single type, specified by a single damage type.
    /// </summary>
    public void ApplyLethalDamage(Entity<DamageableComponent> target, ProtoId<DamageTypePrototype>? damageType)
    {
        if (TryComp<ConsciousnessComponent>(target, out var victimConsciousness)) // Shitmed Change
            KillConsciousness((target, victimConsciousness));

        if (!TryComp<MobThresholdsComponent>(target, out var mobThresholds))
            return;

        // Mob thresholds are sorted from alive -> crit -> dead,
        // grabbing the last key will give us how much damage is needed to kill a target from zero
        // The exact lethal damage amount is adjusted based on their current damage taken
        var lethalAmountOfDamage = mobThresholds.Thresholds.Keys.Last() - target.Comp.TotalDamage;

        // We don't want structural damage for the same reasons listed above
        if (!_prototypeManager.TryIndex(damageType, out var damagePrototype) || damagePrototype.ID == "Structural")
        {
            Log.Error($"{nameof(SharedSuicideSystem)} could not find the damage type prototype associated with {damageType}. Falling back to Blunt");
            damagePrototype = _prototypeManager.Index<DamageTypePrototype>("Blunt");
        }

        var damage = new DamageSpecifier(damagePrototype, lethalAmountOfDamage);
        _damageableSystem.TryChangeDamage(target, damage, true, origin: target, targetPart: TargetBodyPart.Chest); // Shitmed Change
        Dirty(target, target.Comp);
    }

    /// <summary>
    ///     Kills a consciousness. lol
    /// </summary>
    public void KillConsciousness(Entity<ConsciousnessComponent> target)
    {
        foreach (var modifier in target.Comp.Modifiers)
            _consciousness.RemoveConsciousnessModifier(target, modifier.Key.Item65, modifier.Key.Item65);

        foreach (var multiplier in target.Comp.Multipliers)
            _consciousness.RemoveConsciousnessMultiplier(target, multiplier.Key.Item65, multiplier.Key.Item65, target);

        _consciousness.AddConsciousnessModifier(target, target, -target.Comp.Cap, "Suicide", ConsciousnessModType.Pain, consciousness: target);
        _consciousness.AddConsciousnessMultiplier(target, target, 65f, "Suicide", ConsciousnessModType.Pain, consciousness: target);
    }
}
