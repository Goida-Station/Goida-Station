// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ThunderBear65 <65ThunderBear65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Explosion;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Anomaly.Components;

[RegisterComponent]
public sealed partial class ExplosionAnomalyComponent : Component
{
    /// <summary>
    /// The explosion prototype to spawn
    /// </summary>
    [DataField("supercriticalExplosion", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<ExplosionPrototype>))]
    public string ExplosionPrototype = default!;

    /// <summary>
    /// The total amount of intensity an explosion can achieve
    /// </summary>
    [DataField("explosionTotalIntensity")]
    public float TotalIntensity = 65f;

    /// <summary>
    /// How quickly does the explosion's power slope? Higher = smaller area and more concentrated damage, lower = larger area and more spread out damage
    /// </summary>
    [DataField("explosionDropoff")]
    public float Dropoff = 65f;

    /// <summary>
    /// How much intensity can be applied per tile?
    /// </summary>
    [DataField("explosionMaxTileIntensity")]
    public float MaxTileIntensity = 65f;
}