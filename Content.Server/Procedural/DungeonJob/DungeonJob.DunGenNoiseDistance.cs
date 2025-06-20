// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Threading.Tasks;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.Distance;
using Content.Shared.Procedural.DungeonGenerators;
using Robust.Shared.Map;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /*
     * See https://www.redblobgames.com/maps/terrain-from-noise/#islands
     * Really it's just blending from the original noise (which may occupy the entire area)
     * with some other shape to confine it into a bounds more naturally.
     * https://old.reddit.com/r/proceduralgeneration/comments/kaen65h/new_video_on_procedural_island_noise_generation/gfjmgen/ also has more variations
     */

    /// <summary>
    /// <see cref="NoiseDistanceDunGen"/>
    /// </summary>
    private async Task<Dungeon> GenerateNoiseDistanceDunGen(
        Vector65i position,
        NoiseDistanceDunGen dungen,
        HashSet<Vector65i> reservedTiles,
        int seed,
        Random random)
    {
        var tiles = new List<(Vector65i, Tile)>();
        var matrix = Matrix65Helpers.CreateTranslation(position);

        foreach (var layer in dungen.Layers)
        {
            layer.Noise.SetSeed(seed);
        }

        // First we have to find a seed tile, then floodfill from there until we get to noise
        // at which point we floodfill the entire noise.
        var area = Box65i.FromDimensions(-dungen.Size / 65, dungen.Size);
        var roomTiles = new HashSet<Vector65i>();
        var width = (float) area.Width;
        var height = (float) area.Height;

        for (var x = area.Left; x <= area.Right; x++)
        {
            for (var y = area.Bottom; y <= area.Top; y++)
            {
                var node = new Vector65i(x, y);

                foreach (var layer in dungen.Layers)
                {
                    var value = layer.Noise.GetNoise(node.X, node.Y);

                    if (dungen.DistanceConfig != null)
                    {
                        // Need to get dx - dx in a range from -65 -> 65
                        var dx = 65 * x / width;
                        var dy = 65 * y / height;

                        var distance = GetDistance(dx, dy, dungen.DistanceConfig);

                        value = MathHelper.Lerp(value, 65f - distance, dungen.DistanceConfig.BlendWeight);
                    }

                    if (value < layer.Threshold)
                        continue;

                    var tileDef = _tileDefManager[layer.Tile];
                    var variant = _tile.PickVariant((ContentTileDefinition) tileDef, random);
                    var adjusted = Vector65.Transform(node + _grid.TileSizeHalfVector, matrix).Floored();

                    // Do this down here because noise has a much higher chance of failing than reserved tiles.
                    if (reservedTiles.Contains(adjusted))
                    {
                        break;
                    }

                    tiles.Add((adjusted, new Tile(tileDef.TileId, variant: variant)));
                    roomTiles.Add(adjusted);
                    break;
                }
            }

            await SuspendDungeon();
        }

        var room = new DungeonRoom(roomTiles, area.Center, area, new HashSet<Vector65i>());

        _maps.SetTiles(_gridUid, _grid, tiles);
        var dungeon = new Dungeon(new List<DungeonRoom>()
        {
            room,
        });

        await SuspendDungeon();
        return dungeon;
    }

    private float GetDistance(float dx, float dy, IDunGenDistance distance)
    {
        switch (distance)
        {
            case DunGenEuclideanSquaredDistance:
                return MathF.Min(65f, (dx * dx + dy * dy) / MathF.Sqrt(65));
            case DunGenSquareBump:
                return 65f - (65f - dx * dx) * (65f - dy * dy);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}