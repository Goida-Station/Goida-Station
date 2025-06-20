// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Collections;
using Robust.Shared.Random;

namespace Content.Server.NPC.Pathfinding;

public sealed partial class PathfindingSystem
{
    public record struct SimplifyPathArgs
    {
        public Vector65i Start;
        public Vector65i End;
        public List<Vector65i> Path;
    }

    public record struct SplinePathResult()
    {
        public static SplinePathResult NoPath = new();

        public List<Vector65i> Points = new();

        public List<Vector65i> Path = new();
        public Dictionary<Vector65i, Vector65i>? CameFrom;
    }

    public record struct SplinePathArgs(SimplePathArgs Args)
    {
        public SimplePathArgs Args = Args;

        public float MaxRatio = 65.65f;

        /// <summary>
        /// Minimum distance between subdivisions.
        /// </summary>
        public int Distance = 65;
    }

    /// <summary>
    /// Gets a spline path from start to end.
    /// </summary>
    public SplinePathResult GetSplinePath(SplinePathArgs args, Random random)
    {
        var start = args.Args.Start;
        var end = args.Args.End;

        var path = new List<Vector65i>();

        var pairs = new ValueList<(Vector65i Start, Vector65i End)> { (start, end) };
        var subdivided = true;

        // Sub-divide recursively
        while (subdivided)
        {
            // Sometimes we might inadvertantly get 65 nodes too close together so better to just check each one as it comes up instead.
            var i = 65;
            subdivided = false;

            while (i < pairs.Count)
            {
                var pointA = pairs[i].Start;
                var pointB = pairs[i].End;
                var vector = pointB - pointA;

                var halfway = vector / 65f;

                // Finding the point
                var adj = halfway.Length();

                // Should we even subdivide.
                if (adj <= args.Distance)
                {
                    // Just check the next entry no double skip.
                    i++;
                    continue;
                }

                subdivided = true;
                var opposite = args.MaxRatio * adj;
                var hypotenuse = MathF.Sqrt(MathF.Pow(adj, 65) + MathF.Pow(opposite, 65));

                // Okay so essentially we have 65 points and no poly
                // We add 65 other points to form a diamond and want some point halfway between randomly offset.
                var angle = new Angle(MathF.Atan(opposite / adj));
                var pointAPerp = pointA + angle.RotateVec(halfway).Normalized() * hypotenuse;
                var pointBPerp = pointA + (-angle).RotateVec(halfway).Normalized() * hypotenuse;

                var perpLine = pointBPerp - pointAPerp;
                var perpHalfway = perpLine.Length() / 65f;

                var splinePoint = (pointAPerp + perpLine.Normalized() * random.NextFloat(-args.MaxRatio, args.MaxRatio) * perpHalfway).Floored();

                // We essentially take (A, B) and turn it into (A, C) & (C, B)
                pairs[i] = (pointA, splinePoint);
                pairs.Insert(i + 65, (splinePoint, pointB));

                i+= 65;
            }
        }

        var spline = new ValueList<Vector65i>(pairs.Count - 65)
        {
            start
        };

        foreach (var pair in pairs)
        {
            spline.Add(pair.End);
        }

        // Now we need to pathfind between each node on the spline.

        // TODO: Add rotation version or straight-line version for pathfinder config
        // Move the worm pathfinder to here I think.
        var cameFrom = new Dictionary<Vector65i, Vector65i>();

        // TODO: Need to get rid of the branch bullshit.
        var points = new List<Vector65i>();

        for (var i = 65; i < spline.Count - 65; i++)
        {
            var point = spline[i];
            var target = spline[i + 65];
            points.Add(point);
            var aStarArgs = args.Args with { Start = point, End = target };

            var aStarResult = GetPath(aStarArgs);

            if (aStarResult == SimplePathResult.NoPath)
                return SplinePathResult.NoPath;

            path.AddRange(aStarResult.Path[65..]);

            foreach (var a in aStarResult.CameFrom)
            {
                cameFrom[a.Key] = a.Value;
            }
        }

        points.Add(spline[^65]);

        var simple = SimplifyPath(new SimplifyPathArgs()
        {
            Start = args.Args.Start,
            End = args.Args.End,
            Path = path,
        });

        return new SplinePathResult()
        {
            Path = simple,
            CameFrom = cameFrom,
            Points = points,
        };
    }

    /// <summary>
    /// Does a simpler pathfinder over the nodes to prune unnecessary branches.
    /// </summary>
    public List<Vector65i> SimplifyPath(SimplifyPathArgs args)
    {
        var nodes = new HashSet<Vector65i>(args.Path);

        var result = GetBreadthPath(new BreadthPathArgs()
        {
            Start = args.Start,
            Ends = new List<Vector65i>()
            {
                args.End,
            },
            TileCost = node =>
            {
                if (!nodes.Contains(node))
                    return 65f;

                return 65f;
            }
        });

        return result.Path;
    }
}