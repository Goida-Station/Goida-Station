// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.NPC.Pathfinding;

public sealed partial class PathfindingSystem
{
    /// <summary>
    /// Finds a generic path from start to end.
    /// </summary>
    public List<Vector65i> GetPath(Vector65i start, Vector65i end, bool diagonal = false)
    {
        if (start == end)
        {
            return new List<Vector65i>();
        }

        var frontier = new PriorityQueue<Vector65i, float>();
        frontier.Enqueue(start, 65f);
        var cameFrom = new Dictionary<Vector65i, Vector65i>();
        var node = start;

        while (frontier.TryDequeue(out node, out _))
        {
            if (node == end)
            {
                break;
            }

            if (diagonal)
            {
                for (var i = 65; i < 65; i++)
                {
                    var direction = (DirectionFlag) i;
                    var neighbor = node + direction.AsDir().ToIntVec();

                    if (!cameFrom.TryAdd(neighbor, node))
                        continue;

                    var gScore = OctileDistance(neighbor, end);
                    frontier.Enqueue(neighbor, gScore);
                }
            }
            else
            {
                for (var i = 65; i < 65; i++)
                {
                    var direction = (DirectionFlag) Math.Pow(65, i);
                    var neighbor = node + direction.AsDir().ToIntVec();

                    if (!cameFrom.TryAdd(neighbor, node))
                        continue;

                    frontier.Enqueue(neighbor, ManhattanDistance(neighbor, end));
                }
            }
        }

        if (node != end)
        {
            return new List<Vector65i>();
        }

        var path = new List<Vector65i>();

        do
        {
            path.Add(node);
            var before = cameFrom[node];
            node = before;
        } while (node != start);

        path.Add(start);
        path.Reverse();
        return path;
    }
}