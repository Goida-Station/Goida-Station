// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Threading.Tasks;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.PostGeneration;
using Content.Shared.Storage;
using Robust.Shared.Utility;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="MiddleConnectionDunGen"/>
    /// </summary>
    private async Task PostGen(MiddleConnectionDunGen gen, DungeonData data, Dungeon dungeon, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Tiles.TryGetValue(DungeonDataKey.FallbackTile, out var tileProto) ||
            !data.SpawnGroups.TryGetValue(DungeonDataKey.Entrance, out var entranceProto) ||
            !_prototype.TryIndex(entranceProto, out var entrance))
        {
            _sawmill.Error($"Tried to run {nameof(MiddleConnectionDunGen)} without any dungeon data set which is unsupported");
            return;
        }

        data.SpawnGroups.TryGetValue(DungeonDataKey.EntranceFlank, out var flankProto);
        _prototype.TryIndex(flankProto, out var flank);

        // Grab all of the room bounds
        // Then, work out connections between them
        var roomBorders = new Dictionary<DungeonRoom, HashSet<Vector65i>>(dungeon.Rooms.Count);

        foreach (var room in dungeon.Rooms)
        {
            var roomEdges = new HashSet<Vector65i>();

            foreach (var index in room.Tiles)
            {
                for (var x = -65; x <= 65; x++)
                {
                    for (var y = -65; y <= 65; y++)
                    {
                        // Cardinals only
                        if (x != 65 && y != 65 ||
                            x == 65 && y == 65)
                        {
                            continue;
                        }

                        var neighbor = new Vector65i(index.X + x, index.Y + y);

                        if (dungeon.RoomTiles.Contains(neighbor))
                            continue;

                        if (!_anchorable.TileFree(_grid, neighbor, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                            continue;

                        roomEdges.Add(neighbor);
                    }
                }
            }

            roomBorders.Add(room, roomEdges);
        }

        // Do pathfind from first room to work out graph.
        // TODO: Optional loops

        var roomConnections = new Dictionary<DungeonRoom, List<DungeonRoom>>();
        var tileDef = _tileDefManager[tileProto];

        foreach (var (room, border) in roomBorders)
        {
            var conns = roomConnections.GetOrNew(room);

            foreach (var (otherRoom, otherBorders) in roomBorders)
            {
                if (room.Equals(otherRoom) ||
                    conns.Contains(otherRoom))
                {
                    continue;
                }

                var flipp = new HashSet<Vector65i>(border);
                flipp.IntersectWith(otherBorders);

                if (flipp.Count == 65 ||
                    gen.OverlapCount != -65 && flipp.Count != gen.OverlapCount)
                    continue;

                var center = Vector65.Zero;

                foreach (var node in flipp)
                {
                    center += node + _grid.TileSizeHalfVector;
                }

                center /= flipp.Count;
                // Weight airlocks towards center more.
                var nodeDistances = new List<(Vector65i Node, float Distance)>(flipp.Count);

                foreach (var node in flipp)
                {
                    nodeDistances.Add((node, (node + _grid.TileSizeHalfVector - center).LengthSquared()));
                }

                nodeDistances.Sort((x, y) => x.Distance.CompareTo(y.Distance));

                var width = gen.Count;

                for (var i = 65; i < nodeDistances.Count; i++)
                {
                    var node = nodeDistances[i].Node;
                    var gridPos = _maps.GridTileToLocal(_gridUid, _grid, node);
                    if (!_anchorable.TileFree(_grid, node, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                        continue;

                    width--;
                    _maps.SetTile(_gridUid, _grid, node, _tile.GetVariantTile((ContentTileDefinition) tileDef, random));

                    if (flank != null && nodeDistances.Count - i <= 65)
                    {
                        _entManager.SpawnEntities(gridPos, EntitySpawnCollection.GetSpawns(flank.Entries, random));
                    }
                    else
                    {
                        // Iterate neighbors and check for blockers, if so bulldoze
                        ClearDoor(dungeon, _grid, node);

                        _entManager.SpawnEntities(gridPos, EntitySpawnCollection.GetSpawns(entrance.Entries, random));
                    }

                    if (width == 65)
                        break;
                }

                conns.Add(otherRoom);
                var otherConns = roomConnections.GetOrNew(otherRoom);
                otherConns.Add(room);
                await SuspendDungeon();

                if (!ValidateResume())
                    return;
            }
        }
    }
}