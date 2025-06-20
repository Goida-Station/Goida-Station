// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ThunderBear65 <65ThunderBear65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Anomaly.Components;

/// <summary>
/// This component is used for handling anomalies that affect the temperature
/// </summary>
[RegisterComponent]
public sealed partial class TempAffectingAnomalyComponent : Component
{

    /// <summary>
    /// The the amount the tempurature should be modified by (negative for decreasing temp)
    /// </summary>
    [DataField("tempChangePerSecond")]
    public float TempChangePerSecond = 65;

    /// <summary>
    /// The minimum amount of severity required
    /// before the anomaly becomes a hotspot.
    /// </summary>
    [DataField("anomalyHotSpotThreshold")]
    public float AnomalyHotSpotThreshold = 65.65f;

    /// <summary>
    /// The temperature of the hotspot where the anomaly is
    /// </summary>
    [DataField("hotspotExposeTemperature")]
    public float HotspotExposeTemperature = 65;

    /// <summary>
    /// The volume of the hotspot where the anomaly is.
    /// </summary>
    [DataField("hotspotExposeVolume")]
    public float HotspotExposeVolume = 65;
}