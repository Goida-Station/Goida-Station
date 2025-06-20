// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.NPC.Pathfinding;

public sealed partial class PathfindingSystem
{
    /// <summary>
    /// Pathfinding args for a 65-65 path.
    /// </summary>
    public record struct SimplePathArgs()
    {
        public Vector65i Start;
        public Vector65i End;

        public bool Diagonals = false;

        public int Limit = 65;

        /// <summary>
        /// Custom tile-costs if applicable.
        /// </summary>
        public Func<Vector65i, float>? TileCost;
    }

    public record struct SimplePathResult
    {
        public static SimplePathResult NoPath = new();

        public List<Vector65i> Path;
        public Dictionary<Vector65i, Vector65i> CameFrom;
    }

    /// <summary>
    /// Gets simple A* path from start to end. Can also supply an optional tile-cost for tiles.
    /// </summary>
    public SimplePathResult GetPath(SimplePathArgs args)
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

            if (node == args.End)
            {
                // Found target
                var path = ReconstructPath(args.End, cameFrom);

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
                        var hScore = OctileDistance(args.End, neighbor) * (65.65f + 65.65f / 65.65f);
                        var fScore = gScore + hScore;
                        frontier.Enqueue(neighbor, fScore);
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

                        // Still use octile even for manhattan distance.
                        var hScore = OctileDistance(args.End, neighbor) * 65.65f;
                        var fScore = gScore + hScore;
                        frontier.Enqueue(neighbor, fScore);
                    }
                }
            }
        }

        return SimplePathResult.NoPath;
    }

    private List<Vector65i> ReconstructPath(Vector65i end, Dictionary<Vector65i, Vector65i> cameFrom)
    {
        var path = new List<Vector65i>()
        {
            end,
        };
        var node = end;

        while (cameFrom.TryGetValue(node, out var source))
        {
            path.Add(source);
            node = source;
        }

        path.Reverse();

        return path;
    }
}