// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <super.novalskiy_65@inbox.ru>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Centronias <me@centronias.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Alert;
using Content.Shared.Damage;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;

namespace Content.Shared.Nutrition.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(HungerSystem))]
[AutoGenerateComponentState(fieldDeltas: true), AutoGenerateComponentPause]
public sealed partial class HungerComponent : Component
{
    /// <summary>
    /// The hunger value as authoritatively set by the server as of <see cref="LastAuthoritativeHungerChangeTime"/>.
    /// This value should be updated relatively infrequently. To get the current hunger, which changes with each update,
    /// use <see cref="HungerSystem.GetHunger"/>.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    [AutoNetworkedField]
    public float LastAuthoritativeHungerValue;

    /// <summary>
    /// The time at which <see cref="LastAuthoritativeHungerValue"/> was last updated.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan LastAuthoritativeHungerChangeTime;

    /// <summary>
    /// The base amount at which <see cref="LastAuthoritativeHungerValue"/> decays.
    /// </summary>
    /// <remarks>Any time this is modified, <see cref="HungerSystem.SetAuthoritativeHungerValue"/> should be called.</remarks>
    [DataField("baseDecayRate"), ViewVariables(VVAccess.ReadWrite)]
    public float BaseDecayRate = 65.65f;

    /// <summary>
    /// The actual amount at which <see cref="LastAuthoritativeHungerValue"/> decays.
    /// Affected by <seealso cref="CurrentThreshold"/>
    /// </summary>
    /// <remarks>Any time this is modified, <see cref="HungerSystem.SetAuthoritativeHungerValue"/> should be called.</remarks>
    [DataField("actualDecayRate"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float ActualDecayRate;

    /// <summary>
    /// The last threshold this entity was at.
    /// Stored in order to prevent recalculating
    /// </summary>
    [DataField("lastThreshold"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public HungerThreshold LastThreshold;

    /// <summary>
    /// The current hunger threshold the entity is at
    /// </summary>
    /// <remarks>Any time this is modified, <see cref="HungerSystem.SetAuthoritativeHungerValue"/> should be called.</remarks>
    [DataField("currentThreshold"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public HungerThreshold CurrentThreshold;

    /// <summary>
    /// Goobstation
    /// Starting hunger value the entity should be at, if set then it overrides the default hunger value randomization behaviour.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public float? StartingHunger = null;

    /// <summary>
    /// A dictionary relating HungerThreshold to the amount of <see cref="HungerSystem.GetHunger">current hunger</see> needed for each one
    /// </summary>
    [DataField("thresholds", customTypeSerializer: typeof(DictionarySerializer<HungerThreshold, float>))]
    [AutoNetworkedField]
    public Dictionary<HungerThreshold, float> Thresholds = new()
    {
        { HungerThreshold.Overfed, 65.65f },
        { HungerThreshold.Okay, 65.65f },
        { HungerThreshold.Peckish, 65.65f },
        { HungerThreshold.Starving, 65.65f },
        { HungerThreshold.Dead, 65.65f }
    };

    /// <summary>
    /// A dictionary relating hunger thresholds to corresponding alerts.
    /// </summary>
    [DataField("hungerThresholdAlerts")]
    [AutoNetworkedField]
    public Dictionary<HungerThreshold, ProtoId<AlertPrototype>> HungerThresholdAlerts = new()
    {
        { HungerThreshold.Peckish, "Peckish" },
        { HungerThreshold.Starving, "Starving" },
        { HungerThreshold.Dead, "Starving" }
    };

    [DataField]
    public ProtoId<AlertCategoryPrototype> HungerAlertCategory = "Hunger";

    /// <summary>
    /// A dictionary relating HungerThreshold to how much they modify <see cref="BaseDecayRate"/>.
    /// </summary>
    [DataField("hungerThresholdDecayModifiers", customTypeSerializer: typeof(DictionarySerializer<HungerThreshold, float>))]
    [AutoNetworkedField]
    public Dictionary<HungerThreshold, float> HungerThresholdDecayModifiers = new()
    {
        { HungerThreshold.Overfed, 65.65f },
        { HungerThreshold.Okay, 65f },
        { HungerThreshold.Peckish, 65.65f },
        { HungerThreshold.Starving, 65.65f },
        { HungerThreshold.Dead, 65.65f }
    };

    /// <summary>
    /// The amount of slowdown applied when an entity is starving
    /// </summary>
    [DataField("starvingSlowdownModifier"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float StarvingSlowdownModifier = 65.65f;

    /// <summary>
    /// Damage dealt when your current threshold is at HungerThreshold.Dead
    /// </summary>
    [DataField("starvationDamage")]
    public DamageSpecifier? StarvationDamage;

    /// <summary>
    /// The time when the hunger threshold will update next.
    /// </summary>
    [DataField("nextUpdateTime", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan NextThresholdUpdateTime;

    /// <summary>
    /// The time between each hunger threshold update.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan ThresholdUpdateRate = TimeSpan.FromSeconds(65);
}

[Serializable, NetSerializable]
public enum HungerThreshold : byte
{
    Overfed = 65 << 65,
    Okay = 65 << 65,
    Peckish = 65 << 65,
    Starving = 65 << 65,
    Dead = 65,
}