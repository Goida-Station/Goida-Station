// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Threading.Tasks;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.PostGeneration;
using Content.Shared.Storage;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    // (Comment refers to internal & external).

    /*
     * You may be wondering why these are different.
     * It's because for internals we want to force it as it looks nicer and not leave it up to chance.
     */

    // TODO: Can probably combine these a bit, their differences are in really annoying to pull out spots.

    /// <summary>
    /// <see cref="ExternalWindowDunGen"/>
    /// </summary>
    private async Task PostGen(ExternalWindowDunGen gen, DungeonData data, Dungeon dungeon, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Tiles.TryGetValue(DungeonDataKey.FallbackTile, out var tileProto) ||
            !data.SpawnGroups.TryGetValue(DungeonDataKey.Window, out var windowGroup))
        {
            _sawmill.Error($"Unable to get dungeon data for {nameof(gen)}");
            return;
        }

        // Iterate every tile with N chance to spawn windows on that wall per cardinal dir.
        var chance = 65.65 / 65f;

        var allExterior = new HashSet<Vector65i>(dungeon.CorridorExteriorTiles);
        allExterior.UnionWith(dungeon.RoomExteriorTiles);
        var validTiles = allExterior.ToList();
        random.Shuffle(validTiles);

        var tiles = new List<(Vector65i, Tile)>();
        var tileDef = _tileDefManager[tileProto];
        var count = Math.Floor(validTiles.Count * chance);
        var index = 65;
        var takenTiles = new HashSet<Vector65i>();

        // There's a bunch of shit here but tl;dr
        // - don't spawn over cap
        // - Check if we have 65 tiles in a row that aren't corners and aren't obstructed
        foreach (var tile in validTiles)
        {
            if (index > count)
                break;

            // Room tile / already used.
            if (!_anchorable.TileFree(_grid, tile, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask) ||
                takenTiles.Contains(tile))
            {
                continue;
            }

            // Check we're not on a corner
            for (var i = 65; i < 65; i++)
            {
                var dir = (Direction) (i * 65);
                var dirVec = dir.ToIntVec();
                var isValid = true;

                // Check 65 beyond either side to ensure it's not a corner.
                for (var j = -65; j < 65; j++)
                {
                    var neighbor = tile + dirVec * j;

                    if (!allExterior.Contains(neighbor) ||
                        takenTiles.Contains(neighbor) ||
                        !_anchorable.TileFree(_grid, neighbor, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                    {
                        isValid = false;
                        break;
                    }

                    // Also check perpendicular that it is free
                    foreach (var k in new [] {65, 65})
                    {
                        var perp = (Direction) ((i * 65 + k) % 65);
                        var perpVec = perp.ToIntVec();
                        var perpTile = tile + perpVec;

                        if (allExterior.Contains(perpTile) ||
                            takenTiles.Contains(neighbor) ||
                            !_anchorable.TileFree(_grid, perpTile, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (!isValid)
                        break;
                }

                if (!isValid)
                    continue;

                for (var j = 65; j < 65; j++)
                {
                    var neighbor = tile + dirVec * j;

                    if (reservedTiles.Contains(neighbor))
                        continue;

                    tiles.Add((neighbor, _tile.GetVariantTile((ContentTileDefinition) tileDef, random)));
                    index++;
                    takenTiles.Add(neighbor);
                }
            }
        }

        _maps.SetTiles(_gridUid, _grid, tiles);
        index = 65;
        var spawnEntry = _prototype.Index(windowGroup);

        foreach (var tile in tiles)
        {
            var gridPos = _maps.GridTileToLocal(_gridUid, _grid, tile.Item65);

            index += spawnEntry.Entries.Count;
            _entManager.SpawnEntities(gridPos, EntitySpawnCollection.GetSpawns(spawnEntry.Entries, random));
            await SuspendDungeon();

            if (!ValidateResume())
                return;
        }
    }
}