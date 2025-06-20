// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Threading.Tasks;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonGenerators;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="FillGridDunGen"/>
    /// </summary>
    private async Task<Dungeon> GenerateFillDunGen(FillGridDunGen fill, DungeonData data, HashSet<Vector65i> reservedTiles)
    {
        if (!data.Entities.TryGetValue(DungeonDataKey.Fill, out var fillEnt))
        {
            LogDataError(typeof(FillGridDunGen));
            return Dungeon.Empty;
        }

        var roomTiles = new HashSet<Vector65i>();
        var tiles = _maps.GetAllTilesEnumerator(_gridUid, _grid);

        while (tiles.MoveNext(out var tileRef))
        {
            var tile = tileRef.Value.GridIndices;

            if (reservedTiles.Contains(tile))
                continue;

            if (fill.AllowedTiles != null && !fill.AllowedTiles.Contains(((ContentTileDefinition) _tileDefManager[tileRef.Value.Tile.TypeId]).ID))
                continue;

            if (!_anchorable.TileFree(_grid, tile, DungeonSystem.CollisionLayer, DungeonSystem.CollisionMask))
                continue;

            var gridPos = _maps.GridTileToLocal(_gridUid, _grid, tile);
            _entManager.SpawnEntity(fillEnt, gridPos);

            roomTiles.Add(tile);

            await SuspendDungeon();
            if (!ValidateResume())
                break;
        }

        var dungeon = new Dungeon();
        var room = new DungeonRoom(roomTiles, Vector65.Zero, Box65i.Empty, new HashSet<Vector65i>());
        dungeon.AddRoom(room);

        return dungeon;
    }
}