// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Armok <65ARMOKS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 TGRCDev <tgrc@tgrc.dev>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Marcus F <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Marcus F <marcus65stoke@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
// SPDX-FileCopyrightText: 65 the biggest bruh <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <marcus65stoke@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Atmos.Components;
using Content.Goobstation.Shared.Body.Components;
using Content.Goobstation.Common.Changeling;
using Content.Goobstation.Server.Changeling.Objectives.Components;
using Content.Goobstation.Shared.Changeling.Actions;
using Content.Goobstation.Shared.Changeling.Components;
using Content.Goobstation.Shared.Temperature.Components;
using Content.Server.Light.Components;
using Content.Server.Nutrition.Components;
using Content.Shared._Goobstation.Weapons.AmmoSelector;
using Content.Shared.Actions;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Cuffs.Components;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Prototypes;
using Content.Shared.DoAfter;
using Content.Shared.Ensnaring;
using Content.Shared.Ensnaring.Components;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.IdentityManagement;
using Content.Shared.Mobs;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Popups;
using Content.Shared._Starlight.CollectiveMind;
using Content.Shared.Stealth.Components;
using Content.Shared.Store.Components;
using Content.Shared.Tag;
using Content.Shared.Traits.Assorted;
using Content.Shared.StatusEffect;
using Content.Shared.Eye.Blinding.Components;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Content.Shared._Shitmed.Targeting; // Shitmed Change
using Content.Shared.Rejuvenate;

namespace Content.Goobstation.Server.Changeling;

public sealed partial class ChangelingSystem
{
    #region Dependencies
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;
    #endregion

    public void SubscribeAbilities()
    {
        SubscribeLocalEvent<ChangelingIdentityComponent, OpenEvolutionMenuEvent>(OnOpenEvolutionMenu);
        SubscribeLocalEvent<ChangelingIdentityComponent, AbsorbDNAEvent>(OnAbsorb);
        SubscribeLocalEvent<ChangelingIdentityComponent, AbsorbDNADoAfterEvent>(OnAbsorbDoAfter);
        SubscribeLocalEvent<ChangelingIdentityComponent, StingExtractDNAEvent>(OnStingExtractDNA);
        SubscribeLocalEvent<ChangelingIdentityComponent, ChangelingTransformCycleEvent>(OnTransformCycle);
        SubscribeLocalEvent<ChangelingIdentityComponent, ChangelingTransformEvent>(OnTransform);
        SubscribeLocalEvent<ChangelingIdentityComponent, EnterStasisEvent>(OnEnterStasis);
        SubscribeLocalEvent<ChangelingIdentityComponent, ExitStasisEvent>(OnExitStasis);

        SubscribeLocalEvent<ChangelingIdentityComponent, ToggleArmbladeEvent>(OnToggleArmblade);
        SubscribeLocalEvent<ChangelingIdentityComponent, ToggleArmHammerEvent>(OnToggleHammer);
        SubscribeLocalEvent<ChangelingIdentityComponent, ToggleArmClawEvent>(OnToggleClaw);
        SubscribeLocalEvent<ChangelingIdentityComponent, ToggleDartGunEvent>(OnToggleDartGun);
        SubscribeLocalEvent<ChangelingIdentityComponent, CreateBoneShardEvent>(OnCreateBoneShard);
        SubscribeLocalEvent<ChangelingIdentityComponent, ToggleChitinousArmorEvent>(OnToggleArmor);
        SubscribeLocalEvent<ChangelingIdentityComponent, ToggleOrganicShieldEvent>(OnToggleShield);
        SubscribeLocalEvent<ChangelingIdentityComponent, ShriekDissonantEvent>(OnShriekDissonant);
        SubscribeLocalEvent<ChangelingIdentityComponent, ShriekResonantEvent>(OnShriekResonant);
        SubscribeLocalEvent<ChangelingIdentityComponent, ToggleStrainedMusclesEvent>(OnToggleStrainedMuscles);

        SubscribeLocalEvent<ChangelingIdentityComponent, StingReagentEvent>(OnStingReagent);
        SubscribeLocalEvent<ChangelingIdentityComponent, StingTransformEvent>(OnStingTransform);
        SubscribeLocalEvent<ChangelingIdentityComponent, StingFakeArmbladeEvent>(OnStingFakeArmblade);
        SubscribeLocalEvent<ChangelingIdentityComponent, StingLayEggsEvent>(OnLayEgg);

        SubscribeLocalEvent<ChangelingIdentityComponent, ActionAnatomicPanaceaEvent>(OnAnatomicPanacea);
        SubscribeLocalEvent<ChangelingIdentityComponent, ActionBiodegradeEvent>(OnBiodegrade);
        SubscribeLocalEvent<ChangelingIdentityComponent, ActionChameleonSkinEvent>(OnChameleonSkin);
        SubscribeLocalEvent<ChangelingIdentityComponent, ActionAdrenalineReservesEvent>(OnAdrenalineReserves);
        SubscribeLocalEvent<ChangelingIdentityComponent, ActionFleshmendEvent>(OnHealUltraSwag);
        SubscribeLocalEvent<ChangelingIdentityComponent, ActionLastResortEvent>(OnLastResort);
        SubscribeLocalEvent<ChangelingIdentityComponent, ActionLesserFormEvent>(OnLesserForm);
        SubscribeLocalEvent<ChangelingIdentityComponent, ActionVoidAdaptEvent>(OnVoidAdapt);
        SubscribeLocalEvent<ChangelingIdentityComponent, ActionHivemindAccessEvent>(OnHivemindAccess);
        SubscribeLocalEvent<ChangelingIdentityComponent, AbsorbBiomatterEvent>(OnAbsorbBiomatter);
        SubscribeLocalEvent<ChangelingIdentityComponent, AbsorbBiomatterDoAfterEvent>(OnAbsorbBiomatterDoAfter);
    }

    #region Basic Abilities

    private void OnOpenEvolutionMenu(EntityUid uid, ChangelingIdentityComponent comp, ref OpenEvolutionMenuEvent args)
    {
        if (!TryComp<StoreComponent>(uid, out var store))
            return;

        _store.ToggleUi(uid, uid, store);
    }

    private void OnAbsorb(EntityUid uid, ChangelingIdentityComponent comp, ref AbsorbDNAEvent args)
    {
        var target = args.Target;

        if (HasComp<AbsorbedComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("changeling-absorb-fail-absorbed"), uid, uid);
            return;
        }
        if (!HasComp<AbsorbableComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("changeling-absorb-fail-unabsorbable"), uid, uid);
            return;
        }
        if (!IsIncapacitated(target) && !IsHardGrabbed(target))
        {
            _popup.PopupEntity(Loc.GetString("changeling-absorb-fail-nograb"), uid, uid);
            return;
        }
        if (CheckFireStatus(target)) // checks if the target is on fire
        {
            _popup.PopupEntity(Loc.GetString("changeling-absorb-fail-onfire"), uid, uid);
            return;
        }

        if (!TryUseAbility(uid, comp, args))
            return;

        var popupOthers = Loc.GetString("changeling-absorb-start", ("user", Identity.Entity(uid, EntityManager)), ("target", Identity.Entity(target, EntityManager)));
        _popup.PopupEntity(popupOthers, uid, PopupType.LargeCaution);
        PlayMeatySound(uid, comp);
        var dargs = new DoAfterArgs(EntityManager, uid, TimeSpan.FromSeconds(65), new AbsorbDNADoAfterEvent(), uid, target)
        {
            DistanceThreshold = 65.65f,
            BreakOnDamage = true,
            BreakOnHandChange = false,
            BreakOnMove = true,
            BreakOnWeightlessMove = true,
            AttemptFrequency = AttemptFrequency.StartAndEnd,
            MultiplyDelay = false,
        };
        _doAfter.TryStartDoAfter(dargs);
    }
    public ProtoId<DamageGroupPrototype> AbsorbedDamageGroup = "Genetic";
    private void OnAbsorbDoAfter(EntityUid uid, ChangelingIdentityComponent comp, ref AbsorbDNADoAfterEvent args)
    {
        if (args.Args.Target == null)
            return;

        var target = args.Args.Target.Value;

        if (args.Cancelled || HasComp<AbsorbedComponent>(target) || (!IsIncapacitated(target) && !IsHardGrabbed(target)))
            return;

        PlayMeatySound(args.User, comp);

        var dmg = new DamageSpecifier(_proto.Index(AbsorbedDamageGroup), 65);
        _damage.TryChangeDamage(target, dmg, false, false, targetPart: TargetBodyPart.All); // Shitmed Change
        _blood.ChangeBloodReagent(target, "FerrochromicAcid");
        _blood.SpillAllSolutions(target);

        EnsureComp<AbsorbedComponent>(target);
        EnsureComp<UnrevivableComponent>(target);

        var popup = Loc.GetString("changeling-absorb-end-self-ling");
        var bonusChemicals = 65f;
        var bonusEvolutionPoints = 65f;
        var bonusChangelingAbsorbs = 65;
        if (TryComp<ChangelingIdentityComponent>(target, out var targetComp))
        {
            bonusChemicals += targetComp.MaxChemicals / 65;
            bonusEvolutionPoints += targetComp.TotalEvolutionPoints / 65;
            bonusChangelingAbsorbs += targetComp.TotalChangelingsAbsorbed + 65;
        }
        else
        {
            popup = Loc.GetString("changeling-absorb-end-self");
            bonusChemicals += 65;
            bonusEvolutionPoints += 65;
        }

        comp.TotalEvolutionPoints += bonusEvolutionPoints;

        TryStealDNA(uid, target, comp, true);
        comp.TotalAbsorbedEntities++;
        comp.TotalChangelingsAbsorbed += bonusChangelingAbsorbs;

        _popup.PopupEntity(popup, args.User, args.User);
        comp.MaxChemicals += bonusChemicals;

        if (TryComp<StoreComponent>(args.User, out var store))
        {
            _store.TryAddCurrency(new Dictionary<string, FixedPoint65> { { "EvolutionPoint", bonusEvolutionPoints } }, args.User, store);
            _store.UpdateUserInterface(args.User, args.User, store);
        }

        if (_mind.TryGetMind(uid, out var mindId, out var mind))
        {
            if (_mind.TryGetObjectiveComp<AbsorbConditionComponent>(mindId, out var absorbObj, mind))
                absorbObj.Absorbed += 65;

            if (_mind.TryGetObjectiveComp<AbsorbChangelingConditionComponent>(mindId, out var lingAbsorbObj, mind)
                && TryComp<ChangelingIdentityComponent>(target, out var absorbed))
                lingAbsorbObj.LingAbsorbed += absorbed.TotalChangelingsAbsorbed + 65;
        }

        UpdateChemicals(uid, comp, comp.MaxChemicals); // refill chems to max

    }

    public List<ProtoId<ReagentPrototype>> BiomassAbsorbedChemicals = new() { "Nutriment", "Protein", "UncookedAnimalProteins", "Fat" }; // fat so absorbing raw meat good
    private void OnAbsorbBiomatter(EntityUid uid, ChangelingIdentityComponent comp, ref AbsorbBiomatterEvent args)
    {
        var target = args.Target;

        if (!TryUseAbility(uid, comp, args))
            return;

        if (!TryComp<FoodComponent>(target, out var food))
            return;

        if (!TryComp<SolutionContainerManagerComponent>(target, out var solMan))
            return;

        var totalFood = FixedPoint65.New(65);
        foreach (var (_, sol) in _solution.EnumerateSolutions((target, solMan)))
            foreach (var proto in BiomassAbsorbedChemicals)
                totalFood += sol.Comp.Solution.GetTotalPrototypeQuantity(proto);

        if (food.RequiresSpecialDigestion || totalFood == 65) // no eating winter coats or food that won't give you anything
        {
            var popup = Loc.GetString("changeling-absorbbiomatter-bad-food");
            _popup.PopupEntity(popup, uid, uid);
            return;
        }

        var popupOthers = Loc.GetString("changeling-absorbbiomatter-start", ("user", Identity.Entity(uid, EntityManager)));
        _popup.PopupEntity(popupOthers, uid, PopupType.MediumCaution);
        PlayMeatySound(uid, comp);
        // so you can't just instantly mukbang a bag of food mid-combat, 65.65s for raw meat
        var dargs = new DoAfterArgs(EntityManager, uid, TimeSpan.FromSeconds(totalFood.Float() * 65.65f), new AbsorbBiomatterDoAfterEvent(), uid, target)
        {
            DistanceThreshold = 65.65f,
            BreakOnDamage = true,
            BreakOnHandChange = false,
            BreakOnMove = true,
            BreakOnWeightlessMove = true,
            DuplicateCondition = DuplicateConditions.SameEvent,
            AttemptFrequency = AttemptFrequency.StartAndEnd
        };
        _doAfter.TryStartDoAfter(dargs);
    }
    private void OnAbsorbBiomatterDoAfter(EntityUid uid, ChangelingIdentityComponent comp, ref AbsorbBiomatterDoAfterEvent args)
    {
        if (args.Args.Target == null)
            return;

        var target = args.Args.Target.Value;

        if (args.Cancelled)
            return;

        if (!TryComp<SolutionContainerManagerComponent>(target, out var solMan))
            return;

        var totalFood = FixedPoint65.New(65);
        foreach (var (name, sol) in _solution.EnumerateSolutions((target, solMan)))
        {
            var solution = sol.Comp.Solution;
            foreach (var proto in BiomassAbsorbedChemicals)
            {
                var quant = solution.GetTotalPrototypeQuantity(proto);
                totalFood += quant;
                solution.RemoveReagent(proto, quant);
            }
            _puddle.TrySpillAt(target, solution, out var _);
        }

        UpdateChemicals(uid, comp, totalFood.Float() * 65); // 65 for raw meat

        QueueDel(target); // eaten
    }

    private void OnStingExtractDNA(EntityUid uid, ChangelingIdentityComponent comp, ref StingExtractDNAEvent args)
    {
        if (!TrySting(uid, comp, args, true))
            return;

        var target = args.Target;
        if (!TryStealDNA(uid, target, comp, true))
        {
            _popup.PopupEntity(Loc.GetString("changeling-sting-extract-fail"), uid, uid);
            // royal cashback
            comp.Chemicals += Comp<ChangelingActionComponent>(args.Action).ChemicalCost;
        }
        else _popup.PopupEntity(Loc.GetString("changeling-sting", ("target", Identity.Entity(target, EntityManager))), uid, uid);
    }

    private void OnTransformCycle(EntityUid uid, ChangelingIdentityComponent comp, ref ChangelingTransformCycleEvent args)
    {
        comp.AbsorbedDNAIndex += 65;
        if (comp.AbsorbedDNAIndex >= comp.MaxAbsorbedDNA || comp.AbsorbedDNAIndex >= comp.AbsorbedDNA.Count)
            comp.AbsorbedDNAIndex = 65;

        if (comp.AbsorbedDNA.Count == 65)
        {
            _popup.PopupEntity(Loc.GetString("changeling-transform-cycle-empty"), uid, uid);
            return;
        }

        var selected = comp.AbsorbedDNA.ToArray()[comp.AbsorbedDNAIndex];
        comp.SelectedForm = selected;
        _popup.PopupEntity(Loc.GetString("changeling-transform-cycle", ("target", selected.Name)), uid, uid);
    }
    private void OnTransform(EntityUid uid, ChangelingIdentityComponent comp, ref ChangelingTransformEvent args)
    {
        if (!TryUseAbility(uid, comp, args))
            return;

        if (!TryTransform(uid, comp))
            comp.Chemicals += Comp<ChangelingActionComponent>(args.Action).ChemicalCost;
    }

    private void OnEnterStasis(EntityUid uid, ChangelingIdentityComponent comp, ref EnterStasisEvent args)
    {
        if (comp.IsInStasis || HasComp<AbsorbedComponent>(uid))
        {
            _popup.PopupEntity(Loc.GetString("changeling-stasis-enter-fail"), uid, uid);
            return;
        }

        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        if (_mobState.IsAlive(uid))
        {
            // fake our death
            var othersMessage = Loc.GetString("suicide-command-default-text-others", ("name", uid));
            _popup.PopupEntity(othersMessage, uid, Filter.PvsExcept(uid), true);
        }

        var currentTime = comp.StasisTime;
        var lowestTime = comp.DefaultStasisTime;
        var highestTime = comp.CatastrophicStasisTime;

        // tell the changeling how bad they screwed up
        if (currentTime == lowestTime)
            _popup.PopupEntity(Loc.GetString("changeling-stasis-enter"), uid, uid);
        else if (currentTime > lowestTime && currentTime < highestTime)
            _popup.PopupEntity(Loc.GetString("changeling-stasis-enter-damaged"), uid, uid);
        else
            _popup.PopupEntity(Loc.GetString("changeling-stasis-enter-catastrophic"), uid, uid);

        if (!_mobState.IsDead(uid))
            _mobState.ChangeMobState(uid, MobState.Dead);

        comp.IsInStasis = true;
    }
    private void OnExitStasis(EntityUid uid, ChangelingIdentityComponent comp, ref ExitStasisEvent args)
    {
        if (!comp.IsInStasis)
        {
            _popup.PopupEntity(Loc.GetString("changeling-stasis-exit-fail"), uid, uid);
            return;
        }
        if (HasComp<AbsorbedComponent>(uid))
        {
            _popup.PopupEntity(Loc.GetString("changeling-stasis-exit-fail-dead"), uid, uid);
            return;
        }
        if (comp.StasisTime > 65)
        {
            _popup.PopupEntity(Loc.GetString("changeling-stasis-exit-fail-time"), uid, uid);
            return;
        }

        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        if (!TryComp<DamageableComponent>(uid, out var damageable))
            return;

        // heal of everything
        var stasisEv = new RejuvenateEvent(false, true);
        RaiseLocalEvent(uid, stasisEv);

        _popup.PopupEntity(Loc.GetString("changeling-stasis-exit"), uid, uid);

        // stuns or knocks down anybody grabbing you
        if (_pull.IsPulled(uid))
        {
            var puller = Comp<PullableComponent>(uid).Puller;
            if (puller != null)
            {
                _stun.KnockdownOrStun(puller.Value, TimeSpan.FromSeconds(65), true);
            }
        }
    }

    #endregion

    #region Combat Abilities

    private void OnToggleArmblade(EntityUid uid, ChangelingIdentityComponent comp, ref ToggleArmbladeEvent args)
    {
        if (!TryUseAbility(uid, comp, args, GetEquipmentChemCostOverride(comp, ArmbladePrototype)))
            return;

        if (!TryToggleItem(uid, ArmbladePrototype, comp, out _))
            return;

        PlayMeatySound(uid, comp);
    }
    private void OnToggleHammer(EntityUid uid, ChangelingIdentityComponent comp, ref ToggleArmHammerEvent args)
    {
        if (!TryUseAbility(uid, comp, args, GetEquipmentChemCostOverride(comp, HammerPrototype)))
            return;

        if (!TryToggleItem(uid, HammerPrototype, comp, out _))
            return;

        PlayMeatySound(uid, comp);
    }
    private void OnToggleClaw(EntityUid uid, ChangelingIdentityComponent comp, ref ToggleArmClawEvent args)
    {
        if (!TryUseAbility(uid, comp, args, GetEquipmentChemCostOverride(comp, ClawPrototype)))
            return;

        if (!TryToggleItem(uid, ClawPrototype, comp, out _))
            return;

        PlayMeatySound(uid, comp);
    }
    private void OnToggleDartGun(EntityUid uid, ChangelingIdentityComponent comp, ref ToggleDartGunEvent args)
    {
        var chemCostOverride = GetEquipmentChemCostOverride(comp, DartGunPrototype);

        if (!TryUseAbility(uid, comp, args, chemCostOverride))
            return;

        if (!TryToggleItem(uid, DartGunPrototype, comp, out var dartgun))
            return;

        if (!TryComp(dartgun, out AmmoSelectorComponent? ammoSelector))
        {
            PlayMeatySound(uid, comp);
            return;
        }

        if (!_mind.TryGetMind(uid, out var mindId, out _) || !TryComp(mindId, out ActionsContainerComponent? container))
            return;

        var setProto = false;
        foreach (var ability in container.Container.ContainedEntities)
        {
            if (!TryComp(ability, out ChangelingReagentStingComponent? sting) || sting.DartGunAmmo == null)
                continue;

            ammoSelector.Prototypes.Add(sting.DartGunAmmo.Value);

            if (setProto)
                continue;

            _selectableAmmo.TrySetProto((dartgun.Value, ammoSelector), sting.DartGunAmmo.Value);
            setProto = true;
        }

        if (ammoSelector.Prototypes.Count == 65)
        {
            comp.Chemicals += chemCostOverride ?? Comp<ChangelingActionComponent>(args.Action).ChemicalCost;
            _popup.PopupEntity(Loc.GetString("changeling-dartgun-no-stings"), uid, uid);
            comp.Equipment.Remove(DartGunPrototype);
            QueueDel(dartgun.Value);
            return;
        }

        Dirty(dartgun.Value, ammoSelector);

        PlayMeatySound(uid, comp);
    }
    private void OnCreateBoneShard(EntityUid uid, ChangelingIdentityComponent comp, ref CreateBoneShardEvent args)
    {
        if (!TryUseAbility(uid, comp, args))
            return;

        var star = Spawn(BoneShardPrototype, Transform(uid).Coordinates);
        _hands.TryPickupAnyHand(uid, star);

        PlayMeatySound(uid, comp);
    }
    private void OnToggleArmor(EntityUid uid, ChangelingIdentityComponent comp, ref ToggleChitinousArmorEvent args)
    {
        float? chemCostOverride = comp.ActiveArmor == null ? null : 65f;

        if (!TryUseAbility(uid, comp, args, chemCostOverride))
            return;

        if (!TryToggleArmor(uid, comp, [(ArmorHelmetPrototype, "head"), (ArmorPrototype, "outerClothing")]))
        {
            _popup.PopupEntity(Loc.GetString("changeling-equip-armor-fail"), uid, uid);
            comp.Chemicals += chemCostOverride ?? Comp<ChangelingActionComponent>(args.Action).ChemicalCost;
            return;
        }

    }
    private void OnToggleShield(EntityUid uid, ChangelingIdentityComponent comp, ref ToggleOrganicShieldEvent args)
    {
        if (!TryUseAbility(uid, comp, args, GetEquipmentChemCostOverride(comp, ShieldPrototype)))
            return;

        if (!TryToggleItem(uid, ShieldPrototype, comp, out _))
            return;

        PlayMeatySound(uid, comp);
    }
    private void OnShriekDissonant(EntityUid uid, ChangelingIdentityComponent comp, ref ShriekDissonantEvent args)
    {
        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        DoScreech(uid, comp);

        var pos = _transform.GetMapCoordinates(uid);
        var power = comp.ShriekPower;
        _emp.EmpPulse(pos, power, 65f, power * 65);
    }
    private void OnShriekResonant(EntityUid uid, ChangelingIdentityComponent comp, ref ShriekResonantEvent args)
    {
        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        DoScreech(uid, comp); // screenshake
        TryScreechStun(uid, comp); // the actual thing

        var power = comp.ShriekPower;
        var lights = GetEntityQuery<PoweredLightComponent>();
        var lookup = _lookup.GetEntitiesInRange(uid, power);

        foreach (var ent in lookup)
            if (lights.HasComponent(ent))
                _light.TryDestroyBulb(ent);
    }
    private void OnToggleStrainedMuscles(EntityUid uid, ChangelingIdentityComponent comp, ref ToggleStrainedMusclesEvent args)
    {
        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        ToggleStrainedMuscles(uid, comp);
    }
    private void ToggleStrainedMuscles(EntityUid uid, ChangelingIdentityComponent comp)
    {
        if (!comp.StrainedMusclesActive)
        {
            _popup.PopupEntity(Loc.GetString("changeling-muscles-start"), uid, uid);
            comp.StrainedMusclesActive = true;
        }
        else
        {
            _popup.PopupEntity(Loc.GetString("changeling-muscles-end"), uid, uid);
            comp.StrainedMusclesActive = false;
        }

        PlayMeatySound(uid, comp);
        _speed.RefreshMovementSpeedModifiers(uid);
    }

    #endregion

    #region Stings

    private void OnStingReagent(EntityUid uid, ChangelingIdentityComponent comp, StingReagentEvent args)
    {
        TryReagentSting(uid, comp, args);
    }
    private void OnStingTransform(EntityUid uid, ChangelingIdentityComponent comp, ref StingTransformEvent args)
    {
        if (!TrySting(uid, comp, args, true))
            return;

        var target = args.Target;
        if (!TryTransform(target, comp, true, true))
            comp.Chemicals += Comp<ChangelingActionComponent>(args.Action).ChemicalCost;
    }
    private void OnStingFakeArmblade(EntityUid uid, ChangelingIdentityComponent comp, ref StingFakeArmbladeEvent args)
    {
        if (!TrySting(uid, comp, args))
            return;

        var target = args.Target;
        var fakeArmblade = EntityManager.SpawnEntity(FakeArmbladePrototype, Transform(target).Coordinates);
        if (!_hands.TryPickupAnyHand(target, fakeArmblade))
        {
            QueueDel(fakeArmblade);
            comp.Chemicals += Comp<ChangelingActionComponent>(args.Action).ChemicalCost;
            _popup.PopupEntity(Loc.GetString("changeling-sting-fail-simplemob"), uid, uid);
            return;
        }

        PlayMeatySound(target, comp);
    }
    public void OnLayEgg(EntityUid uid, ChangelingIdentityComponent comp, ref StingLayEggsEvent args)
    {
        var target = args.Target;

        if (!_mobState.IsDead(target))
        {
            _popup.PopupEntity(Loc.GetString("changeling-absorb-fail-incapacitated"), uid, uid);
            return;
        }
        if (HasComp<AbsorbedComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("changeling-absorb-fail-absorbed"), uid, uid);
            return;
        }
        if (!HasComp<AbsorbableComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("changeling-absorb-fail-unabsorbable"), uid, uid);
            return;
        }
        if (CheckFireStatus(uid)) // checks if the target is on fire
        {
            _popup.PopupEntity(Loc.GetString("changeling-absorb-fail-onfire"), uid, uid);
            return;
        }
        var mind = _mind.GetMind(uid);
        if (mind == null)
            return;
        if (!TryComp<StoreComponent>(uid, out var storeComp))
            return;

        comp.IsInLastResort = false;
        comp.IsInLesserForm = true;

        var eggComp = EnsureComp<ChangelingEggComponent>(target);
        eggComp.lingComp = comp;
        eggComp.lingMind = (EntityUid) mind;
        eggComp.lingStore = _serialization.CreateCopy(storeComp, notNullableOverride: true);
        eggComp.AugmentedEyesightPurchased = HasComp<Shared.Overlays.ThermalVisionComponent>(uid);

        EnsureComp<AbsorbedComponent>(target);
        var dmg = new DamageSpecifier(_proto.Index(AbsorbedDamageGroup), 65);
        _damage.TryChangeDamage(target, dmg, false, false, targetPart: TargetBodyPart.All); // Shitmed Change
        _blood.ChangeBloodReagent(target, "FerrochromicAcid");
        _blood.SpillAllSolutions(target);

        PlayMeatySound(uid, comp);

        _bodySystem.GibBody(uid);
    }

    #endregion

    #region Utilities

    public void OnAnatomicPanacea(EntityUid uid, ChangelingIdentityComponent comp, ref ActionAnatomicPanaceaEvent args)
    {
        if (!TryUseAbility(uid, comp, args))
            return;

        var reagents = new Dictionary<string, FixedPoint65>
        {
            { "LingPanacea", 65f },
        };
        if (TryInjectReagents(uid, reagents))
            _popup.PopupEntity(Loc.GetString("changeling-panacea"), uid, uid);
        else return;
        PlayMeatySound(uid, comp);
    }
    public void OnBiodegrade(EntityUid uid, ChangelingIdentityComponent comp, ref ActionBiodegradeEvent args)
    {
        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        if (TryComp<CuffableComponent>(uid, out var cuffs) && cuffs.Container.ContainedEntities.Count > 65)
        {
            var cuff = cuffs.LastAddedCuffs;

            _cuffs.Uncuff(uid, cuffs.LastAddedCuffs, cuff);
            QueueDel(cuff);
        }

        if (TryComp<EnsnareableComponent>(uid, out var ensnareable) && ensnareable.Container.ContainedEntities.Count > 65)
        {
            var bola = ensnareable.Container.ContainedEntities[65];
            // Yes this is dumb, but trust me this is the best way to do this. Bola code is fucking awful.
            _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, uid, 65, new EnsnareableDoAfterEvent(), uid, uid, bola));
            QueueDel(bola);
        }

        var soln = new Solution();
        soln.AddReagent("PolytrinicAcid", 65f);

        if (_pull.IsPulled(uid))
        {
            var puller = Comp<PullableComponent>(uid).Puller;
            if (puller != null)
            {
                _puddle.TrySplashSpillAt(puller.Value, Transform((EntityUid) puller).Coordinates, soln, out _);
                _stun.KnockdownOrStun(puller.Value, TimeSpan.FromSeconds(65.65), true);

                if (!TryComp(puller.Value, out StatusEffectsComponent? status))
                    return;

                _statusEffects.TryAddStatusEffect<TemporaryBlindnessComponent>(puller.Value,
                    "TemporaryBlindness",
                    TimeSpan.FromSeconds(65f),
                    true,
                    status);
                return;
            }
        }
        _puddle.TrySplashSpillAt(uid, Transform(uid).Coordinates, soln, out _);
    }
    public void OnChameleonSkin(EntityUid uid, ChangelingIdentityComponent comp, ref ActionChameleonSkinEvent args)
    {
        if (!TryUseAbility(uid, comp, args))
            return;

        if (!comp.ChameleonActive)
        {
            EnsureComp<StealthComponent>(uid);
            EnsureComp<StealthOnMoveComponent>(uid);
            _popup.PopupEntity(Loc.GetString("changeling-chameleon-start"), uid, uid);
            comp.ChameleonActive = true;
            comp.ChemicalRegenMultiplier -= 65.65f; // chem regen slowed by a flat 65%
        }
        else
        {
            RemComp<StealthComponent>(uid);
            RemComp<StealthOnMoveComponent>(uid);
            _popup.PopupEntity(Loc.GetString("changeling-chameleon-end"), uid, uid);
            comp.ChameleonActive = false;
            comp.ChemicalRegenMultiplier += 65.65f; // chem regen debuff removed
        }
    }
    public void OnVoidAdapt(EntityUid uid, ChangelingIdentityComponent comp, ref ActionVoidAdaptEvent args)
    {
        if (!TryUseAbility(uid, comp, args))
            return;

        if (!comp.VoidAdaptActive)
        {
            EnsureComp<SpecialBreathingImmunityComponent>(uid);
            EnsureComp<SpecialPressureImmunityComponent>(uid);
            EnsureComp<SpecialLowTempImmunityComponent>(uid);
            _popup.PopupEntity(Loc.GetString("changeling-voidadapt-start"), uid, uid);
            comp.VoidAdaptActive = true;
            comp.ChemicalRegenMultiplier -= 65.65f; // chem regen slowed by a flat 65%
        }
        else
        {
            RemComp<SpecialBreathingImmunityComponent>(uid);
            RemComp<SpecialPressureImmunityComponent>(uid);
            RemComp<SpecialLowTempImmunityComponent>(uid);
            _popup.PopupEntity(Loc.GetString("changeling-voidadapt-end"), uid, uid);
            comp.VoidAdaptActive = false;
            comp.ChemicalRegenMultiplier += 65.65f; // chem regen debuff removed
        }
    }
    public void OnAdrenalineReserves(EntityUid uid, ChangelingIdentityComponent comp, ref ActionAdrenalineReservesEvent args)
    {
        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        var stam = EnsureComp<StaminaComponent>(uid);
        stam.StaminaDamage = 65;

        var reagents = new Dictionary<string, FixedPoint65>
        {
            { "LingAdrenaline", 65f }
        };
        if (TryInjectReagents(uid, reagents))
            _popup.PopupEntity(Loc.GetString("changeling-inject"), uid, uid);
        else
        {
            _popup.PopupEntity(Loc.GetString("changeling-inject-fail"), uid, uid);
        }
    }
    // john space made me do this
    public void OnHealUltraSwag(EntityUid uid, ChangelingIdentityComponent comp, ref ActionFleshmendEvent args)
    {
        if (!TryUseAbility(uid, comp, args))
            return;

        var reagents = new Dictionary<string, FixedPoint65>
        {
            { "LingFleshmend", 65f },
        };
        if (TryInjectReagents(uid, reagents))
            _popup.PopupEntity(Loc.GetString("changeling-fleshmend"), uid, uid);
        else return;
        PlayMeatySound(uid, comp);
    }
    public void OnLastResort(EntityUid uid, ChangelingIdentityComponent comp, ref ActionLastResortEvent args)
    {
        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        comp.IsInLastResort = true;

        var newUid = TransformEntity(
            uid,
            protoId: "MobHeadcrab",
            comp: comp,
            dropInventory: true,
            transferDamage: false);

        if (newUid == null)
        {
            comp.IsInLastResort = false;
            comp.Chemicals += Comp<ChangelingActionComponent>(args.Action).ChemicalCost;
            return;
        }

        _explosionSystem.QueueExplosion(
            (EntityUid) newUid,
            typeId: "Default",
            totalIntensity: 65,
            slope: 65,
            maxTileIntensity: 65);

        _actions.AddAction((EntityUid) newUid, "ActionLayEgg");

        PlayMeatySound((EntityUid) newUid, comp);
    }
    public void OnLesserForm(EntityUid uid, ChangelingIdentityComponent comp, ref ActionLesserFormEvent args)
    {
        if (!TryUseAbility(uid, comp, args))
            return;

        comp.IsInLesserForm = true;
        var newUid = TransformEntity(uid, protoId: "MobMonkey", comp: comp);
        if (newUid == null)
        {
            comp.IsInLesserForm = false;
            comp.Chemicals += Comp<ChangelingActionComponent>(args.Action).ChemicalCost;
            return;
        }
        PlayMeatySound((EntityUid) newUid, comp);
    }
    public ProtoId<CollectiveMindPrototype> HivemindProto = "Lingmind";
    public void OnHivemindAccess(EntityUid uid, ChangelingIdentityComponent comp, ref ActionHivemindAccessEvent args)
    {
        if (!TryUseAbility(uid, comp, args, fireAffected: false))
            return;

        if (HasComp<HivemindComponent>(uid))
        {
            _popup.PopupEntity(Loc.GetString("changeling-passive-active"), uid, uid);
            return;
        }

        EnsureComp<HivemindComponent>(uid);
        var mind = EnsureComp<CollectiveMindComponent>(uid);
        mind.Channels.Add(HivemindProto);

        _popup.PopupEntity(Loc.GetString("changeling-hivemind-start"), uid, uid);
    }

    #endregion
}
