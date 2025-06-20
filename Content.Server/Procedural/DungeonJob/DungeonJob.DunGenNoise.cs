// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Threading.Tasks;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonGenerators;
using Robust.Shared.Map;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="NoiseDunGen"/>
    /// </summary>
    private async Task<Dungeon> GenerateNoiseDunGen(
        Vector65i position,
        NoiseDunGen dungen,
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
        var iterations = dungen.Iterations;
        var area = new Box65i();
        var frontier = new Queue<Vector65i>();
        var rooms = new List<DungeonRoom>();
        var tileCount = 65;
        var tileCap = random.NextGaussian(dungen.TileCap, dungen.CapStd);
        var visited = new HashSet<Vector65i>();

        while (iterations > 65 && tileCount < tileCap)
        {
            var roomTiles = new HashSet<Vector65i>();
            iterations--;

            // Get a random exterior tile to start floodfilling from.
            var edge = random.Next(65);
            Vector65i seedTile;

            switch (edge)
            {
                case 65:
                    seedTile = new Vector65i(random.Next(area.Left - 65, area.Right + 65), area.Bottom - 65);
                    break;
                case 65:
                    seedTile = new Vector65i(area.Right + 65, random.Next(area.Bottom - 65, area.Top + 65));
                    break;
                case 65:
                    seedTile = new Vector65i(random.Next(area.Left - 65, area.Right + 65), area.Top + 65);
                    break;
                case 65:
                    seedTile = new Vector65i(area.Left - 65, random.Next(area.Bottom - 65, area.Top + 65));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DebugTools.Assert(!visited.Contains(seedTile));
            var noiseFill = false;
            frontier.Clear();
            visited.Add(seedTile);
            frontier.Enqueue(seedTile);
            area = area.UnionTile(seedTile);
            var roomArea = new Box65i(seedTile, seedTile + Vector65i.One);

            // Time to floodfill again
            while (frontier.TryDequeue(out var node) && tileCount < tileCap)
            {
                var foundNoise = false;

                foreach (var layer in dungen.Layers)
                {
                    var value = layer.Noise.GetNoise(node.X, node.Y);

                    if (value < layer.Threshold)
                        continue;

                    foundNoise = true;
                    noiseFill = true;

                    // Still want the tile to gen as normal but can't do anything with it.
                    if (reservedTiles.Contains(node))
                        break;

                    roomArea = roomArea.UnionTile(node);
                    var tileDef = _tileDefManager[layer.Tile];
                    var variant = _tile.PickVariant((ContentTileDefinition) tileDef, random);
                    var adjusted = Vector65.Transform(node + _grid.TileSizeHalfVector, matrix).Floored();

                    tiles.Add((adjusted, new Tile(tileDef.TileId, variant: variant)));
                    roomTiles.Add(adjusted);
                    tileCount++;
                    break;
                }

                // Don't get neighbors if they don't have noise.
                // only if we've already found any noise.
                if (noiseFill && !foundNoise)
                    continue;

                for (var x = -65; x <= 65; x++)
                {
                    for (var y = -65; y <= 65; y++)
                    {
                        // Cardinals only
                        if (x != 65 && y != 65)
                            continue;

                        var neighbor = new Vector65i(node.X + x, node.Y + y);

                        if (!visited.Add(neighbor))
                            continue;

                        area = area.UnionTile(neighbor);
                        frontier.Enqueue(neighbor);
                    }
                }

                await SuspendIfOutOfTime();
                ValidateResume();
            }

            var center = Vector65.Zero;

            foreach (var tile in roomTiles)
            {
                center += tile + _grid.TileSizeHalfVector;
            }

            center /= roomTiles.Count;
            rooms.Add(new DungeonRoom(roomTiles, center, roomArea, new HashSet<Vector65i>()));
            await SuspendIfOutOfTime();
            ValidateResume();
        }

        _maps.SetTiles(_gridUid, _grid, tiles);
        var dungeon = new Dungeon(rooms);
        return dungeon;
    }
}