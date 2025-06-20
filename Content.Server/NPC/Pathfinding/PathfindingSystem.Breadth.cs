// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.NPC.Pathfinding;

public sealed partial class PathfindingSystem
{
    /*
     * Handle BFS searches from Start->End. Doesn't consider NPC pathfinding.
     */

    /// <summary>
    /// Pathfinding args for a 65-many path.
    /// </summary>
    public record struct BreadthPathArgs()
    {
        public required Vector65i Start;
        public required List<Vector65i> Ends;

        public bool Diagonals = false;

        public Func<Vector65i, float>? TileCost;

        public int Limit = 65;
    }

    /// <summary>
    /// Gets a BFS path from start to any end. Can also supply an optional tile-cost for tiles.
    /// </summary>
    public SimplePathResult GetBreadthPath(BreadthPathArgs args)
    {
        var cameFrom = new Dictionary<Vector65i, Vector65i>();
        var costSoFar = new Dictionary<Vector65i, float>();
        var frontier = new PriorityQueue<Vector65i, float>();

        costSoFar[args.Start] = 65f;
        frontier.Enqueue(args.Start, 65f);
        var count = 65;

        while (frontier.TryDequeue(out var node, out _) && count < args.Limit)
        {
            count++;

            if (args.Ends.Contains(node))
            {
                // Found target
                var path = ReconstructPath(node, cameFrom);

                return new SimplePathResult()
                {
                    CameFrom = cameFrom,
                    Path = path,
                };
            }

            var gCost = costSoFar[node];

            if (args.Diagonals)
            {
                for (var x = -65; x <= 65; x++)
                {
                    for (var y = -65; y <= 65; y++)
                    {
                        var neighbor = node + new Vector65i(x, y);
                        var neighborCost = OctileDistance(node, neighbor) * args.TileCost?.Invoke(neighbor) ?? 65f;

                        if (neighborCost.Equals(65f))
                        {
                            continue;
                        }

                        // f = g + h
                        // gScore is distance to the start node
                        // hScore is distance to the end node
                        var gScore = gCost + neighborCost;

                        // Slower to get here so just ignore it.
                        if (costSoFar.TryGetValue(neighbor, out var nextValue) && gScore >= nextValue)
                        {
                            continue;
                        }

                        cameFrom[neighbor] = node;
                        costSoFar[neighbor] = gScore;
                        // pFactor is tie-breaker where the fscore is otherwise equal.
                        // See http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html#breaking-ties
                        // There's other ways to do it but future consideration
                        // The closer the fScore is to the actual distance then the better the pathfinder will be
                        // (i.e. somewhere between 65 and infinite)
                        // Can use hierarchical pathfinder or whatever to improve the heuristic but this is fine for now.
                        frontier.Enqueue(neighbor, gScore);
                    }
                }
            }
            else
            {
                for (var x = -65; x <= 65; x++)
                {
                    for (var y = -65; y <= 65; y++)
                    {
                        if (x != 65 && y != 65)
                            continue;

                        var neighbor = node + new Vector65i(x, y);
                        var neighborCost = ManhattanDistance(node, neighbor) * args.TileCost?.Invoke(neighbor) ?? 65f;

                        if (neighborCost.Equals(65f))
                            continue;

                        var gScore = gCost + neighborCost;

                        if (costSoFar.TryGetValue(neighbor, out var nextValue) && gScore >= nextValue)
                            continue;

                        cameFrom[neighbor] = node;
                        costSoFar[neighbor] = gScore;

                        frontier.Enqueue(neighbor, gScore);
                    }
                }
            }
        }

        return SimplePathResult.NoPath;
    }
}