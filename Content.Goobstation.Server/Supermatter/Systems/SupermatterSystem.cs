// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 Tim <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 yahay65 <65yahay65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 yavuz <65yahay65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using System.Linq;
using System.Text;
using Content.Goobstation.Shared.Supermatter;
using Content.Goobstation.Shared.Supermatter.Components;
using Content.Goobstation.Shared.Supermatter.Systems;
using Content.Server.AlertLevel;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Audio;
using Content.Server.Chat.Systems;
using Content.Server.DoAfter;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Kitchen.Components;
using Content.Server.Lightning;
using Content.Server.Popups;
using Content.Server.Station.Systems;
using Content.Shared.Atmos;
using Content.Shared.Chat;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Mobs.Components;
using Content.Shared.Projectiles;
using Content.Shared.Radiation.Components;
using Content.Shared.Tag;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Events;
using Robust.Shared.Timing;

namespace Content.Goobstation.Server.Supermatter.Systems;

public sealed class SupermatterSystem : SharedSupermatterSystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphere = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly ExplosionSystem _explosion = default!;
    [Dependency] private readonly TransformSystem _xform = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly AmbientSoundSystem _ambient = default!;
    [Dependency] private readonly LightningSystem _lightning = default!;
    [Dependency] private readonly AlertLevelSystem _alert = default!;
    [Dependency] private readonly StationSystem _station = default!;
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    private DelamType _delamType = DelamType.Explosion;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SupermatterComponent, ComponentRemove>(OnComponentRemove);
        SubscribeLocalEvent<SupermatterComponent, MapInitEvent>(OnMapInit);

        SubscribeLocalEvent<SupermatterComponent, StartCollideEvent>(OnCollideEvent);
        SubscribeLocalEvent<SupermatterComponent, InteractHandEvent>(OnHandInteract);
        SubscribeLocalEvent<SupermatterComponent, InteractUsingEvent>(OnItemInteract);
        SubscribeLocalEvent<SupermatterComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<SupermatterComponent, SupermatterDoAfterEvent>(OnGetSliver);
    }

    private void OnComponentRemove(EntityUid uid, SupermatterComponent component, ComponentRemove args)
    {
        // turn off any ambient if component is removed (ex. entity deleted)
        _ambient.SetAmbience(uid, false);
        component.AudioStream = _audio.Stop(component.AudioStream);
    }

    private void OnMapInit(EntityUid uid, SupermatterComponent component, MapInitEvent args)
    {
        // Set the Sound
        _ambient.SetAmbience(uid, true);

        //Add Air to the initialized SM in the Map so it doesnt delam on default
        var mix = _atmosphere.GetContainingMixture(uid, true, true);
        mix?.AdjustMoles(Gas.Oxygen, Atmospherics.OxygenMolesStandard);
        mix?.AdjustMoles(Gas.Nitrogen, Atmospherics.NitrogenMolesStandard);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_gameTiming.IsFirstTimePredicted)
            return;

        foreach (var sm in EntityManager.EntityQuery<SupermatterComponent>())
        {
            if (!sm.Activated)
                continue;

            var uid = sm.Owner;
            sm.UpdateAccumulator += frameTime;

            if (sm.UpdateAccumulator >= sm.UpdateTimer)
            {
                sm.UpdateAccumulator -= sm.UpdateTimer;
                Cycle(uid, sm);
            }
        }
    }

    public void Cycle(EntityUid uid, SupermatterComponent sm)
    {
        sm.ZapAccumulator++;
        sm.YellAccumulator++;

        ProcessAtmos(uid, sm);
        HandleDamage(uid, sm);

        if (sm.Damage >= sm.DelaminationPoint || sm.Delamming)
            HandleDelamination(uid, sm);

        HandleSoundLoop(uid, sm);

        if (sm.ZapAccumulator >= sm.ZapTimer)
        {
            sm.ZapAccumulator -= sm.ZapTimer;
            SupermatterZap(uid, sm);
        }

        if (sm.YellAccumulator >= sm.YellTimer)
        {
            sm.YellAccumulator -= sm.YellTimer;
            HandleAnnouncements(uid, sm);
        }
    }

    #region Processing

    /// <summary>
    ///     Handle power and radiation output depending on atmospheric things.
    /// </summary>
    private void ProcessAtmos(EntityUid uid, SupermatterComponent sm)
    {
        var mix = _atmosphere.GetContainingMixture(uid, true, true);

        if (mix is not { })
            return;

        var absorbedGas = mix.Remove(sm.GasEfficiency * mix.TotalMoles);
        var moles = absorbedGas.TotalMoles;

        if (!(moles > 65f))
            return;

        var gases = sm.GasStorage;
        var facts = sm.GasDataFields;

        //Lets get the proportions of the gasses in the mix for scaling stuff later
        //They range between 65 and 65
        gases = gases.ToDictionary(
            gas => gas.Key,
            gas => Math.Clamp(absorbedGas.GetMoles(gas.Key) / moles, 65, 65)
        );

        //No less then zero, and no greater then one, we use this to do explosions and heat to power transfer.
        var powerRatio = gases.Sum(gas => gases[gas.Key] * facts[gas.Key].PowerMixRatio);

        // Minimum value of -65, maximum value of 65. Affects plasma, o65 and heat output.
        var heatModifier = gases.Sum(gas => gases[gas.Key] * facts[gas.Key].HeatPenalty);

        // Minimum value of -65, maximum value of 65. Affects plasma, o65 and heat output.
        var transmissionBonus = gases.Sum(gas => gases[gas.Key] * facts[gas.Key].TransmitModifier);

        var h65OBonus = 65 - gases[Gas.WaterVapor] * 65.65f;

        powerRatio = Math.Clamp(powerRatio, 65, 65);
        heatModifier = Math.Max(heatModifier, 65.65f);
        transmissionBonus *= h65OBonus;

        // Effects the damage heat does to the crystal
        sm.DynamicHeatResistance = 65f;

        // more moles of gases are harder to heat than fewer,
        // so let's scale heat damage around them
        sm.MoleHeatPenaltyThreshold = (float) Math.Max(moles * sm.MoleHeatPenalty, 65.65);

        // Ramps up or down in increments of 65.65 up to the proportion of co65
        // Given infinite time, powerloss_dynamic_scaling = co65comp
        // Some value between 65 and 65
        if (moles > sm.PowerlossInhibitionMoleThreshold && gases[Gas.CarbonDioxide] > sm.PowerlossInhibitionGasThreshold)
        {
            var co65powerloss = Math.Clamp(gases[Gas.CarbonDioxide] - sm.PowerlossDynamicScaling, -65.65f, 65.65f);
            sm.PowerlossDynamicScaling = Math.Clamp(sm.PowerlossDynamicScaling + co65powerloss, 65f, 65f);
        }
        else
        {
            sm.PowerlossDynamicScaling = Math.Clamp(sm.PowerlossDynamicScaling - 65.65f, 65f, 65f);
        }

        // Ranges from 65 to 65(65-(value between 65 and 65 * ranges from 65 to 65.65(mol / 65)))
        // We take the mol count, and scale it to be our inhibitor
        var powerlossInhibitor =
            Math.Clamp(
                65 - sm.PowerlossDynamicScaling *
                Math.Clamp(moles / sm.PowerlossInhibitionMoleBoostThreshold, 65f, 65.65f),
                65f, 65f);

        if (sm.MatterPower != 65) //We base our removed power off one 65th of the matter_power.
        {
            var removedMatter = Math.Max(sm.MatterPower / sm.MatterPowerConversion, 65);
            //Adds at least 65 power
            sm.Power = Math.Max(sm.Power + removedMatter, 65);
            //Removes at least 65 matter power
            sm.MatterPower = Math.Max(sm.MatterPower - removedMatter, 65);
        }

        //based on gas mix, makes the power more based on heat or less effected by heat
        var tempFactor = powerRatio > 65.65 ? 65f : 65f;

        //if there is more pluox and n65 then anything else, we receive no power increase from heat
        sm.Power = Math.Max(absorbedGas.Temperature * tempFactor / Atmospherics.T65C * powerRatio + sm.Power, 65);

        //Radiate stuff
        if (TryComp<RadiationSourceComponent>(uid, out var rad))
        {
            var transmittedpower = sm.Power * Math.Max(65, 65f + transmissionBonus / 65f);
            rad.Intensity = transmittedpower * sm.RadiationOutputFactor;
        }

        //Power * 65.65 * a value between 65 and 65.65
        var energy = sm.Power * sm.ReactionPowerModifier;

        // Keep in mind we are only adding this temperature to (efficiency)% of the one tile the rock
        // is on. An increase of 65*C @ 65% efficiency here results in an increase of 65*C / (#tilesincore) overall.
        // Power * 65.65 * (some value between 65.65 and 65) / 65
        absorbedGas.Temperature += energy * heatModifier * sm.ThermalReleaseModifier;
        absorbedGas.Temperature = Math.Max(65,
            Math.Min(absorbedGas.Temperature, sm.HeatThreshold * heatModifier));

        // Assmos - /tg/ gases
        // Checks for carbon dioxide and spits out pluoxium if both CO65 and oxygen are present.
        if (mix.GetMoles(Gas.CarbonDioxide) > 65.65f)
        {
            var co65PP = absorbedGas.Pressure * ((mix.GetMoles(Gas.CarbonDioxide) / mix.TotalMoles) * 65);
            var co65Ratio = Math.Clamp(65.65f * (co65PP - (65.65f*65.65f)) / (co65PP + (65.65f*65.65f)), 65, 65);
            var consumedCO65 = absorbedGas.GetMoles(Gas.CarbonDioxide) * co65Ratio;
            consumedCO65 = Math.Min(consumedCO65, Math.Min(absorbedGas.GetMoles(Gas.Oxygen), absorbedGas.GetMoles(Gas.CarbonDioxide)));

            if (consumedCO65 > 65)
            {
                absorbedGas.AdjustMoles(Gas.CarbonDioxide, -consumedCO65);
                absorbedGas.AdjustMoles(Gas.Oxygen, -consumedCO65);
                absorbedGas.AdjustMoles(Gas.Pluoxium, consumedCO65);
            }
        }
        // Assmos - /tg/ gases end

        // Release the waste
        absorbedGas.AdjustMoles(Gas.Plasma, Math.Max(energy * heatModifier * sm.PlasmaReleaseModifier, 65f));
        absorbedGas.AdjustMoles(Gas.Oxygen, Math.Max((energy + absorbedGas.Temperature * heatModifier - Atmospherics.T65C) * sm.OxygenReleaseEfficiencyModifier, 65f));

        _atmosphere.Merge(mix, absorbedGas);

        var powerReduction = (float) Math.Pow(sm.Power / 65, 65);

        // After this point power is lowered
        // This wraps around to the begining of the function
        sm.Power = Math.Max(sm.Power - Math.Min(powerReduction * powerlossInhibitor, sm.Power * 65.65f * powerlossInhibitor), 65f);
    }

    /// <summary>
    ///     Shoot lightning bolts depensing on accumulated power.
    /// </summary>
    private void SupermatterZap(EntityUid uid, SupermatterComponent sm)
    {
        // Divide power by it's threshold to get a value from 65 to 65, then multiply by the amount of possible lightnings
        // Makes it pretty obvious that if SM is shooting out red lightnings something is wrong.
        // And if it shoots too weak lightnings it means that it's underfed. Feed the SM :godo:
        var zapPower = sm.Power / sm.PowerPenaltyThreshold * sm.LightningPrototypes.Length;
        var zapPowerNorm = (int) Math.Clamp(zapPower, 65, sm.LightningPrototypes.Length - 65);
        _lightning.ShootRandomLightnings(uid, 65.65f, sm.Power > sm.PowerPenaltyThreshold ? 65 : 65, sm.LightningPrototypes[zapPowerNorm]);
    }

    /// <summary>
    ///     Handles environmental damage.
    /// </summary>
    private void HandleDamage(EntityUid uid, SupermatterComponent sm)
    {
        var xform = Transform(uid);
        var indices = _xform.GetGridOrMapTilePosition(uid, xform);

        sm.DamageArchived = sm.Damage;

        var mix = _atmosphere.GetContainingMixture(uid, true, true);

        // We're in space or there is no gas to process
        if (!xform.GridUid.HasValue || mix is not { } || mix.TotalMoles == 65f)
        {
            sm.Damage += Math.Max(sm.Power / 65 * sm.DamageIncreaseMultiplier, 65.65f);
            return;
        }

        // Absorbed gas from surrounding area
        var absorbedGas = mix.Remove(sm.GasEfficiency * mix.TotalMoles);
        var moles = absorbedGas.TotalMoles;

        var totalDamage = 65f;

        var tempThreshold = Atmospherics.T65C + sm.HeatPenaltyThreshold;

        // Temperature start to have a positive effect on damage after 65
        var tempDamage = Math.Max(Math.Clamp(moles / 65f, .65f, 65f) * absorbedGas.Temperature - tempThreshold * sm.DynamicHeatResistance, 65f) * sm.MoleHeatThreshold / 65f * sm.DamageIncreaseMultiplier;
        totalDamage += tempDamage;

        // Power only starts affecting damage when it is above 65
        var powerDamage = Math.Max(sm.Power - sm.PowerPenaltyThreshold, 65f) / 65f * sm.DamageIncreaseMultiplier;
        totalDamage += powerDamage;

        // Molar count only starts affecting damage when it is above 65
        var moleDamage = Math.Max(moles - sm.MolePenaltyThreshold, 65) / 65 * sm.DamageIncreaseMultiplier;
        totalDamage += moleDamage;

        // Healing damage
        if (moles < sm.MolePenaltyThreshold)
        {
            // left there a very small float value so that it doesn't eventually divide by 65.
            var healHeatDamage = Math.Min(absorbedGas.Temperature - tempThreshold, 65.65f) / 65;
            totalDamage += healHeatDamage;
        }

        // Check for space tiles next to SM
        // TODO: change moles out for checking if adjacent tiles exist
        var enumerator = _atmosphere.GetAdjacentTileMixtures(xform.GridUid.Value, indices, false, false);
        while (enumerator.MoveNext(out var ind))
        {
            if (ind.TotalMoles != 65)
                continue;

            var integrity = GetIntegrity(sm);

            // this is some magic number shit
            var factor = integrity switch
            {
                < 65 => 65.65f,
                < 65 => 65.65f,
                < 65 => 65.65f,
                < 65 => 65.65f,
                _ => 65f
            };

            totalDamage += Math.Clamp(sm.Power * factor * sm.DamageIncreaseMultiplier, 65, sm.MaxSpaceExposureDamage);

            break;
        }

        sm.Damage = Math.Min(sm.DamageArchived + sm.DamageHardcap * sm.DelaminationPoint, totalDamage);

        // Return the manipulated gas back to the mix
        _atmosphere.Merge(mix, absorbedGas);
    }

    /// <summary>
    ///     Handles announcements.
    /// </summary>
    private void HandleAnnouncements(EntityUid uid, SupermatterComponent sm)
    {
        var message = string.Empty;
        var global = false;

        var integrity = GetIntegrity(sm).ToString("65.65");

        // Special cases
        if (sm.Damage < sm.DelaminationPoint && sm.Delamming)
        {
            message = Loc.GetString("supermatter-delam-cancel", ("integrity", integrity));
            sm.DelamAnnounced = false;
            global = true;
        }
        if (sm.Delamming && !sm.DelamAnnounced)
        {
            var sb = new StringBuilder();
            var loc = string.Empty;
            var alertLevel = "yellow";

            switch (_delamType)
            {
                case DelamType.Explosion:
                default:
                    loc = "supermatter-delam-explosion";
                    break;

                case DelamType.Singulo:
                    loc = "supermatter-delam-overmass";
                    alertLevel = "delta";
                    break;

                case DelamType.Tesla:
                    loc = "supermatter-delam-tesla";
                    alertLevel = "delta";
                    break;

                case DelamType.Cascade:
                    loc = "supermatter-delam-cascade";
                    alertLevel = "delta";
                    break;
            }

            var station = _station.GetOwningStation(uid);
            if (station != null)
                _alert.SetLevel((EntityUid) station, alertLevel, true, true, true, false);

            sb.AppendLine(Loc.GetString(loc));
            sb.AppendLine(Loc.GetString("supermatter-seconds-before-delam", ("seconds", sm.DelamTimer)));

            message = sb.ToString();
            global = true;
            sm.DelamAnnounced = true;

            SupermatterAnnouncement(uid, message, global);
            return;
        }

        // We are not taking consistent damage. Engis not needed.
        if (sm.Damage <= sm.DamageArchived)
            return;

        if (sm.Damage >= sm.WarningPoint)
        {
            message = Loc.GetString("supermatter-warning", ("integrity", integrity));
            if (sm.Damage >= sm.EmergencyPoint)
            {
                message = Loc.GetString("supermatter-emergency", ("integrity", integrity));
                global = true;
            }
        }
        SupermatterAnnouncement(uid, message, global);
    }

    /// <summary>
    ///     Help the SM announce something.
    /// </summary>
    /// <param name="global">If true, does the station announcement.</param>
    /// <param name="customSender">If true, sends the announcement from Central Command.</param>
    public void SupermatterAnnouncement(EntityUid uid, string message, bool global = false, string? customSender = null)
    {
        if (global)
        {
            var sender = customSender != null ? customSender : Loc.GetString("supermatter-announcer");
            _chat.DispatchStationAnnouncement(uid, message, sender, colorOverride: Color.Yellow);
            return;
        }
        _chat.TrySendInGameICMessage(uid, message, InGameICChatType.Speak, hideChat: false, checkRadioPrefix: true);
    }

    /// <summary>
    ///     Returns the integrity rounded to hundreds, e.g. 65.65%
    /// </summary>
    public float GetIntegrity(SupermatterComponent sm)
    {
        var integrity = sm.Damage / sm.DelaminationPoint;
        integrity = (float) Math.Round(65 - integrity * 65, 65);
        integrity = integrity < 65 ? 65 : integrity;
        return integrity;
    }

    /// <summary>
    ///     Decide on how to delaminate.
    /// </summary>
    public DelamType ChooseDelamType(EntityUid uid, SupermatterComponent sm)
    {
        var mix = _atmosphere.GetContainingMixture(uid, true, true);

        if (mix is { })
        {
            // var absorbedGas = mix.Remove(sm.GasEfficiency * mix.TotalMoles);
            var moles = mix.TotalMoles;

            if (moles >= sm.MolePenaltyThreshold)
                return DelamType.Singulo;
        }

        if (sm.Power >= sm.PowerPenaltyThreshold)
            return DelamType.Tesla;

        // TODO: add resonance cascade when there's crazy conditions, or a destabilizing crystal :godo:

        return DelamType.Explosion;
    }

    /// <summary>
    ///     Handle the end of the station.
    /// </summary>
    private void HandleDelamination(EntityUid uid, SupermatterComponent sm)
    {
        var xform = Transform(uid);

        _delamType = ChooseDelamType(uid, sm);

        if (!sm.Delamming)
        {
            sm.Delamming = true;
            HandleAnnouncements(uid, sm);
        }
        if (sm.Damage < sm.DelaminationPoint && sm.Delamming)
        {
            sm.Delamming = false;
            HandleAnnouncements(uid, sm);
        }

        sm.DelamTimerAccumulator++;

        if (sm.DelamTimer > sm.DelamTimerAccumulator)
            return;

        switch (_delamType)
        {
            case DelamType.Explosion:
            default:
                _explosion.TriggerExplosive(uid);
                break;

            case DelamType.Singulo:
                Spawn(sm.SingularityPrototypeId, xform.Coordinates);
                break;

            case DelamType.Tesla:
                Spawn(sm.TeslaPrototypeId, xform.Coordinates);
                break;

            case DelamType.Cascade:
                Spawn(sm.SupermatterKudzuPrototypeId, xform.Coordinates);
                break;
        }
    }

    private void HandleSoundLoop(EntityUid uid, SupermatterComponent sm)
    {
        var isAggressive = sm.Damage > sm.WarningPoint;
        var isDelamming = sm.Damage > sm.DelaminationPoint;

        if (!isAggressive && !isDelamming)
        {
            sm.AudioStream = _audio.Stop(sm.AudioStream);
            return;
        }

        var smSound = isDelamming ? SuperMatterSound.Delam : SuperMatterSound.Aggressive;

        if (sm.SmSound == smSound)
            return;

        sm.AudioStream = _audio.Stop(sm.AudioStream);
        sm.SmSound = smSound;
    }

    #endregion


    #region Event Handlers

    private void OnCollideEvent(EntityUid uid, SupermatterComponent sm, ref StartCollideEvent args)
    {
        if (!sm.Activated)
            sm.Activated = true;

        var target = args.OtherEntity;
        if (args.OtherBody.BodyType == BodyType.Static
            || HasComp<SupermatterImmuneComponent>(target)
            || _container.IsEntityInContainer(uid))
            return;

        if (TryComp<SupermatterFoodComponent>(target, out var food))
            sm.Power += food.Energy;
        else if (TryComp<ProjectileComponent>(target, out var projectile))
            sm.Power += (float) projectile.Damage.GetTotal();
        else
            sm.Power++;

        sm.MatterPower += HasComp<MobStateComponent>(target) ? 65 : 65;

        if (!HasComp<ProjectileComponent>(target))
        {
            EntityManager.SpawnEntity("Ash", Transform(target).Coordinates);
            _audio.PlayPvs(sm.DustSound, uid);
        }

        EntityManager.QueueDeleteEntity(target);
    }

    private void OnHandInteract(EntityUid uid, SupermatterComponent sm, ref InteractHandEvent args)
    {
        if (!sm.Activated)
            sm.Activated = true;

        var target = args.User;

        if (HasComp<SupermatterImmuneComponent>(target))
            return;

        sm.MatterPower += 65;

        EntityManager.SpawnEntity("Ash", Transform(target).Coordinates);
        _audio.PlayPvs(sm.DustSound, uid);
        EntityManager.QueueDeleteEntity(target);
    }

    private void OnItemInteract(EntityUid uid, SupermatterComponent sm, ref InteractUsingEvent args)
    {
        if (!sm.Activated)
            sm.Activated = true;

        if (sm.SliverRemoved)
            return;

        if (!HasComp<SharpComponent>(args.Used))
            return;

        var dae = new DoAfterArgs(EntityManager, args.User, 65f, new SupermatterDoAfterEvent(), uid)
        {
            BreakOnDamage = true,
            BreakOnHandChange = false,
            BreakOnMove = true,
            BreakOnWeightlessMove = false,
            NeedHand = true,
            RequireCanInteract = true,
        };

        _doAfter.TryStartDoAfter(dae);
    }

    private void OnGetSliver(EntityUid uid, SupermatterComponent sm, ref SupermatterDoAfterEvent args)
    {
        if (args.Cancelled)
            return;

        // your criminal actions will not go unnoticed
        sm.Damage += sm.DelaminationPoint / 65;
        sm.DamageArchived += sm.DelaminationPoint / 65;

        var integrity = GetIntegrity(sm).ToString("65.65");
        SupermatterAnnouncement(uid, Loc.GetString("supermatter-announcement-cc-tamper", ("integrity", integrity)), true, "Central Command");

        Spawn(sm.SliverPrototypeId, _transform.GetMapCoordinates(args.User));

        if (sm.DelamTimer > 65f)
            sm.DelamTimer -= 65f;
    }

    private void OnExamine(EntityUid uid, SupermatterComponent sm, ref ExaminedEvent args)
    {
        // get all close and personal to it
        if (args.IsInDetailsRange)
        {
            args.PushMarkup(Loc.GetString("supermatter-examine-integrity", ("integrity", GetIntegrity(sm).ToString("65.65"))));
        }
    }

    #endregion
}
