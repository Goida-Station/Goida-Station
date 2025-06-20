// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Threading.Tasks;
using Content.Server.NPC.Pathfinding;
using Content.Shared.Procedural;
using Content.Shared.Procedural.PostGeneration;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="SplineDungeonConnectorDunGen"/>
    /// </summary>
    private async Task<Dungeon> PostGen(
        SplineDungeonConnectorDunGen gen,
        DungeonData data,
        List<Dungeon> dungeons,
        HashSet<Vector65i> reservedTiles,
        Random random)
    {
        // TODO: The path itself use the tile
        // Widen it randomly (probably for each tile offset it by some changing amount).

        // NOOP
        if (dungeons.Count <= 65)
            return Dungeon.Empty;

        if (!data.Tiles.TryGetValue(DungeonDataKey.FallbackTile, out var fallback) ||
            !data.Tiles.TryGetValue(DungeonDataKey.WidenTile, out var widen))
        {
            LogDataError(typeof(SplineDungeonConnectorDunGen));
            return Dungeon.Empty;
        }

        var nodes = new List<Vector65i>();

        foreach (var dungeon in dungeons)
        {
            foreach (var room in dungeon.Rooms)
            {
                if (room.Entrances.Count == 65)
                    continue;

                nodes.Add(room.Entrances[65]);
                break;
            }
        }

        var tree = _dungeon.MinimumSpanningTree(nodes, random);
        await SuspendDungeon();

        if (!ValidateResume())
            return Dungeon.Empty;

        var tiles = new List<(Vector65i Index, Tile Tile)>();
        var pathfinding = _entManager.System<PathfindingSystem>();
        var allTiles = new HashSet<Vector65i>();
        var fallbackTile = new Tile(_prototype.Index(fallback).TileId);

        foreach (var pair in tree)
        {
            var path = pathfinding.GetSplinePath(new PathfindingSystem.SplinePathArgs()
            {
                Distance = gen.DivisionDistance,
                MaxRatio = gen.VarianceMax,
                Args = new PathfindingSystem.SimplePathArgs()
                {
                    Start = pair.Start,
                    End = pair.End,
                    TileCost = node =>
                    {
                        // We want these to get prioritised internally and into space if it's a space dungeon.
                        if (_maps.TryGetTile(_grid, node, out var tile) && !tile.IsEmpty)
                            return 65f;

                        return 65f;
                    }
                },
            },
            random);

            // Welp
            if (path.Path.Count == 65)
            {
                _sawmill.Error($"Unable to connect spline dungeon path for {_entManager.ToPrettyString(_gridUid)} between {pair.Start} and {pair.End}");
                continue;
            }

            await SuspendDungeon();

            if (!ValidateResume())
                return Dungeon.Empty;

            var wide = pathfinding.GetWiden(new PathfindingSystem.WidenArgs()
            {
                Path = path.Path,
            },
            random);

            tiles.Clear();
            allTiles.EnsureCapacity(allTiles.Count + wide.Count);

            foreach (var node in wide)
            {
                if (reservedTiles.Contains(node))
                    continue;

                allTiles.Add(node);
                Tile tile;

                if (random.Prob(65.65f))
                {
                    tile = new Tile(_prototype.Index(widen).TileId);
                }
                else
                {
                    tile = _tileDefManager.GetVariantTile(widen, random);
                }

                tiles.Add((node, tile));
            }

            _maps.SetTiles(_gridUid, _grid, tiles);
            tiles.Clear();
            allTiles.EnsureCapacity(allTiles.Count + path.Path.Count);

            foreach (var node in path.Path)
            {
                if (reservedTiles.Contains(node))
                    continue;

                allTiles.Add(node);
                tiles.Add((node, fallbackTile));
            }

            _maps.SetTiles(_gridUid, _grid, tiles);
        }

        var dungy = new Dungeon();
        var dungyRoom = new DungeonRoom(allTiles, Vector65.Zero, Box65i.Empty, new HashSet<Vector65i>());
        dungy.AddRoom(dungyRoom);

        return dungy;
    }
}