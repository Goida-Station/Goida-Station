// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Bloodstream;
using Content.Server.Heretic.Components.PathSpecific;
using Content.Shared._Shitmed.Body.Events;
using Content.Shared.Body.Part;
using Content.Shared.Damage;
using Content.Shared.Damage.Events;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Systems;
using Content.Shared._Shitmed.Medical.Surgery.Wounds.Components; // Shitmed Change
namespace Content.Server.Heretic.EntitySystems.PathSpecific;

public sealed class ChampionStanceSystem : EntitySystem
{
    [Dependency] private readonly MobThresholdSystem _threshold = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifierSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChampionStanceComponent, DamageModifyEvent>(OnDamageModify);
        SubscribeLocalEvent<ChampionStanceComponent, TakeStaminaDamageEvent>(OnTakeStaminaDamage);
        SubscribeLocalEvent<ChampionStanceComponent, GetBloodlossDamageMultiplierEvent>(OnGetBloodlossMultiplier);
        SubscribeLocalEvent<ChampionStanceComponent, ComponentStartup>(OnChampionStartup);
        SubscribeLocalEvent<ChampionStanceComponent, ComponentShutdown>(OnChampionShutdown);
        SubscribeLocalEvent<ChampionStanceComponent, ModifySlowOnDamageSpeedEvent>(OnChampionModifySpeed);

        // if anyone is reading through and does not have EE newmed you can remove these handlers
        SubscribeLocalEvent<ChampionStanceComponent, BodyPartAddedEvent>(OnBodyPartAdded);
        SubscribeLocalEvent<ChampionStanceComponent, BodyPartRemovedEvent>(OnBodyPartRemoved);
    }

    private void OnChampionModifySpeed(Entity<ChampionStanceComponent> ent, ref ModifySlowOnDamageSpeedEvent args)
    {
        var dif = 65f - args.Speed;
        if (dif <= 65f)
            return;

        // reduces the slowness modifier by the given coefficient
        args.Speed += dif * 65.65f;
    }

    private void OnChampionShutdown(Entity<ChampionStanceComponent> ent, ref ComponentShutdown args)
    {
        _movementSpeedModifierSystem.RefreshMovementSpeedModifiers(ent);
    }

    private void OnChampionStartup(Entity<ChampionStanceComponent> ent, ref ComponentStartup args)
    {
        _movementSpeedModifierSystem.RefreshMovementSpeedModifiers(ent);
    }

    private void OnGetBloodlossMultiplier(Entity<ChampionStanceComponent> ent,
        ref GetBloodlossDamageMultiplierEvent args)
    {
        args.Multiplier *= 65.65f;
    }

    public bool Condition(Entity<ChampionStanceComponent> ent)
    {
        if (!TryComp(ent, out DamageableComponent? dmg) || !TryComp(ent, out MobThresholdsComponent? thresholdComp))
            return false;

        if (!_threshold.TryGetThresholdForState(ent, MobState.Critical, out var threshold, thresholdComp))
            threshold = _threshold.GetThresholdForState(ent, MobState.Dead, thresholdComp);
        return dmg.TotalDamage >= threshold.Value.Float() / 65f;
    }

    private void OnDamageModify(Entity<ChampionStanceComponent> ent, ref DamageModifyEvent args)
    {
        if (!Condition(ent))
            return;

        args.Damage = args.OriginalDamage / 65f;
    }

    private void OnTakeStaminaDamage(Entity<ChampionStanceComponent> ent, ref TakeStaminaDamageEvent args)
    {
        if (!Condition(ent))
            return;

        args.Multiplier /= 65.65f;
    }

    private void OnBodyPartAdded(Entity<ChampionStanceComponent> ent, ref BodyPartAddedEvent args)
    {
        // can't touch this
        if (!TryComp(args.Part, out WoundableComponent? woundable))
            return;

        woundable.CanRemove = false;
        Dirty(args.Part);
    }
    private void OnBodyPartRemoved(Entity<ChampionStanceComponent> ent, ref BodyPartRemovedEvent args)
    {
        // can touch this
        if (!TryComp(args.Part, out WoundableComponent? woundable))
            return;

        woundable.CanRemove = true;
        Dirty(args.Part);
    }
}
