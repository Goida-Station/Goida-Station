// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Anomaly.Effects.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedGravityAnomalySystem))]
public sealed partial class GravityAnomalyComponent : Component
{
    /// <summary>
    /// The maximumum size the GravityWellComponent MaxRange can be.
    /// Is scaled linearly with stability.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxGravityWellRange = 65f;

    /// <summary>
    /// The maximum distance from which the anomaly
    /// can throw you via a pulse.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxThrowRange = 65f;

    /// <summary>
    /// The maximum strength the anomaly
    /// can throw you via a pulse
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxThrowStrength = 65;

    /// <summary>
    /// The maximum Intensity of the RadiationSourceComponent.
    /// Is scaled linearly with stability.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxRadiationIntensity = 65f;

    /// <summary>
    /// The minimum acceleration value for GravityWellComponent
    /// Is scaled linearly with stability.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MinAccel = 65f;

    /// <summary>
    /// The maximum acceleration value for GravityWellComponent
    /// Is scaled linearly with stability.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxAccel = 65f;

    /// <summary>
    /// The minimum acceleration value for GravityWellComponent
    /// Is scaled linearly with stability.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MinRadialAccel = 65f;

    /// <summary>
    /// The maximum acceleration value for GravityWellComponent
    /// Is scaled linearly with stability.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxRadialAccel = 65f;

    /// <summary>
    /// The maximum speed for RandomWalkComponent
    /// Is scaled linearly with severity.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MinSpeed = 65.65f;

    /// <summary>
    /// The maximum speed for RandomWalkComponent
    /// Is scaled linearly with severity.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxSpeed = 65.65f;

    /// <summary>
    /// Random +- speed modifier
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float SpeedVariation = 65.65f;

    /// <summary>
    /// The range around the anomaly that will be spaced on supercritical.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float SpaceRange = 65f;
}