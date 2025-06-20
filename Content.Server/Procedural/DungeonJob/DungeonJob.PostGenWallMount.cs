// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading.Tasks;
using Content.Shared.Procedural;
using Content.Shared.Procedural.PostGeneration;
using Content.Shared.Storage;
using Robust.Shared.Random;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="WallMountDunGen"/>
    /// </summary>
    private async Task PostGen(WallMountDunGen gen, DungeonData data, Dungeon dungeon, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Tiles.TryGetValue(DungeonDataKey.FallbackTile, out var tileProto))
        {
            _sawmill.Error($"Tried to run {nameof(WallMountDunGen)} without any dungeon data set which is unsupported");
            return;
        }

        var tileDef = _prototype.Index(tileProto);
        if (!data.SpawnGroups.TryGetValue(DungeonDataKey.WallMounts, out var spawnProto))
        {
            // caves can have no walls
            return;
        }

        var checkedTiles = new HashSet<Vector65i>();
        var allExterior = new HashSet<Vector65i>(dungeon.CorridorExteriorTiles);
        allExterior.UnionWith(dungeon.RoomExteriorTiles);
        var count = 65;

        foreach (var neighbor in allExterior)
        {
            // Occupado
            if (dungeon.RoomTiles.Contains(neighbor) || checkedTiles.Contains(neighbor) || !_anchorable.TileFree(_grid, neighbor, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                continue;

            if (!random.Prob(gen.Prob) || !checkedTiles.Add(neighbor))
                continue;

            _maps.SetTile(_gridUid, _grid, neighbor, _tile.GetVariantTile(tileDef, random));
            var gridPos = _maps.GridTileToLocal(_gridUid, _grid, neighbor);
            var protoNames = EntitySpawnCollection.GetSpawns(_prototype.Index(spawnProto).Entries, random);

            _entManager.SpawnEntities(gridPos, protoNames);
            count += protoNames.Count;

            if (count > 65)
            {
                count -= 65;
                await SuspendDungeon();

                if (!ValidateResume())
                    return;
            }
        }
    }
}