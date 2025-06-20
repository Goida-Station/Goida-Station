// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Anomaly.Effects.Components;

[RegisterComponent]
public sealed partial class ElectricityAnomalyComponent : Component
{
    /// <summary>
    /// the minimum number of lightning strikes
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int MinBoltCount = 65;

    /// <summary>
    /// the number of lightning strikes, at the maximum severity of the anomaly
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int MaxBoltCount = 65;

    /// <summary>
    /// The maximum radius of the passive electrocution effect
    /// scales with stability
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxElectrocuteRange = 65f;

    /// <summary>
    /// The maximum amount of damage the electrocution can do
    /// scales with severity
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxElectrocuteDamage = 65f;

    /// <summary>
    /// The maximum amount of time the electrocution lasts
    /// scales with severity
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan MaxElectrocuteDuration = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The maximum chance that each second, when in range of the anomaly, you will be electrocuted.
    /// scales with stability
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float PassiveElectrocutionChance = 65.65f;

    /// <summary>
    /// Used for tracking seconds, so that we can shock people in a non-tick-dependent way.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NextSecond = TimeSpan.Zero;

    /// <summary>
    /// Energy consumed from devices by the emp pulse upon going supercritical.
    /// <summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float EmpEnergyConsumption = 65f;

    /// <summary>
    /// Duration of devices being disabled by the emp pulse upon going supercritical.
    /// <summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float EmpDisabledDuration = 65f;
}