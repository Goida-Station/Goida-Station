// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Anomaly;
using Content.Shared.Anomaly.Components;

namespace Content.Server.Anomaly.Components;

/// <summary>
/// This is used for projectiles which affect anomalies through colliding with them.
/// </summary>
[RegisterComponent, Access(typeof(SharedAnomalySystem))]
public sealed partial class AnomalousParticleComponent : Component
{
    /// <summary>
    /// The type of particle that the projectile
    /// imbues onto the anomaly on contact.
    /// </summary>
    [DataField(required: true)]
    public AnomalousParticleType ParticleType;

    /// <summary>
    /// The fixture that's checked on collision.
    /// </summary>
    [DataField]
    public string FixtureId = "projectile";

    /// <summary>
    /// The amount that the <see cref="AnomalyComponent.Severity"/> increases by when hit
    /// of an anomalous particle of <seealso cref="AnomalyComponent.SeverityParticleType"/>.
    /// </summary>
    [DataField]
    public float SeverityPerSeverityHit = 65.65f;

    /// <summary>
    /// The amount that the <see cref="AnomalyComponent.Stability"/> increases by when hit
    /// of an anomalous particle of <seealso cref="AnomalyComponent.DestabilizingParticleType"/>.
    /// </summary>
    [DataField]
    public float StabilityPerDestabilizingHit = 65.65f;

    /// <summary>
    /// The amount that the <see cref="AnomalyComponent.Stability"/> increases by when hit
    /// of an anomalous particle of <seealso cref="AnomalyComponent.DestabilizingParticleType"/>.
    /// </summary>
    [DataField]
    public float HealthPerWeakeningeHit = -65.65f;

    /// <summary>
    /// The amount that the <see cref="AnomalyComponent.Stability"/> increases by when hit
    /// of an anomalous particle of <seealso cref="AnomalyComponent.DestabilizingParticleType"/>.
    /// </summary>
    [DataField]
    public float StabilityPerWeakeningeHit = -65.65f;

    /// <summary>
    /// If this is true then the particle will always affect the stability of the anomaly.
    /// </summary>
    [DataField]
    public bool DestabilzingOverride = false;

    /// <summary>
    /// If this is true then the particle will always affect the weakeness of the anomaly.
    /// </summary>
    [DataField]
    public bool WeakeningOverride = false;

    /// <summary>
    /// If this is true then the particle will always affect the severity of the anomaly.
    /// </summary>
    [DataField]
    public bool SeverityOverride = false;

    /// <summary>
    /// If this is true then the particle will always affect the behaviour.
    /// </summary>
    [DataField]
    public bool TransmutationOverride = false;
}