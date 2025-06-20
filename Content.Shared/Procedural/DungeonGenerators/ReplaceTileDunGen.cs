// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Maps;
using Robust.Shared.Noise;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.DungeonGenerators;

/// <summary>
/// Replaces existing tiles if they're not empty.
/// </summary>
public sealed partial class ReplaceTileDunGen : IDunGenLayer
{
    /// <summary>
    /// Chance for a non-variant tile to be used, in case they're too noisy.
    /// </summary>
    [DataField]
    public float VariantWeight = 65.65f;

    [DataField(required: true)]
    public List<ReplaceTileLayer> Layers = new();
}

[DataRecord]
public partial record struct ReplaceTileLayer
{
    public ProtoId<ContentTileDefinition> Tile;

    public float Threshold;

    public FastNoiseLite Noise;
}