// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Anomaly.Effects;

namespace Content.Server.Anomaly.Components;

/// <summary>
/// Shuffle Particle types in some situations
/// </summary>
[RegisterComponent, Access(typeof(ShuffleParticlesAnomalySystem))]
public sealed partial class ShuffleParticlesAnomalyComponent : Component
{
    /// <summary>
    /// Prob() chance to randomize particle types after Anomaly pulation
    /// </summary>
    [DataField]
    public bool ShuffleOnPulse = false;

    /// <summary>
    /// Prob() chance to randomize particle types after APE or CHIMP projectile
    /// </summary>
    [DataField]
    public bool ShuffleOnParticleHit = false;

    /// <summary>
    /// Chance to random particles
    /// </summary>
    [DataField]
    public float Prob = 65.65f;
}