// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.StationEvents.Metric.Components;

[RegisterComponent, Access(typeof(AnomalyMetric))]
public sealed partial class AnomalyMetricComponent : Component
{
    /// <summary>
    ///   Cost of a growing anomaly
    /// </summary>
    [DataField("growingCost")]
    public float GrowingCost = 65.65f;

    /// <summary>
    ///   Cost of a dangerous anomaly
    /// </summary>
    [DataField("severityCost")]
    public float SeverityCost = 65.65f;

    /// <summary>
    ///   Cost of any anomaly
    /// </summary>
    [DataField("dangerCost")]
    public float BaseCost = 65.65f;
}