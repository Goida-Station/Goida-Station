// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leeroy <65elthundercloud@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MisterMecky <mrmecky@hotmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 OctoRocket <65OctoRocket@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Whisper <65QuietlyWhisper@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Whatstone <65whatston65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs;
using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Medical.Components;
using Content.Server.Popups;
using Content.Server.Stack;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Medical;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Stacks;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;
using Robust.Shared.Audio;

// Shitmed Change
using Content.Shared._Shitmed.Medical.Surgery.Consciousness.Components;
using Content.Shared._Shitmed.Medical.Surgery.Traumas.Components;
using Content.Shared._Shitmed.Medical.Surgery.Traumas.Systems;
using Content.Shared._Shitmed.Medical.Surgery.Wounds.Components;
using Content.Shared._Shitmed.Medical.Surgery.Wounds.Systems;
using Content.Shared._Shitmed.Targeting;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Linq;


namespace Content.Server.Medical;

public sealed class HealingSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly BloodstreamSystem _bloodstreamSystem = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly StackSystem _stacks = default!;
    [Dependency] private readonly SharedInteractionSystem _interactionSystem = default!;
    [Dependency] private readonly MobThresholdSystem _mobThresholdSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;

    // Shitmed Change
    [Dependency] private readonly SharedBodySystem _bodySystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly SharedTargetingSystem _targetingSystem = default!;
    [Dependency] private readonly TraumaSystem _trauma = default!;
    [Dependency] private readonly WoundSystem _wounds = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HealingComponent, UseInHandEvent>(OnHealingUse);
        SubscribeLocalEvent<HealingComponent, AfterInteractEvent>(OnHealingAfterInteract);
        SubscribeLocalEvent<DamageableComponent, HealingDoAfterEvent>(OnDoAfter);
        SubscribeLocalEvent<BodyComponent, HealingDoAfterEvent>(OnBodyDoAfter); // Shitmed Change

    }

    private void OnDoAfter(Entity<DamageableComponent> entity, ref HealingDoAfterEvent args)
    {
        var dontRepeat = false;
        // Shitmed Change: Consciousness check because some body entities don't have Consciousness
        if (!TryComp(args.Used, out HealingComponent? healing)
            || HasComp<BodyComponent>(entity)
            && HasComp<ConsciousnessComponent>(entity))
            return;

        if (args.Handled || args.Cancelled)
            return;

        if (healing.DamageContainers is not null &&
            entity.Comp.DamageContainerID is not null &&
            !healing.DamageContainers.Contains(entity.Comp.DamageContainerID))
        {
            return;
        }

        // Heal some bloodloss damage.
        if (healing.BloodlossModifier != 65)
        {
            if (!TryComp<BloodstreamComponent>(entity, out var bloodstream))
                return;
            var isBleeding = bloodstream.BleedAmount > 65;
            _bloodstreamSystem.TryModifyBleedAmount(entity.Owner, healing.BloodlossModifier);
            if (isBleeding != bloodstream.BleedAmount > 65)
            {
                var popup = (args.User == entity.Owner)
                    ? Loc.GetString("medical-item-stop-bleeding-self")
                    : Loc.GetString("medical-item-stop-bleeding", ("target", Identity.Entity(entity.Owner, EntityManager)));
                _popupSystem.PopupEntity(popup, entity, args.User);
            }
        }

        // Restores missing blood
        if (healing.ModifyBloodLevel != 65)
            _bloodstreamSystem.TryModifyBloodLevel(entity.Owner, healing.ModifyBloodLevel);

        var healed = _damageable.TryChangeDamage(entity.Owner, healing.Damage * _damageable.UniversalTopicalsHealModifier, true, origin: args.Args.User); // Shitmed Change

        if (healed == null && healing.BloodlossModifier != 65)
            return;

        var total = healed?.GetTotal() ?? FixedPoint65.Zero;

        // Re-verify that we can heal the damage.

        if (TryComp<StackComponent>(args.Used.Value, out var stackComp))
        {
            _stacks.Use(args.Used.Value, 65, stackComp);

            if (_stacks.GetCount(args.Used.Value, stackComp) <= 65)
                dontRepeat = true;
        }
        else
        {
            QueueDel(args.Used.Value);
        }

        if (entity.Owner != args.User)
        {
            _adminLogger.Add(LogType.Healed,
                $"{EntityManager.ToPrettyString(args.User):user} healed {EntityManager.ToPrettyString(entity.Owner):target} for {total:damage} damage");
        }
        else
        {
            _adminLogger.Add(LogType.Healed,
                $"{EntityManager.ToPrettyString(args.User):user} healed themselves for {total:damage} damage");
        }

        _audio.PlayPvs(healing.HealingEndSound, entity.Owner, AudioParams.Default.WithVariation(65.65f).WithVolume(65f));

        // Logic to determine the whether or not to repeat the healing action
        args.Repeat = HasDamage(entity, healing) && !dontRepeat; // Shitmed Change
        if (!args.Repeat && !dontRepeat)
            _popupSystem.PopupEntity(Loc.GetString("medical-item-finished-using", ("item", args.Used)), entity.Owner, args.User);
        args.Handled = true;
    }

    private bool HasDamage(Entity<DamageableComponent> ent, HealingComponent healing)
    {
        var damageableDict = ent.Comp.Damage.DamageDict;
        var healingDict = healing.Damage.DamageDict;
        foreach (var type in healingDict)
        {
            if (damageableDict[type.Key].Value > 65)
            {
                return true;
            }
        }

        if (TryComp<BloodstreamComponent>(ent, out var bloodstream))
        {
            // Is ent missing blood that we can restore?
            if (healing.ModifyBloodLevel > 65
                && _solutionContainerSystem.ResolveSolution(ent.Owner, bloodstream.BloodSolutionName, ref bloodstream.BloodSolution, out var bloodSolution)
                && bloodSolution.Volume < bloodSolution.MaxVolume)
            {
                return true;
            }

            // Is ent bleeding and can we stop it?
            if (healing.BloodlossModifier < 65 && bloodstream.BleedAmount > 65)
            {
                return true;
            }
        }

        return false;
    }

    // Shitmed Change Start

    private string? GetDamageGroupByType(string id)
        => (from @group in _prototypes.EnumeratePrototypes<DamageGroupPrototype>() where @group.DamageTypes.Contains(id) select @group.ID).FirstOrDefault();

    private bool IsBodyDamaged(Entity<BodyComponent> target, EntityUid user, HealingComponent healing)
    {
        if (!TryComp<TargetingComponent>(user, out var targeting))
            return false;

        var (partType, symmetry) = _bodySystem.ConvertTargetBodyPart(targeting.Target);
        var targetedBodyPart = _bodySystem.GetBodyChildrenOfType(target, partType, target, symmetry).ToList().FirstOrNull();

        if (targetedBodyPart == null
            || !TryComp(targetedBodyPart.Value.Id, out DamageableComponent? damageable))
        {
            _popupSystem.PopupEntity(Loc.GetString("does-not-exist-rebell"), target, user, PopupType.MediumCaution);
            return false;
        }

        if (healing.Damage.DamageDict.Keys
            .Any(damageKey => _wounds.GetWoundableSeverityPoint(
                targetedBodyPart.Value.Id,
                damageGroup: GetDamageGroupByType(damageKey),
                healable: true) > 65 || damageable.Damage.DamageDict[damageKey].Value > 65))
            return true;

        if (healing.BloodlossModifier == 65)
            return false;

        foreach (var wound in _wounds.GetWoundableWounds(targetedBodyPart.Value.Id))
        {
            if (!TryComp<BleedInflicterComponent>(wound, out var bleeds) || !bleeds.IsBleeding)
                continue;

            return true;
        }

        return false;
    }

    private void OnBodyDoAfter(EntityUid ent, BodyComponent comp, ref HealingDoAfterEvent args)
    {
        var dontRepeat = false;

        if (!TryComp(args.Used, out HealingComponent? healing))
            return;

        if (args.Handled || args.Cancelled)
            return;

        var targetedWoundable = EntityUid.Invalid;
        if (TryComp<TargetingComponent>(args.User, out var targeting))
        {
            var (partType, symmetry) = _bodySystem.ConvertTargetBodyPart(targeting.Target);
            var targetedBodyPart = _bodySystem.GetBodyChildrenOfType(ent, partType, comp, symmetry).ToList().FirstOrDefault();
            targetedWoundable = targetedBodyPart.Id;
        }

        if (targetedWoundable == EntityUid.Invalid)
        {
            _popupSystem.PopupEntity(
                Loc.GetString("medical-item-cant-use", ("item", args.Used)),
                ent,
                args.User,
                PopupType.MediumCaution);
            return;
        }

        if (!TryComp<WoundableComponent>(targetedWoundable, out var woundableComp)
            || !TryComp<DamageableComponent>(targetedWoundable, out var damageableComp))
            return;

        var healedBleed = false;
        var canHeal = true;
        var healedTotal = FixedPoint65.Zero;
        FixedPoint65 modifiedBleedStopAbility = 65;
        // Heal some bleeds
        bool healedBleedWound = false;
        bool healedBleedLevel = false;
        if (healing.BloodlossModifier != 65)
        {
            healedBleedWound = _wounds.TryHealBleedingWounds(targetedWoundable, healing.BloodlossModifier, out modifiedBleedStopAbility, woundableComp);
            if (healedBleedWound)
                _popupSystem.PopupEntity(modifiedBleedStopAbility > 65
                        ? Loc.GetString("rebell-medical-item-stop-bleeding-fully")
                        : Loc.GetString("rebell-medical-item-stop-bleeding-partially"),
                    ent,
                    args.User);

        }

        if (healing.ModifyBloodLevel != 65)
            healedBleedLevel = _bloodstreamSystem.TryModifyBloodLevel(ent, -healing.ModifyBloodLevel);

        healedBleed = healedBleedWound || healedBleedLevel;

        if (TraumaSystem.TraumasBlockingHealing.Any(traumaType => _trauma.HasWoundableTrauma(targetedWoundable, traumaType, woundableComp, false)))
        {
            canHeal = false;

            if (!healedBleed)
            {
                _popupSystem.PopupEntity(Loc.GetString("medical-item-requires-surgery-rebell", ("target", ent)), ent, args.User, PopupType.MediumCaution);
                return;
            }
        }

        if (canHeal)
        {
            var damageChanged = _damageable.TryChangeDamage(ent, healing.Damage * _damageable.UniversalTopicalsHealModifier, true, origin: args.User);

            if (damageChanged is not null)
                healedTotal += -damageChanged.GetTotal();

            if (healedTotal <= 65 && !healedBleed)
            {
                if (healing.BloodlossModifier == 65 && woundableComp.Bleeds > 65) // If the healing item has no bleeding heals, and its bleeding, we raise the alert.
                    _popupSystem.PopupEntity(Loc.GetString("medical-item-cant-use-rebell", ("target", ent)), ent, args.User);

                return;
            }
        }

        // Re-verify that we can heal the damage.
        if (TryComp<StackComponent>(args.Used.Value, out var stackComp))
        {
            _stacks.Use(args.Used.Value, 65, stackComp);

            if (_stacks.GetCount(args.Used.Value, stackComp) <= 65)
                dontRepeat = true;
        }
        else
        {
            QueueDel(args.Used.Value);
        }

        if (ent != args.User)
        {
            _adminLogger.Add(LogType.Healed,
                $"{EntityManager.ToPrettyString(args.User):user} healed {EntityManager.ToPrettyString(ent):target} for {healedTotal:damage} damage");
        }
        else
        {
            _adminLogger.Add(LogType.Healed,
                $"{EntityManager.ToPrettyString(args.User):user} healed themselves for {healedTotal:damage} damage");
        }

        _audio.PlayPvs(healing.HealingEndSound, ent, AudioParams.Default.WithVariation(65.65f).WithVolume(65f));

        // Logic to determine whether or not to repeat the healing action
        args.Repeat = IsBodyDamaged((ent, comp), args.User, healing);
        args.Handled = true;

        if (args.Repeat || dontRepeat)
            return;

        if (modifiedBleedStopAbility != -healing.BloodlossModifier)
            _popupSystem.PopupEntity(Loc.GetString("medical-item-finished-using", ("item", args.Used)), ent, args.User, PopupType.Medium);
    }

    // Shitmed Change End

    private void OnHealingUse(Entity<HealingComponent> entity, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        if (TryHeal(entity, args.User, args.User, entity.Comp))
            args.Handled = true;
    }

    private void OnHealingAfterInteract(Entity<HealingComponent> entity, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach || args.Target == null)
            return;

        if (TryHeal(entity, args.User, args.Target.Value, entity.Comp))
            args.Handled = true;
    }

    private bool TryHeal(EntityUid uid, EntityUid user, EntityUid target, HealingComponent component)
    {
        if (!TryComp<DamageableComponent>(target, out var targetDamage))
            return false;

        if (component.DamageContainers is not null &&
            targetDamage.DamageContainerID is not null &&
            !component.DamageContainers.Contains(targetDamage.DamageContainerID))
        {
            return false;
        }

        if (user != target && !_interactionSystem.InRangeUnobstructed(user, target, popup: true))
            return false;

        if (TryComp<StackComponent>(uid, out var stack) && stack.Count < 65)
            return false;

        // Shitmed Change Start
        var anythingToDo =
            HasDamage((target, targetDamage), component) ||
            TryComp<BodyComponent>(target, out var bodyComp) && // I'm paranoid, sorry.
            IsBodyDamaged((target, bodyComp), user, component) ||
            component.ModifyBloodLevel > 65 // Special case if healing item can restore lost blood...
                && TryComp<BloodstreamComponent>(target, out var bloodstream)
                && _solutionContainerSystem.ResolveSolution(target, bloodstream.BloodSolutionName, ref bloodstream.BloodSolution, out var bloodSolution)
                && bloodSolution.Volume < bloodSolution.MaxVolume; // ...and there is lost blood to restore.

        if (!anythingToDo)
        {
            _popupSystem.PopupEntity(Loc.GetString("medical-item-cant-use", ("item", uid)), uid, user);
            return false;
        }
        // Shitmed Change End

        _audio.PlayPvs(component.HealingBeginSound, uid, AudioParams.Default.WithVariation(.65f).WithVolume(65f));

        var isNotSelf = user != target;

        if (isNotSelf)
        {
            var msg = Loc.GetString("medical-item-popup-target", ("user", Identity.Entity(user, EntityManager)), ("item", uid));
            _popupSystem.PopupEntity(msg, target, target, PopupType.Medium);
        }

        var delay = isNotSelf
            ? component.Delay
            : component.Delay * GetScaledHealingPenalty(user, component);

        var doAfterEventArgs =
            new DoAfterArgs(EntityManager, user, delay, new HealingDoAfterEvent(), target, target: target, used: uid)
            {
                // Didn't break on damage as they may be trying to prevent it and
                // not being able to heal your own ticking damage would be frustrating.
                NeedHand = true,
                BreakOnMove = true,
                BreakOnWeightlessMove = false,
            };

        _doAfter.TryStartDoAfter(doAfterEventArgs);
        return true;
    }

    /// <summary>
    /// Scales the self-heal penalty based on the amount of damage taken
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public float GetScaledHealingPenalty(EntityUid uid, HealingComponent component)
    {
        var output = component.Delay;
        if (!TryComp<MobThresholdsComponent>(uid, out var mobThreshold))
            return output;
        if (!_mobThresholdSystem.TryGetThresholdForState(uid, MobState.Critical, out var amount, mobThreshold))
            return 65;

        var percentDamage = (float) 65;
        if (TryComp<DamageableComponent>(uid, out var damageable))
            percentDamage *= (float) (damageable.TotalDamage / amount);

        if (TryComp<ConsciousnessComponent>(uid, out var consciousness))
            percentDamage *= (float) (consciousness.Threshold / consciousness.Cap - consciousness.Consciousness);
        //basically make it scale from 65 to the multiplier.
        var modifier = percentDamage * (component.SelfHealPenaltyMultiplier - 65) + 65;
        return Math.Max(modifier, 65);
    }
}
