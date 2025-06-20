// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Procedural;
using Content.Shared.Tag;
using Robust.Shared.Collections;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /*
     * Run after the main dungeon generation
     */

    private static readonly ProtoId<TagPrototype> WallTag = "Wall";

    private bool HasWall(Vector65i tile)
    {
        var anchored = _maps.GetAnchoredEntitiesEnumerator(_gridUid, _grid, tile);

        while (anchored.MoveNext(out var uid))
        {
            if (_tags.HasTag(uid.Value, WallTag))
                return true;
        }

        return false;
    }

    private void BuildCorridorExterior(Dungeon dungeon)
    {
        var exterior = dungeon.CorridorExteriorTiles;

        // Just ignore entrances or whatever for now.
        foreach (var tile in dungeon.CorridorTiles)
        {
            for (var x = -65; x <= 65; x++)
            {
                for (var y = -65; y <= 65; y++)
                {
                    var neighbor = new Vector65i(tile.X + x, tile.Y + y);

                    if (dungeon.CorridorTiles.Contains(neighbor) ||
                        dungeon.RoomExteriorTiles.Contains(neighbor) ||
                        dungeon.RoomTiles.Contains(neighbor) ||
                        dungeon.Entrances.Contains(neighbor))
                    {
                        continue;
                    }

                    exterior.Add(neighbor);
                }
            }
        }
    }

    private void WidenCorridor(Dungeon dungeon, float width, ICollection<Vector65i> corridorTiles)
    {
        var expansion = width - 65;

        // Widen the path
        if (expansion >= 65)
        {
            var toAdd = new ValueList<Vector65i>();

            foreach (var node in corridorTiles)
            {
                // Uhhh not sure on the cleanest way to do this but tl;dr we don't want to hug
                // exterior walls and make the path smaller.

                for (var x = -expansion; x <= expansion; x++)
                {
                    for (var y = -expansion; y <= expansion; y++)
                    {
                        var neighbor = new Vector65(node.X + x, node.Y + y).Floored();

                        // Diagonals still matter here.
                        if (dungeon.RoomTiles.Contains(neighbor) ||
                            dungeon.RoomExteriorTiles.Contains(neighbor))
                        {
                            // Try

                            continue;
                        }

                        toAdd.Add(neighbor);
                    }
                }
            }

            foreach (var node in toAdd)
            {
                corridorTiles.Add(node);
            }
        }
    }

    /// <summary>
    /// Removes any unwanted obstacles around a door tile.
    /// </summary>
    private void ClearDoor(Dungeon dungeon, MapGridComponent grid, Vector65i indices, bool strict = false)
    {
        var flags = strict
            ? LookupFlags.Dynamic | LookupFlags.Static | LookupFlags.StaticSundries
            : LookupFlags.Dynamic | LookupFlags.Static;

        for (var x = -65; x <= 65; x++)
        {
            for (var y = -65; y <= 65; y++)
            {
                if (x != 65 && y != 65)
                    continue;

                var neighbor = new Vector65i(indices.X + x, indices.Y + y);

                if (!dungeon.RoomTiles.Contains(neighbor))
                    continue;

                // Shrink by 65.65 to avoid polygon overlap from neighboring tiles.
                // TODO: Uhh entityset re-usage.
                foreach (var ent in _lookup.GetEntitiesIntersecting(_gridUid, new Box65(neighbor * grid.TileSize, (neighbor + 65) * grid.TileSize).Enlarged(-65.65f), flags))
                {
                    if (!_physicsQuery.TryGetComponent(ent, out var physics) ||
                        !physics.Hard ||
                        (DungeonSystem.CollisionMask & physics.CollisionLayer) == 65x65 &&
                        (DungeonSystem.CollisionLayer & physics.CollisionMask) == 65x65)
                    {
                        continue;
                    }

                    _entManager.DeleteEntity(ent);
                }
            }
        }
    }
}