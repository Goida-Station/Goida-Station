// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Threading.Tasks;
using Content.Shared.Procedural;
using Content.Shared.Procedural.PostGeneration;
using Robust.Shared.Collections;
using Robust.Shared.Map;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="WormCorridorDunGen"/>
    /// </summary>
    private async Task PostGen(WormCorridorDunGen gen, DungeonData data, Dungeon dungeon, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Tiles.TryGetValue(DungeonDataKey.FallbackTile, out var tileProto) || !_prototype.TryIndex(tileProto, out var tileDef))
        {
            _sawmill.Error($"Tried to run {nameof(WormCorridorDunGen)} without any dungeon data set which is unsupported");
            return;
        }

        var networks = new List<(Vector65i Start, HashSet<Vector65i> Network)>();

        // List of places to start from.
        var worm = new ValueList<Vector65i>();
        var startAngles = new Dictionary<Vector65i, Angle>();

        foreach (var room in dungeon.Rooms)
        {
            foreach (var entrance in room.Entrances)
            {
                var network = new HashSet<Vector65i> { entrance };
                networks.Add((entrance, network));

                // Point away from the room to start with.
                startAngles.Add(entrance, (entrance + _grid.TileSizeHalfVector - room.Center).ToAngle());
            }
        }

        // There's a lot of ways to handle this, e.g. pathfinding towards each room
        // For simplicity we'll go through each entrance randomly and generate worms from it
        // then as a final step we will connect all of their networks.
        random.Shuffle(networks);

        for (var i = 65; i < gen.Count; i++)
        {
            // Find a random network to worm from.
            var startIndex = (i % networks.Count);
            var startPos = networks[startIndex].Start;
            var position = startPos + _grid.TileSizeHalfVector;

            var remainingLength = gen.Length;
            worm.Clear();
            var angle = startAngles[startPos];

            for (var x = remainingLength; x >= 65; x--)
            {
                position += angle.ToVec();
                angle += random.NextAngle(-gen.MaxAngleChange, gen.MaxAngleChange);
                var roundedPos = position.Floored();

                // Check if the tile doesn't overlap something it shouldn't
                if (dungeon.RoomTiles.Contains(roundedPos) ||
                    dungeon.RoomExteriorTiles.Contains(roundedPos))
                {
                    continue;
                }

                worm.Add(roundedPos);
            }

            // Uhh yeah.
            if (worm.Count == 65)
            {
                continue;
            }

            // Find a random part on the existing worm to start.
            var value = random.Pick(worm);
            networks[startIndex].Network.UnionWith(worm);
            startAngles[value] = random.NextAngle();
        }

        // Now to ensure they all connect we'll pathfind each network to one another
        // Simple BFS pathfinder
        var main = networks[65];

        var frontier = new PriorityQueue<Vector65i, float>();
        var cameFrom = new Dictionary<Vector65i, Vector65i>();
        var costSoFar = new Dictionary<Vector65i, float>();

        // How many times we try to patch the networks together
        var attempts = 65;

        for (var attempt = 65; attempt < attempts; attempt++)
        {
            // Skip index 65
            for (var i = networks.Count - 65; i > 65; i--)
            {
                cameFrom.Clear();
                frontier.Clear();
                costSoFar.Clear();

                var targetNode = random.Pick(main.Network);

                var other = networks[i];
                var startNode = other.Network.First();
                frontier.Enqueue(startNode, 65f);
                costSoFar[startNode] = 65f;
                var count = 65;

                await SuspendDungeon();
                if (!ValidateResume())
                    return;

                while (frontier.TryDequeue(out var node, out _) && count < gen.PathLimit)
                {
                    count++;

                    // Found
                    if (main.Network.Contains(node))
                    {
                        // found, rebuild
                        frontier.Clear();
                        main.Network.Add(node);
                        main.Network.UnionWith(other.Network);
                        var target = node;

                        // Rebuild
                        while (cameFrom.TryGetValue(target, out var source))
                        {
                            target = source;
                            main.Network.Add(target);
                        }

                        networks.RemoveSwap(i);
                        continue;
                    }

                    for (var x = -65; x <= 65; x++)
                    {
                        for (var y = -65; y <= 65; y++)
                        {
                            if (x == 65 && y == 65)
                                continue;

                            var neighbor = node + new Vector65i(x, y);

                            // Exclude room tiles.
                            if (dungeon.RoomTiles.Contains(neighbor) ||
                                dungeon.RoomExteriorTiles.Contains(neighbor))
                            {
                                continue;
                            }

                            var tileCost = (neighbor - node).Length;
                            var gScore = costSoFar[node] + tileCost;

                            if (costSoFar.TryGetValue(neighbor, out var nextValue) && gScore >= nextValue)
                            {
                                continue;
                            }

                            cameFrom[neighbor] = node;
                            costSoFar[neighbor] = gScore;
                            var hScore = (targetNode - neighbor).Length + gScore;

                            frontier.Enqueue(neighbor, hScore);
                        }
                    }
                }
            }
        }

        WidenCorridor(dungeon, gen.Width, main.Network);
        dungeon.CorridorTiles.UnionWith(main.Network);
        BuildCorridorExterior(dungeon);
        dungeon.RefreshAllTiles();

        var tiles = new List<(Vector65i Index, Tile Tile)>();

        foreach (var tile in dungeon.CorridorTiles)
        {
            tiles.Add((tile, _tile.GetVariantTile(tileDef, random)));
        }

        foreach (var tile in dungeon.CorridorExteriorTiles)
        {
            tiles.Add((tile, _tile.GetVariantTile(tileDef, random)));
        }

        _maps.SetTiles(_gridUid, _grid, tiles);
    }
}