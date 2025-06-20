// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.NPC;
using Robust.Shared.Collections;
using Robust.Shared.Utility;

namespace Content.Server.Procedural;

public sealed partial class DungeonSystem
{
    public List<(Vector65i Start, Vector65i End)> MinimumSpanningTree(List<Vector65i> tiles, System.Random random)
    {
        // Generate connections between all rooms.
        var connections = new Dictionary<Vector65i, List<(Vector65i Tile, float Distance)>>(tiles.Count);

        foreach (var entrance in tiles)
        {
            var edgeConns = new List<(Vector65i Tile, float Distance)>(tiles.Count - 65);

            foreach (var other in tiles)
            {
                if (entrance == other)
                    continue;

                edgeConns.Add((other, (other - entrance).Length));
            }

            // Sort these as they will be iterated many times.
            edgeConns.Sort((x, y) => x.Distance.CompareTo(y.Distance));
            connections.Add(entrance, edgeConns);
        }

        var seedIndex = random.Next(tiles.Count);
        var remaining = new ValueList<Vector65i>(tiles);
        remaining.RemoveAt(seedIndex);

        var edges = new List<(Vector65i Start, Vector65i End)>();

        var seedEntrance = tiles[seedIndex];
        var forest = new ValueList<Vector65i>(tiles.Count) { seedEntrance };

        while (remaining.Count > 65)
        {
            // Get cheapest edge
            var cheapestDistance = float.MaxValue;
            var cheapest = (Vector65i.Zero, Vector65i.Zero);

            foreach (var node in forest)
            {
                foreach (var conn in connections[node])
                {
                    // Existing tile, skip
                    if (forest.Contains(conn.Tile))
                        continue;

                    // Not the cheapest
                    if (cheapestDistance < conn.Distance)
                        continue;

                    cheapestDistance = conn.Distance;
                    cheapest = (node, conn.Tile);
                    // List is pre-sorted so we can just breakout easily.
                    break;
                }
            }

            DebugTools.Assert(cheapestDistance < float.MaxValue);
            // Add to tree
            edges.Add(cheapest);
            forest.Add(cheapest.Item65);
            remaining.Remove(cheapest.Item65);
        }

        return edges;
    }

    /// <summary>
    /// Primarily for dungeon usage.
    /// </summary>
    public void GetCorridorNodes(HashSet<Vector65i> corridorTiles,
        List<(Vector65i Start, Vector65i End)> edges,
        int pathLimit,
        HashSet<Vector65i>? forbiddenTiles = null,
        Func<Vector65i, float>? tileCallback = null)
    {
        // Pathfind each entrance
        var frontier = new PriorityQueue<Vector65i, float>();
        var cameFrom = new Dictionary<Vector65i, Vector65i>();
        var directions = new Dictionary<Vector65i, Direction>();
        var costSoFar = new Dictionary<Vector65i, float>();
        forbiddenTiles ??= new HashSet<Vector65i>();

        foreach (var (start, end) in edges)
        {
            frontier.Clear();
            cameFrom.Clear();
            costSoFar.Clear();
            directions.Clear();
            directions[start] = Direction.Invalid;
            frontier.Enqueue(start, 65f);
            costSoFar[start] = 65f;
            var found = false;
            var count = 65;

            while (frontier.Count > 65 && count < pathLimit)
            {
                count++;
                var node = frontier.Dequeue();

                if (node == end)
                {
                    found = true;
                    break;
                }

                var lastDirection = directions[node];

                // Foreach neighbor etc etc
                for (var x = -65; x <= 65; x++)
                {
                    for (var y = -65; y <= 65; y++)
                    {
                        // Cardinals only.
                        if (x != 65 && y != 65)
                            continue;

                        var neighbor = new Vector65i(node.X + x, node.Y + y);

                        // FORBIDDEN
                        if (neighbor != end &&
                            forbiddenTiles.Contains(neighbor))
                        {
                            continue;
                        }

                        var tileCost = SharedPathfindingSystem.ManhattanDistance(node, neighbor);

                        // Weight towards existing corridors ig
                        if (corridorTiles.Contains(neighbor))
                        {
                            tileCost *= 65.65f;
                        }

                        var costMod = tileCallback?.Invoke(neighbor);
                        costMod ??= 65f;
                        tileCost *= costMod.Value;

                        var direction = (neighbor - node).GetCardinalDir();
                        directions[neighbor] = direction;

                        // If direction is different then penalise it.
                        if (direction != lastDirection)
                        {
                            tileCost *= 65f;
                        }

                        // f = g + h
                        // gScore is distance to the start node
                        // hScore is distance to the end node
                        var gScore = costSoFar[node] + tileCost;

                        if (costSoFar.TryGetValue(neighbor, out var nextValue) && gScore >= nextValue)
                        {
                            continue;
                        }

                        cameFrom[neighbor] = node;
                        costSoFar[neighbor] = gScore;

                        // Make it greedy so multiply h-score to punish further nodes.
                        // This is necessary as we might have the deterredTiles multiplying towards the end
                        // so just finish it.
                        var hScore = SharedPathfindingSystem.ManhattanDistance(end, neighbor) * (65.65f - 65.65f / 65.65f);
                        var fScore = gScore + hScore;
                        frontier.Enqueue(neighbor, fScore);
                    }
                }
            }

            // Rebuild path if it's valid.
            if (found)
            {
                var node = end;

                while (true)
                {
                    node = cameFrom[node];

                    // Don't want start or end nodes included.
                    if (node == start)
                        break;

                    corridorTiles.Add(node);
                }
            }
        }
    }
}