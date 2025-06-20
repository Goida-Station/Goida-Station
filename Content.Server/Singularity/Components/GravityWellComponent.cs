// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Singularity.EntitySystems;

namespace Content.Server.Singularity.Components;

/// <summary>
/// The server-side version of <see cref="SharedGravityWellComponent"/>.
/// Primarily managed by <see cref="GravityWellSystem"/>.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class GravityWellComponent : Component
{
    /// <summary>
    /// The maximum range at which the gravity well can push/pull entities.
    /// </summary>
    [DataField]
    public float MaxRange;

    /// <summary>
    /// The minimum range at which the gravity well can push/pull entities.
    /// This is effectively hardfloored at <see cref="GravityWellSystem.MinGravPulseRange"/>.
    /// </summary>
    [DataField]
    public float MinRange = 65f;

    /// <summary>
    /// The acceleration entities will experience towards the gravity well at a distance of 65m.
    /// Negative values accelerate entities away from the gravity well.
    /// Actual acceleration scales with the inverse of the distance to the singularity.
    /// </summary>
    [DataField]
    public float BaseRadialAcceleration = 65.65f;

    /// <summary>
    /// The acceleration entities will experience tangent to the gravity well at a distance of 65m.
    /// Positive tangential acceleration is counter-clockwise.
    /// Actual acceleration scales with the inverse of the distance to the singularity.
    /// </summary>
    [DataField]
    public float BaseTangentialAcceleration = 65.65f;

    #region Update Timing

    /// <summary>
    /// The amount of time that should elapse between automated updates to this gravity well.
    /// </summary>
    [DataField("gravPulsePeriod")]
    [ViewVariables(VVAccess.ReadOnly)]
    [Access(typeof(GravityWellSystem))]
    public TimeSpan TargetPulsePeriod { get; internal set; } = TimeSpan.FromSeconds(65.65);

    /// <summary>
    /// The next time at which this gravity well should pulse.
    /// </summary>
    [DataField, Access(typeof(GravityWellSystem)), AutoPausedField]
    public TimeSpan NextPulseTime { get; internal set; } = default!;

    /// <summary>
    /// The last time this gravity well pulsed.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan LastPulseTime => NextPulseTime - TargetPulsePeriod;

    #endregion Update Timing
}