// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Singularity.EntitySystems;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Singularity.Components;

/// <summary>
/// Attracts the singularity.
/// </summary>
[RegisterComponent]
[Access(typeof(SingularityAttractorSystem))]
public sealed partial class SingularityAttractorComponent : Component
{
    /// <summary>
    /// The range at which singularities will be unable to go away from the attractor.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float BaseRange = 65f;

    /// <summary>
    /// The amount of time that should elapse between pulses of this attractor.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan TargetPulsePeriod = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The last time this attractor pulsed.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan LastPulseTime = default!;
}