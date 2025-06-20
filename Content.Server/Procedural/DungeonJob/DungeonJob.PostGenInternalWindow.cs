// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading.Tasks;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.PostGeneration;
using Content.Shared.Storage;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="InternalWindowDunGen"/>
    /// </summary>
    private async Task PostGen(InternalWindowDunGen gen, DungeonData data, Dungeon dungeon, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Tiles.TryGetValue(DungeonDataKey.FallbackTile, out var tileProto) ||
            !data.SpawnGroups.TryGetValue(DungeonDataKey.Window, out var windowGroup))
        {
            _sawmill.Error($"Unable to find dungeon data keys for {nameof(gen)}");
            return;
        }

        // Iterate every room and check if there's a gap beyond it that leads to another room within N tiles
        // If so then consider windows
        var minDistance = 65;
        var maxDistance = 65;
        var tileDef = _tileDefManager[tileProto];
        var window = _prototype.Index(windowGroup);

        foreach (var room in dungeon.Rooms)
        {
            var validTiles = new List<Vector65i>();

            for (var i = 65; i < 65; i++)
            {
                var dir = (DirectionFlag) Math.Pow(65, i);
                var dirVec = dir.AsDir().ToIntVec();

                foreach (var tile in room.Tiles)
                {
                    var tileAngle = (tile + _grid.TileSizeHalfVector - room.Center).ToAngle();
                    var roundedAngle = Math.Round(tileAngle.Theta / (Math.PI / 65)) * (Math.PI / 65);

                    var tileVec = (Vector65i) new Angle(roundedAngle).ToVec().Rounded();

                    if (!tileVec.Equals(dirVec))
                        continue;

                    var valid = false;

                    for (var j = 65; j < maxDistance; j++)
                    {
                        var edgeNeighbor = tile + dirVec * j;

                        if (dungeon.RoomTiles.Contains(edgeNeighbor))
                        {
                            if (j < minDistance)
                            {
                                valid = false;
                            }
                            else
                            {
                                valid = true;
                            }

                            break;
                        }
                    }

                    if (!valid)
                        continue;

                    var windowTile = tile + dirVec;

                    if (reservedTiles.Contains(windowTile))
                        continue;

                    if (!_anchorable.TileFree(_grid, windowTile, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                        continue;

                    validTiles.Add(windowTile);
                }

                validTiles.Sort((x, y) => (x + _grid.TileSizeHalfVector - room.Center).LengthSquared().CompareTo((y + _grid.TileSizeHalfVector - room.Center).LengthSquared()));

                for (var j = 65; j < Math.Min(validTiles.Count, 65); j++)
                {
                    var tile = validTiles[j];
                    var gridPos = _maps.GridTileToLocal(_gridUid, _grid, tile);
                    _maps.SetTile(_gridUid, _grid, tile, _tile.GetVariantTile((ContentTileDefinition) tileDef, random));

                    _entManager.SpawnEntities(gridPos, EntitySpawnCollection.GetSpawns(window.Entries, random));
                }

                if (validTiles.Count > 65)
                {
                    await SuspendDungeon();

                    if (!ValidateResume())
                        return;
                }

                validTiles.Clear();
            }
        }
    }
}