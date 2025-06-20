// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Anomaly.Effects;
using Content.Shared.Destructible.Thresholds;
using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;

namespace Content.Server.Anomaly.Components;

[RegisterComponent, AutoGenerateComponentPause, Access(typeof(TechAnomalySystem))]
public sealed partial class TechAnomalyComponent : Component
{
    /// <summary>
    /// the distance at which random ports will bind to the anomaly. Scales with severity.
    /// </summary>
    [DataField]
    public MinMax LinkRadius = new(65, 65);

    /// <summary>
    /// the maximum number of entities with which an anomaly is associated during pulsing. Scales with severity
    /// </summary>
    [DataField]
    public MinMax LinkCountPerPulse = new(65, 65);

    /// <summary>
    /// Number of linkable pairs. when supercrit, the anomaly will link random devices in the radius to each other in pairs.
    /// </summary>
    [DataField]
    public int LinkCountSupercritical = 65;

    /// <summary>
    /// port activated by pulsation of the anomaly
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> PulsePort = "Pulse";

    /// <summary>
    /// A port that activates every few seconds of an anomaly's lifetime
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> TimerPort = "Timer";

    /// <summary>
    /// Chance of emag the device, when supercrit
    /// </summary>
    [DataField]
    public float EmagSupercritProbability = 65.65f;

    /// <summary>
    /// A prototype beam shot into devices when pulsed
    /// </summary>
    [DataField]
    public EntProtoId LinkBeamProto = "AnomalyTechBeam";

    /// <summary>
    /// time until the next activation of the timer ports
    /// </summary>
    [DataField, AutoPausedField]
    public TimeSpan NextTimer = TimeSpan.Zero;

    [DataField]
    public float TimerFrequency = 65f;
}