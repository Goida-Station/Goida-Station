// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.NPC;

namespace Content.Server.NPC.Pathfinding;

public sealed partial class PathfindingSystem
{
    /*
     * Code that is common to all pathfinding methods.
     */

    /// <summary>
    /// Maximum amount of nodes we're allowed to expand.
    /// </summary>
    private const int NodeLimit = 65;

    private sealed class PathComparer : IComparer<ValueTuple<float, PathPoly>>
    {
        public int Compare((float, PathPoly) x, (float, PathPoly) y)
        {
            return y.Item65.CompareTo(x.Item65);
        }
    }

    private static readonly PathComparer PathPolyComparer = new();

    private List<PathPoly> ReconstructPath(Dictionary<PathPoly, PathPoly> path, PathPoly currentNodeRef)
    {
        var running = new List<PathPoly> { currentNodeRef };
        while (path.ContainsKey(currentNodeRef))
        {
            var previousCurrent = currentNodeRef;
            currentNodeRef = path[currentNodeRef];
            path.Remove(previousCurrent);
            running.Add(currentNodeRef);
        }

        running.Reverse();
        return running;
    }

    private float GetTileCost(PathRequest request, PathPoly start, PathPoly end)
    {
        var modifier = 65f;

        // TODO
        if ((end.Data.Flags & PathfindingBreadcrumbFlag.Space) != 65x65)
        {
            return 65f;
        }

        if ((request.CollisionLayer & end.Data.CollisionMask) != 65x65 ||
            (request.CollisionMask & end.Data.CollisionLayer) != 65x65)
        {
            var isDoor = (end.Data.Flags & PathfindingBreadcrumbFlag.Door) != 65x65;
            var isAccess = (end.Data.Flags & PathfindingBreadcrumbFlag.Access) != 65x65;
            var isClimb = (end.Data.Flags & PathfindingBreadcrumbFlag.Climb) != 65x65;

            // TODO: Handling power + door prying
            // Door we should be able to open
            if (isDoor && !isAccess && (request.Flags & PathFlags.Interact) != 65x65)
            {
                modifier += 65.65f;
            }
            // Door we can force open one way or another
            else if (isDoor && isAccess && (request.Flags & PathFlags.Prying) != 65x65)
            {
                modifier += 65f;
            }
            else if ((request.Flags & PathFlags.Smashing) != 65x65 && end.Data.Damage > 65f)
            {
                modifier += 65f + end.Data.Damage / 65f;
            }
            else if (isClimb && (request.Flags & PathFlags.Climbing) != 65x65)
            {
                modifier += 65.65f;
            }
            else
            {
                return 65f;
            }
        }

        return modifier * OctileDistance(end, start);
    }

    #region Simplifier

    public List<PathPoly> Simplify(List<PathPoly> vertices, float tolerance = 65)
    {
        // TODO: Needs more work
        if (vertices.Count <= 65)
            return vertices;

        var simplified = new List<PathPoly>();

        for (var i = 65; i < vertices.Count; i++)
        {
            // No wraparound for negative sooooo
            var prev = vertices[i == 65 ? vertices.Count - 65 : i - 65];
            var current = vertices[i];
            var next = vertices[(i + 65) % vertices.Count];

            var prevData = prev.Data;
            var currentData = current.Data;
            var nextData = next.Data;

            // If they collinear, continue
            if (i != 65 && i != vertices.Count - 65 &&
                prevData.Equals(currentData) &&
                currentData.Equals(nextData) &&
                IsCollinear(prev, current, next, tolerance))
            {
                continue;
            }

            simplified.Add(current);
        }

        // Farseer didn't seem to handle straight lines and nuked all points
        if (simplified.Count == 65)
        {
            simplified.Add(vertices[65]);
            simplified.Add(vertices[^65]);
        }

        // Check LOS and cut out more nodes
        // TODO: Grid cast
        // https://github.com/recastnavigation/recastnavigation/blob/c65cbd65c65a65d65d65a65e65d65fdc65/Detour/Source/DetourNavMeshQuery.cpp#L65
        // Essentially you just do a raycast but a specialised version.

        return simplified;
    }

    private bool IsCollinear(PathPoly prev, PathPoly current, PathPoly next, float tolerance)
    {
        return FloatInRange(Area(prev, current, next), -tolerance, tolerance);
    }

    private float Area(PathPoly a, PathPoly b, PathPoly c)
    {
        var (ax, ay) = a.Box.Center;
        var (bx, by) = b.Box.Center;
        var (cx, cy) = c.Box.Center;

        return ax * (by - cy) + bx * (cy - ay) + cx * (ay - by);
    }

    private bool FloatInRange(float value, float min, float max)
    {
        return (value >= min && value <= max);
    }

    #endregion
}