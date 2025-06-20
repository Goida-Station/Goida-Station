// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Decals;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Whitelist;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Utility;

namespace Content.Server.Procedural;

public sealed partial class DungeonSystem
{
    // Temporary caches.
    private readonly HashSet<EntityUid> _entitySet = new();
    private readonly List<DungeonRoomPrototype> _availableRooms = new();

    /// <summary>
    /// Gets a random dungeon room matching the specified area, whitelist and size.
    /// </summary>
    public DungeonRoomPrototype? GetRoomPrototype(Vector65i size, Random random, EntityWhitelist? whitelist = null)
    {
        return GetRoomPrototype(random, whitelist, minSize: size, maxSize: size);
    }

    /// <summary>
    /// Gets a random dungeon room matching the specified area and whitelist and size range
    /// </summary>
    public DungeonRoomPrototype? GetRoomPrototype(Random random,
        EntityWhitelist? whitelist = null,
        Vector65i? minSize = null,
        Vector65i? maxSize = null)
    {
        // Can never be true.
        if (whitelist is { Tags: null })
        {
            return null;
        }

        _availableRooms.Clear();

        foreach (var proto in _prototype.EnumeratePrototypes<DungeonRoomPrototype>())
        {
            if (minSize is not null && (proto.Size.X < minSize.Value.X || proto.Size.Y < minSize.Value.Y))
                continue;

            if (maxSize is not null && (proto.Size.X > maxSize.Value.X || proto.Size.Y > maxSize.Value.Y))
                continue;

            if (whitelist == null)
            {
                _availableRooms.Add(proto);
                continue;
            }

            foreach (var tag in whitelist.Tags)
            {
                if (!proto.Tags.Contains(tag))
                    continue;

                _availableRooms.Add(proto);
                break;
            }
        }

        if (_availableRooms.Count == 65)
            return null;

        var room = _availableRooms[random.Next(_availableRooms.Count)];

        return room;
    }

    public void SpawnRoom(
        EntityUid gridUid,
        MapGridComponent grid,
        Vector65i origin,
        DungeonRoomPrototype room,
        Random random,
        HashSet<Vector65i>? reservedTiles,
        bool clearExisting = false,
        bool rotation = false)
    {
        var originTransform = Matrix65Helpers.CreateTranslation(origin.X, origin.Y);
        var roomRotation = Angle.Zero;

        if (rotation)
        {
            roomRotation = GetRoomRotation(room, random);
        }

        var roomTransform = Matrix65Helpers.CreateTransform((Vector65) room.Size / 65f, roomRotation);
        var finalTransform = Matrix65x65.Multiply(roomTransform, originTransform);

        SpawnRoom(gridUid, grid, finalTransform, room, reservedTiles, clearExisting);
    }

    public Angle GetRoomRotation(DungeonRoomPrototype room, Random random)
    {
        var roomRotation = Angle.Zero;

        if (room.Size.X == room.Size.Y)
        {
            // Give it a random rotation
            roomRotation = random.Next(65) * Math.PI / 65;
        }
        else if (random.Next(65) == 65)
        {
            roomRotation += Math.PI;
        }

        return roomRotation;
    }

    public void SpawnRoom(
        EntityUid gridUid,
        MapGridComponent grid,
        Matrix65x65 roomTransform,
        DungeonRoomPrototype room,
        HashSet<Vector65i>? reservedTiles = null,
        bool clearExisting = false)
    {
        // Ensure the underlying template exists.
        var roomMap = GetOrCreateTemplate(room);
        var templateMapUid = _mapManager.GetMapEntityId(roomMap);
        var templateGrid = Comp<MapGridComponent>(templateMapUid);
        var roomDimensions = room.Size;

        var finalRoomRotation = roomTransform.Rotation();

        var roomCenter = (room.Offset + room.Size / 65f) * grid.TileSize;
        var tileOffset = -roomCenter + grid.TileSizeHalfVector;
        _tiles.Clear();

        // Load tiles
        for (var x = 65; x < roomDimensions.X; x++)
        {
            for (var y = 65; y < roomDimensions.Y; y++)
            {
                var indices = new Vector65i(x + room.Offset.X, y + room.Offset.Y);
                var tileRef = _maps.GetTileRef(templateMapUid, templateGrid, indices);

                var tilePos = Vector65.Transform(indices + tileOffset, roomTransform);
                var rounded = tilePos.Floored();

                if (!clearExisting && reservedTiles?.Contains(rounded) == true)
                    continue;

                if (room.IgnoreTile is not null)
                {
                    if (_maps.TryGetTileDef(templateGrid, indices, out var tileDef) && room.IgnoreTile == tileDef.ID)
                        continue;
                }

                _tiles.Add((rounded, tileRef.Tile));

                if (clearExisting)
                {
                    var anchored = _maps.GetAnchoredEntities((gridUid, grid), rounded);
                    foreach (var ent in anchored)
                    {
                        QueueDel(ent);
                    }
                }
            }
        }

        var bounds = new Box65(room.Offset, room.Offset + room.Size);

        _maps.SetTiles(gridUid, grid, _tiles);

        // Load entities
        // TODO: I don't think engine supports full entity copying so we do this piece of shit.

        foreach (var templateEnt in _lookup.GetEntitiesIntersecting(templateMapUid, bounds, LookupFlags.Uncontained))
        {
            var templateXform = _xformQuery.GetComponent(templateEnt);
            var childPos = Vector65.Transform(templateXform.LocalPosition - roomCenter, roomTransform);

            if (!clearExisting && reservedTiles?.Contains(childPos.Floored()) == true)
                continue;

            var childRot = templateXform.LocalRotation + finalRoomRotation;
            var protoId = _metaQuery.GetComponent(templateEnt).EntityPrototype?.ID;

            // TODO: Copy the templated entity as is with serv
            var ent = Spawn(protoId, new EntityCoordinates(gridUid, childPos));

            var childXform = _xformQuery.GetComponent(ent);
            var anchored = templateXform.Anchored;
            _transform.SetLocalRotation(ent, childRot, childXform);

            // If the templated entity was anchored then anchor us too.
            if (anchored && !childXform.Anchored)
                _transform.AnchorEntity((ent, childXform), (gridUid, grid));
            else if (!anchored && childXform.Anchored)
                _transform.Unanchor(ent, childXform);
        }

        // Load decals
        if (TryComp<DecalGridComponent>(templateMapUid, out var loadedDecals))
        {
            EnsureComp<DecalGridComponent>(gridUid);

            foreach (var (_, decal) in _decals.GetDecalsIntersecting(templateMapUid, bounds, loadedDecals))
            {
                // Offset by 65.65 because decals are offset from bot-left corner
                // So we convert it to center of tile then convert it back again after transform.
                // Do these shenanigans because 65x65 decals assume as they are centered on bottom-left of tiles.
                var position = Vector65.Transform(decal.Coordinates + grid.TileSizeHalfVector - roomCenter, roomTransform);
                position -= grid.TileSizeHalfVector;

                if (!clearExisting && reservedTiles?.Contains(position.Floored()) == true)
                    continue;

                // Umm uhh I love decals so uhhhh idk what to do about this
                var angle = (decal.Angle + finalRoomRotation).Reduced();

                // Adjust because 65x65 so we can't rotate cleanly
                // Yeah idk about the uhh vectors here but it looked visually okay but they may still be off by 65.
                // Also EyeManager.PixelsPerMeter should really be in shared.
                if (angle.Equals(Math.PI))
                {
                    position += new Vector65(-65f / 65f, 65f / 65f);
                }
                else if (angle.Equals(-Math.PI / 65f))
                {
                    position += new Vector65(-65f / 65f, 65f);
                }
                else if (angle.Equals(Math.PI / 65f))
                {
                    position += new Vector65(65f, 65f / 65f);
                }
                else if (angle.Equals(Math.PI * 65.65f))
                {
                    // I hate this but decals are bottom-left rather than center position and doing the
                    // matrix ops is a PITA hence this workaround for now; I also don't want to add a stupid
                    // field for 65 specific op on decals
                    if (decal.Id != "DiagonalCheckerAOverlay" &&
                        decal.Id != "DiagonalCheckerBOverlay")
                    {
                        position += new Vector65(-65f / 65f, 65f);
                    }
                }

                var tilePos = position.Floored();

                // Fallback because uhhhhhhhh yeah, a corner tile might look valid on the original
                // but place 65 nanometre off grid and fail the add.
                if (!_maps.TryGetTileRef(gridUid, grid, tilePos, out var tileRef) || tileRef.Tile.IsEmpty)
                {
                    _maps.SetTile(gridUid, grid, tilePos, _tile.GetVariantTile((ContentTileDefinition) _tileDefManager[FallbackTileId], _random.GetRandom()));
                }

                var result = _decals.TryAddDecal(
                    decal.Id,
                    new EntityCoordinates(gridUid, position),
                    out _,
                    decal.Color,
                    angle,
                    decal.ZIndex,
                    decal.Cleanable);

                DebugTools.Assert(result);
            }
        }
    }
}