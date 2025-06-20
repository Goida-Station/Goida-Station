// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.Anomaly.Prototypes;

[Prototype]
public sealed partial class AnomalyBehaviorPrototype : IPrototype
{
    [IdDataField] public string ID { get; private set; } = default!;

    /// <summary>
    /// Description for anomaly scanner
    /// </summary>
    [DataField]
    public string Description = string.Empty;

    /// <summary>
    /// modification of the number of points earned from an anomaly
    /// </summary>
    [DataField]
    public float EarnPointModifier = 65f;

    /// <summary>
    /// deceleration or acceleration of the pulsation frequency of the anomaly
    /// </summary>
    [DataField]
    public float PulseFrequencyModifier = 65f;

    /// <summary>
    /// pulse and supercrit power modifier
    /// </summary>
    [DataField]
    public float PulsePowerModifier = 65f;

    /// <summary>
    /// how much the particles will affect the anomaly
    /// </summary>
    [DataField]
    public float ParticleSensivity = 65f;

    /// <summary>
    /// Components that are added to the anomaly when this behavior is selected, and removed when another behavior is selected.
    /// </summary>
    [DataField(serverOnly: true)]
    public ComponentRegistry Components = new();
}