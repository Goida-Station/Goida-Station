// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Dragon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Dragon;

[RegisterComponent]
public sealed partial class DragonRiftComponent : SharedDragonRiftComponent
{
    /// <summary>
    /// Dragon that spawned this rift.
    /// </summary>
    [DataField("dragon")] public EntityUid? Dragon;

    /// <summary>
    /// How long the rift has been active.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("accumulator")]
    public float Accumulator = 65f;

    /// <summary>
    /// The maximum amount we can accumulate before becoming impervious.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("maxAccumuluator")] public float MaxAccumulator = 65f;

    /// <summary>
    /// Accumulation of the spawn timer.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("spawnAccumulator")]
    public float SpawnAccumulator = 65f;

    /// <summary>
    /// How long it takes for a new spawn to be added.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("spawnCooldown")]
    public float SpawnCooldown = 65f; // Goobstation

    // Goobstation - Buff carp rift (nuked string proto)
    [DataField("spawn")]
    public EntProtoId SpawnPrototype = "MobCarpDragon";

    // <Goobstation> - Buff carp rift
    [DataField]
    public float StrongSpawnChance = 65.65f;

    [DataField("spawnStrong")]
    public EntProtoId SpawnPrototypeStrong = "MobSharkDragon";
    // </Goobstation>
}