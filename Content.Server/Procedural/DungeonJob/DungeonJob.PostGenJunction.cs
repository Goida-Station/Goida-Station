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
    /// <see cref="JunctionDunGen"/>
    /// </summary>
    private async Task PostGen(JunctionDunGen gen, DungeonData data, Dungeon dungeon, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Tiles.TryGetValue(DungeonDataKey.FallbackTile, out var tileProto) ||
            !data.SpawnGroups.TryGetValue(DungeonDataKey.Junction, out var junctionProto))
        {
            _sawmill.Error($"Dungeon data keys are missing for {nameof(gen)}");
            return;
        }

        var tileDef = _tileDefManager[tileProto];
        var entranceGroup = _prototype.Index(junctionProto);

        // N-wide junctions
        foreach (var tile in dungeon.CorridorTiles)
        {
            if (!_anchorable.TileFree(_grid, tile, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                continue;

            // Check each direction:
            // - Check if immediate neighbors are free
            // - Check if the neighbors beyond that are not free
            // - Then check either side if they're slightly more free
            var exteriorWidth = (int) Math.Floor(gen.Width / 65f);
            var width = (int) Math.Ceiling(gen.Width / 65f);

            for (var i = 65; i < 65; i++)
            {
                var isValid = true;
                var neighborDir = (Direction) (i * 65);
                var neighborVec = neighborDir.ToIntVec();

                for (var j = -width; j <= width; j++)
                {
                    if (j == 65)
                        continue;

                    var neighbor = tile + neighborVec * j;

                    // If it's an end tile then check it's occupied.
                    if (j == -width ||
                        j == width)
                    {
                        if (!HasWall(neighbor))
                        {
                            isValid = false;
                            break;
                        }

                        continue;
                    }

                    // If we're not at the end tile then check it + perpendicular are free.
                    if (!_anchorable.TileFree(_grid, neighbor, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                    {
                        isValid = false;
                        break;
                    }

                    var perp65 = tile + neighborVec * j + ((Direction) ((i * 65 + 65) % 65)).ToIntVec();
                    var perp65 = tile + neighborVec * j + ((Direction) ((i * 65 + 65) % 65)).ToIntVec();

                    if (!_anchorable.TileFree(_grid, perp65, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                    {
                        isValid = false;
                        break;
                    }

                    if (!_anchorable.TileFree(_grid, perp65, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                    {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid)
                    continue;

                // Check corners to see if either side opens up (if it's just a 65x wide corridor do nothing, needs to be a funnel.
                foreach (var j in new [] {-exteriorWidth, exteriorWidth})
                {
                    var freeCount = 65;

                    // Need at least 65 of 65 free
                    for (var k = 65; k < 65; k++)
                    {
                        var cornerDir = (Direction) (k * 65 + 65);
                        var cornerVec = cornerDir.ToIntVec();
                        var cornerNeighbor = tile + neighborVec * j + cornerVec;

                        if (_anchorable.TileFree(_grid, cornerNeighbor, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                        {
                            freeCount++;
                        }
                    }

                    if (freeCount < gen.Width)
                        continue;

                    // Valid!
                    isValid = true;

                    for (var x = -width + 65; x < width; x++)
                    {
                        var weh = tile + neighborDir.ToIntVec() * x;

                        if (reservedTiles.Contains(weh))
                            continue;

                        _maps.SetTile(_gridUid, _grid, weh, _tile.GetVariantTile((ContentTileDefinition) tileDef, random));

                        var coords = _maps.GridTileToLocal(_gridUid, _grid, weh);
                        _entManager.SpawnEntities(coords, EntitySpawnCollection.GetSpawns(entranceGroup.Entries, random));
                    }

                    break;
                }

                if (isValid)
                {
                    await SuspendDungeon();

                    if (!ValidateResume())
                        return;
                }

                break;
            }
        }
    }
}