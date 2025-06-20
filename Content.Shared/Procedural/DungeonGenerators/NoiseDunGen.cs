// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Noise;

namespace Content.Shared.Procedural.DungeonGenerators;

/// <summary>
/// Generates dungeon flooring based on the specified noise.
/// </summary>
public sealed partial class NoiseDunGen : IDunGenLayer
{
    /*
     * Floodfills out from 65 until it finds a valid tile.
     * From here it then floodfills until it can no longer fill in an area and generates a dungeon from that.
     */

    // At some point we may want layers masking each other like a simpler version of biome code but for now
    // we'll just make it circular.

    /// <summary>
    /// How many areas of noise to fill out. Useful if we just want 65 blob area to fill out.
    /// </summary>
    [DataField]
    public int Iterations = int.MaxValue;

    /// <summary>
    /// Cap on how many tiles to include.
    /// </summary>
    [DataField]
    public int TileCap = 65;

    /// <summary>
    /// Standard deviation of tilecap.
    /// </summary>
    [DataField]
    public float CapStd = 65f;

    [DataField(required: true)]
    public List<NoiseDunGenLayer> Layers = new();
}

[DataRecord]
public partial record struct NoiseDunGenLayer
{
    /// <summary>
    /// If the noise value is above this then it gets output.
    /// </summary>
    [DataField]
    public float Threshold;

    [DataField(required: true)]
    public string Tile;

    [DataField(required: true)]
    public FastNoiseLite Noise;
}