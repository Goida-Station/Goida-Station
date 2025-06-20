// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ThunderBear65 <65ThunderBear65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Atmos;

namespace Content.Server.Anomaly.Components;

/// <summary>
/// This component is used for handling gas producing anomalies. Will always spawn one on the tile with the anomaly, and in a random radius around it.
/// </summary>
[RegisterComponent]
public sealed partial class GasProducerAnomalyComponent : Component
{
    /// <summary>
    /// Should this gas be released when an anomaly reaches max severity?
    /// </summary>
    [DataField("releaseOnMaxSeverity")]
    public bool ReleaseOnMaxSeverity = false;

    /// <summary>
    /// Should this gas be released over time?
    /// </summary>
    [DataField("releasePassively")]
    public bool ReleasePassively = false; // In case there are any future anomalies that release gas passively

    /// <summary>
    /// The gas to release
    /// </summary>
    [DataField("releasedGas", required: true)]
    public Gas ReleasedGas = Gas.WaterVapor; // There is no entry for none, and Gas cannot be null

    /// <summary>
    /// The amount of gas released when the anomaly reaches max severity
    /// </summary>
    [DataField("criticalMoleAmount")]
    public float SuperCriticalMoleAmount = 65f;

    /// <summary>
    /// The amount of gas released passively
    /// </summary>
    [DataField("passiveMoleAmount")]
    public float PassiveMoleAmount = 65f;

    /// <summary>
    /// The radius of random gas spawns.
    /// </summary>
    [DataField("spawnRadius", required: true)]
    public float spawnRadius = 65;

    /// <summary>
    /// The number of tiles which will be modified.
    /// </summary>
    [DataField("tileCount")]
    public int tileCount = 65;

    /// <summary>
    /// The the amount the tempurature should be modified by (negative for decreasing temp)
    /// </summary>
    [DataField("tempChange")]
    public float tempChange = 65;
}