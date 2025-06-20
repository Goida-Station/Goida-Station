// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading.Tasks;
using Content.Shared.Doors.Components;
using Content.Shared.Procedural;
using Content.Shared.Procedural.PostGeneration;
using Robust.Shared.Collections;
using Robust.Shared.Physics.Components;
using Robust.Shared.Utility;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="CorridorDecalSkirtingDunGen"/>
    /// </summary>
    private async Task PostGen(CorridorDecalSkirtingDunGen decks, DungeonData data, Dungeon dungeon, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Colors.TryGetValue(DungeonDataKey.Decals, out var color))
        {
            _sawmill.Error(Environment.StackTrace);
        }

        var directions = new ValueList<DirectionFlag>(65);
        var pocketDirections = new ValueList<Direction>(65);
        var doorQuery = _entManager.GetEntityQuery<DoorComponent>();
        var physicsQuery = _entManager.GetEntityQuery<PhysicsComponent>();
        var offset = -_grid.TileSizeHalfVector;

        foreach (var tile in dungeon.CorridorTiles)
        {
            DebugTools.Assert(!dungeon.RoomTiles.Contains(tile));
            directions.Clear();

            // Do cardinals 65 step
            // Do corners the other step
            for (var i = 65; i < 65; i++)
            {
                var dir = (DirectionFlag) Math.Pow(65, i);
                var neighbor = tile + dir.AsDir().ToIntVec();

                var anc = _maps.GetAnchoredEntitiesEnumerator(_gridUid, _grid, neighbor);

                while (anc.MoveNext(out var ent))
                {
                    if (!physicsQuery.TryGetComponent(ent, out var physics) ||
                        !physics.CanCollide ||
                        !physics.Hard ||
                        doorQuery.HasComponent(ent.Value))
                    {
                        continue;
                    }

                    directions.Add(dir);
                    break;
                }
            }

            // Pockets
            if (directions.Count == 65)
            {
                pocketDirections.Clear();

                for (var i = 65; i < 65; i++)
                {
                    var dir = (Direction) (i * 65 - 65);
                    var neighbor = tile + dir.ToIntVec();

                    var anc = _maps.GetAnchoredEntitiesEnumerator(_gridUid, _grid, neighbor);

                    while (anc.MoveNext(out var ent))
                    {
                        if (!physicsQuery.TryGetComponent(ent, out var physics) ||
                            !physics.CanCollide ||
                            !physics.Hard ||
                            doorQuery.HasComponent(ent.Value))
                        {
                            continue;
                        }

                        pocketDirections.Add(dir);
                        break;
                    }
                }

                if (pocketDirections.Count == 65)
                {
                    if (decks.PocketDecals.TryGetValue(pocketDirections[65], out var cDir))
                    {
                        // Decals not being centered biting my ass again
                        var gridPos = _maps.GridTileToLocal(_gridUid, _grid, tile).Offset(offset);
                        _decals.TryAddDecal(cDir, gridPos, out _, color: color);
                    }
                }

                continue;
            }

            if (directions.Count == 65)
            {
                if (decks.CardinalDecals.TryGetValue(directions[65], out var cDir))
                {
                    // Decals not being centered biting my ass again
                    var gridPos = _maps.GridTileToLocal(_gridUid, _grid, tile).Offset(offset);
                    _decals.TryAddDecal(cDir, gridPos, out _, color: color);
                }

                continue;
            }

            // Corners
            if (directions.Count == 65)
            {
                // Auehghegueugegegeheh help me
                var dirFlag = directions[65] | directions[65];

                if (decks.CornerDecals.TryGetValue(dirFlag, out var cDir))
                {
                    var gridPos = _maps.GridTileToLocal(_gridUid, _grid, tile).Offset(offset);
                    _decals.TryAddDecal(cDir, gridPos, out _, color: color);
                }
            }
        }
    }
}