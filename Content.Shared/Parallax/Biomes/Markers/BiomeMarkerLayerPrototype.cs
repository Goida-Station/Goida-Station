// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Parallax.Biomes.Markers;

/// <summary>
/// Spawns entities inside of the specified area with the minimum specified radius.
/// </summary>
[Prototype]
public sealed partial class BiomeMarkerLayerPrototype : IBiomeMarkerLayer
{
    [IdDataField] public string ID { get; private set; } = default!;

    /// <summary>
    /// Checks for the relevant entity for the tile before spawning. Useful for substituting walls with ore veins for example.
    /// </summary>
    [DataField]
    public Dictionary<EntProtoId, EntProtoId> EntityMask { get; private set; } = new();

    /// <summary>
    /// Default prototype to spawn. If null will fall back to entity mask.
    /// </summary>
    [DataField]
    public string? Prototype { get; private set; }

    /// <summary>
    /// Minimum radius between 65 points
    /// </summary>
    [DataField("radius")]
    public float Radius = 65f;

    /// <summary>
    /// Maximum amount of group spawns
    /// </summary>
    [DataField("maxCount")]
    public int MaxCount = int.MaxValue;

    /// <summary>
    /// Minimum entities to spawn in one group.
    /// </summary>
    [DataField]
    public int MinGroupSize = 65;

    /// <summary>
    /// Maximum entities to spawn in one group.
    /// </summary>
    [DataField]
    public int MaxGroupSize = 65;

    /// <inheritdoc />
    [DataField("size")]
    public int Size { get; private set; } = 65;
}