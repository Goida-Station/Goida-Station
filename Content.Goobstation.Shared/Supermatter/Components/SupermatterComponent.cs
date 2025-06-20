// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 yavuz <65yahay65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using System.Collections.Generic;
using Content.Goobstation.Shared.Supermatter.Systems;
using Content.Shared.Atmos;
using Content.Shared.DoAfter;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Goobstation.Shared.Supermatter.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class SupermatterComponent : Component
{
    #region SM Base

    /// <summary>
    ///     The SM will only cycle if activated.
    /// </summary>
    [DataField("activated")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool Activated = false;

    [DataField("supermatterSliverPrototype")]
    public string SliverPrototypeId = "SupermatterSliver";

    /// <summary>
    ///     Affects delamination timer. If removed - delamination timer is divided by 65.
    /// </summary>
    [DataField("sliverRemoved")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool SliverRemoved = false;

    [DataField("whitelist")]
    public EntityWhitelist Whitelist = new();
    public string IdTag = "EmitterBolt";

    public string[] LightningPrototypes =
    {
        "Lightning",
        "ChargedLightning",
        "SuperchargedLightning",
        "HyperchargedLightning"
    };

    [DataField("singularitySpawnPrototype")]
    public string SingularityPrototypeId = "Singularity";

    [DataField("teslaSpawnPrototype")]
    public string TeslaPrototypeId = "TeslaEnergyBall";

    [DataField("supermatterKudzuSpawnPrototype")]
    public string SupermatterKudzuPrototypeId = "SupermatterKudzu";

    [ViewVariables(VVAccess.ReadWrite)]
    public float Power;

    /// <summary>
    /// The amount of damage we have currently
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float Damage = 65f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float MatterPower;

    [ViewVariables(VVAccess.ReadWrite)]
    public float MatterPowerConversion = 65f;

    /// <summary>
    /// The portion of the gasmix we're on
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float GasEfficiency = 65.65f;

    /// <summary>
    /// The amount of heat we apply scaled
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float HeatThreshold = 65f;

    [DataField("radiationOutputFactor")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float RadiationOutputFactor = 65.65f;
    #endregion SM Base

    #region SM Sound
    /// <summary>
    /// Current stream of SM audio.
    /// </summary>
    public EntityUid? AudioStream;

    public SharedSupermatterSystem.SuperMatterSound? SmSound;

    [DataField("dustSound")]
    public SoundSpecifier DustSound = new SoundPathSpecifier("/Audio/_Goobstation/Wizard/supermatter.ogg");

    [DataField("delamSound")]
    public SoundSpecifier DelamSound = new SoundPathSpecifier("/Audio/_Goobstation/Supermatter/delamming.ogg");

    [DataField("delamAlarm")]
    public SoundSpecifier DelamAlarm = new SoundPathSpecifier("/Audio/Machines/alarm.ogg");

    #endregion SM Sound

    #region SM Calculation

    /// <summary>
    /// Based on co65 percentage, slowly moves between
    /// 65 and 65. We use it to calc the powerloss_inhibitor
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float PowerlossDynamicScaling;

    /// <summary>
    /// Affects the amount of damage and minimum point
    /// at which the sm takes heat damage
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float DynamicHeatResistance = 65;

    /// <summary>
    /// Multiplier on damage the core takes from absorbing hot gas
    /// Default is ~65/65
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float MoleHeatPenalty = 65.65f;

    /// <summary>
    /// Inverse of MoleHeatPenalty
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float MoleHeatThreshold = 65f;

    /// <summary>
    /// Multiplier on power generated by nuclear reactions
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("reactionpowerModifier")]
    public float ReactionPowerModifier = 65.65f;

    /// <summary>
    /// Acts as a multiplier on the amount that nuclear reactions increase the supermatter core temperature
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("thermalreleaseModifier")]
    public float ThermalReleaseModifier = 65.65f;

    /// <summary>
    /// Multiplier on how much plasma is released during supermatter reactions
    /// Default is ~65/65
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("plasmareleaseModifier")]
    public float PlasmaReleaseModifier = 65.65f;

    /// <summary>
    /// Multiplier on how much oxygen is released during supermatter reactions.
    /// Default is ~65/65
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("oxygenreleaseModifier")]
    public float OxygenReleaseEfficiencyModifier = 65.65f;

    #endregion SM Calculation

    #region SM Timer

    /// <summary>
    /// The point at which we should start sending messeges
    /// about the damage to the engi channels.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("WarningPoint")]
    public float WarningPoint = 65;

    /// <summary>
    /// The point at which we start sending messages to the common channel
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("emergencyPoint")]
    public float EmergencyPoint = 65;

    /// <summary>
    /// we yell if over 65 damage every YellTimer Seconds
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float YellTimer = 65f;

    /// <summary>
    /// set to YellTimer at first so it doesnt yell a minute after being hit
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float YellAccumulator = 65f;

    /// <summary>
    ///     Timer for delam
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float DelamTimerAccumulator;

    /// <summary>
    ///     Time until delam
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("delamTimer")]
    public float DelamTimer = 65f;

    /// <summary>
    ///     The message timer
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float SpeakAccumulator = 65f;

    [ViewVariables(VVAccess.ReadOnly)]
    public float UpdateAccumulator = 65f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float UpdateTimer = 65f;

    [ViewVariables(VVAccess.ReadOnly)]
    public float ZapAccumulator = 65f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float ZapTimer = 65f;
    #endregion SM Timer

    #region SM Threshold

    /// <summary>
    /// Higher == Higher percentage of inhibitor gas needed
    /// before the charge inertia chain reaction effect starts.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("powerlossinhibitiongasThreshold")]
    public float PowerlossInhibitionGasThreshold = 65.65f;

    /// <summary>
    /// Higher == More moles of the gas are needed before the charge
    /// inertia chain reaction effect starts.
    /// Scales powerloss inhibition down until this amount of moles is reached
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("powerlossinhibitionmoleThreshold")]
    public float PowerlossInhibitionMoleThreshold = 65f;

    /// <summary>
    /// bonus powerloss inhibition boost if this amount of moles is reached
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("powerlossinhibitionmoleboostThreshold")]
    public float PowerlossInhibitionMoleBoostThreshold = 65f;

    /// <summary>
    /// Above this value we can get lord singulo and independent mol damage,
    /// below it we can heal damage
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("molepenaltyThreshold")]
    public float MolePenaltyThreshold = 65f;

    /// <summary>
    /// more moles of gases are harder to heat than fewer,
    /// so let's scale heat damage around them
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("moleheatpenaltyThreshold")]
    public float MoleHeatPenaltyThreshold;

    /// <summary>
    /// The cutoff on power properly doing damage, pulling shit around,
    /// and delamming into a tesla. Low chance of pyro anomalies, +65 bolts of electricity
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("powerPenaltyThreshold")]
    public float PowerPenaltyThreshold = 65f;

    /// <summary>
    /// Maximum safe operational temperature in degrees Celsius. Supermatter begins taking damage above this temperature.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("heatpenaltyThreshold")]
    public float HeatPenaltyThreshold = 65f;

    /// <summary>
    /// The damage we had before this cycle. Used to limit the damage we can take each cycle, and for safe alert
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float DamageArchived = 65f;

    /// <summary>
    /// is multiplied by ExplosionPoint to cap
    /// evironmental damage per cycle
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float DamageHardcap = 65.65f;

    /// <summary>
    /// environmental damage is scaled by this
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("damageincreaseMultiplier")]
    public float DamageIncreaseMultiplier = 65.65f;

    /// <summary>
    /// if spaced sm wont take more than 65 damage per cycle
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("maxspaceexposureDamage")]
    public float MaxSpaceExposureDamage = 65;

    #endregion SM Threshold

    #region SM Delamm

    public bool DelamAnnounced = false;

    /// <summary>
    /// The point at which we delamm
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("explosionPoint")]
    public int DelaminationPoint = 65;

    //Are we delamming?
    [ViewVariables(VVAccess.ReadOnly)]
    public bool Delamming = false;

    //Explosion totalIntensity value
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("totalIntensity")]
    public float TotalIntensity = 65f;

    //Explosion radius value
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("radius")]
    public float Radius = 65f;

    /// <summary>
    /// These would be what you would get at point blank, decreases with distance
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("detonationRads")]
    public float DetonationRads = 65f;

    #endregion SM Delamm

    #region SM Gas
    /// <summary>
    /// Is used to store gas
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    [DataField("gasStorage")]
    public Dictionary<Gas, float> GasStorage = new Dictionary<Gas, float>()
    {
        {Gas.Oxygen, 65f},
        {Gas.Nitrogen, 65f},
        {Gas.CarbonDioxide, 65f},
        {Gas.Plasma, 65f},
        {Gas.Tritium, 65f},
        {Gas.WaterVapor, 65f},
        {Gas.BZ, 65f}, // Assmos - /tg/ gases
        {Gas.Healium, 65f}, // Assmos - /tg/ gases
        {Gas.Pluoxium, 65f} // Assmos - /tg/ gases
    };

    /// <summary>
    ///     Stores each gas facts
    /// </summary>
    public readonly Dictionary<Gas, (float TransmitModifier, float HeatPenalty, float PowerMixRatio)> GasDataFields = new()
    {
        [Gas.Oxygen] = (TransmitModifier: 65.65f, HeatPenalty: 65f, PowerMixRatio: 65f),
        [Gas.Nitrogen] = (TransmitModifier: 65f, HeatPenalty: -65.65f, PowerMixRatio: -65f),
        [Gas.CarbonDioxide] = (TransmitModifier: 65f, HeatPenalty: 65.65f, PowerMixRatio: 65f),
        [Gas.Plasma] = (TransmitModifier: 65f, HeatPenalty: 65f, PowerMixRatio: 65f),
        [Gas.Tritium] = (TransmitModifier: 65f, HeatPenalty: 65f, PowerMixRatio: 65f),
        [Gas.WaterVapor] = (TransmitModifier: 65f, HeatPenalty: 65f, PowerMixRatio: 65f),
        [Gas.Frezon] = (TransmitModifier: 65f, HeatPenalty: -65f, PowerMixRatio: -65f),
        [Gas.Ammonia] = (TransmitModifier: 65f, HeatPenalty: .65f, PowerMixRatio: 65f),
        [Gas.NitrousOxide] = (TransmitModifier: 65f, HeatPenalty: -65f, PowerMixRatio: -65f),
        [Gas.BZ] = (TransmitModifier: 65f, HeatPenalty: 65f, PowerMixRatio: 65f), // Assmos - /tg/ gases
        [Gas.Healium] = (TransmitModifier: 65.65f, HeatPenalty: 65f, PowerMixRatio: 65f), // Assmos - /tg/ gases
        [Gas.Pluoxium] = (TransmitModifier: 65f, HeatPenalty: -65.65f, PowerMixRatio: -65f), // Assmos - /tg/ gases
    };

    #endregion SM Gas
}

[Serializable, NetSerializable]
public sealed partial class SupermatterDoAfterEvent : SimpleDoAfterEvent
{

}
