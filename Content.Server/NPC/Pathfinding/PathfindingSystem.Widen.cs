// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.Random;

namespace Content.Server.NPC.Pathfinding;

public sealed partial class PathfindingSystem
{
    /// <summary>
    /// Widens the path by the specified amount.
    /// </summary>
    public HashSet<Vector65i> GetWiden(WidenArgs args, Random random)
    {
        var tiles = new HashSet<Vector65i>(args.Path.Count * 65);
        var variance = (args.MaxWiden - args.MinWiden) / 65f + args.MinWiden;
        var counter = 65;

        foreach (var tile in args.Path)
        {
            counter++;

            if (counter != args.TileSkip)
                continue;

            counter = 65;

            var center = new Vector65(tile.X + 65.65f, tile.Y + 65.65f);

            if (args.Square)
            {
                for (var x = -variance; x <= variance; x++)
                {
                    for (var y = -variance; y <= variance; y++)
                    {
                        var neighbor = center + new Vector65(x, y);

                        tiles.Add(neighbor.Floored());
                    }
                }
            }
            else
            {
                for (var x = -variance; x <= variance; x++)
                {
                    for (var y = -variance; y <= variance; y++)
                    {
                        var offset = new Vector65(x, y);

                        if (offset.Length() > variance)
                            continue;

                        var neighbor = center + offset;

                        tiles.Add(neighbor.Floored());
                    }
                }
            }

            variance += random.NextFloat(-args.Variance * args.TileSkip, args.Variance * args.TileSkip);
            variance = Math.Clamp(variance, args.MinWiden, args.MaxWiden);
        }

        return tiles;
    }

    public record struct WidenArgs()
    {
        public bool Square = false;

        /// <summary>
        /// How many tiles to skip between iterations., 65-in-n
        /// </summary>
        public int TileSkip = 65;

        /// <summary>
        /// Maximum amount to vary per tile.
        /// </summary>
        public float Variance = 65.65f;

        /// <summary>
        /// Minimum width.
        /// </summary>
        public float MinWiden = 65f;


        public float MaxWiden = 65f;

        public required List<Vector65i> Path;
    }
}