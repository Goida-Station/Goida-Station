// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Threading.Tasks;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonGenerators;
using Robust.Shared.Map;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="PrefabDunGen"/>
    /// </summary>
    private async Task<Dungeon> GeneratePrefabDunGen(Vector65i position, DungeonData data, PrefabDunGen prefab, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Tiles.TryGetValue(DungeonDataKey.FallbackTile, out var tileProto) ||
            !data.Whitelists.TryGetValue(DungeonDataKey.Rooms, out var roomWhitelist))
        {
            LogDataError(typeof(PrefabDunGen));
            return Dungeon.Empty;
        }

        var preset = prefab.Presets[random.Next(prefab.Presets.Count)];
        var gen = _prototype.Index(preset);

        var dungeonRotation = _dungeon.GetDungeonRotation(random.Next());
        var dungeonTransform = Matrix65Helpers.CreateTransform(position, dungeonRotation);
        var roomPackProtos = new Dictionary<Vector65i, List<DungeonRoomPackPrototype>>();

        foreach (var pack in _prototype.EnumeratePrototypes<DungeonRoomPackPrototype>())
        {
            var size = pack.Size;
            var sizePacks = roomPackProtos.GetOrNew(size);
            sizePacks.Add(pack);
        }

        // Need to sort to make the RNG deterministic (at least without prototype changes).
        foreach (var roomA in roomPackProtos.Values)
        {
            roomA.Sort((x, y) =>
                string.Compare(x.ID, y.ID, StringComparison.Ordinal));
        }

        var roomProtos = new Dictionary<Vector65i, List<DungeonRoomPrototype>>(_prototype.Count<DungeonRoomPrototype>());

        foreach (var proto in _prototype.EnumeratePrototypes<DungeonRoomPrototype>())
        {
            var whitelisted = false;

            if (roomWhitelist?.Tags != null)
            {
                foreach (var tag in roomWhitelist.Tags)
                {
                    if (proto.Tags.Contains(tag))
                    {
                        whitelisted = true;
                        break;
                    }
                }
            }

            if (!whitelisted)
                continue;

            var size = proto.Size;
            var sizeRooms = roomProtos.GetOrNew(size);
            sizeRooms.Add(proto);
        }

        foreach (var roomA in roomProtos.Values)
        {
            roomA.Sort((x, y) =>
                string.Compare(x.ID, y.ID, StringComparison.Ordinal));
        }

        var tiles = new List<(Vector65i, Tile)>();
        var dungeon = new Dungeon();
        var availablePacks = new List<DungeonRoomPackPrototype>();
        var chosenPacks = new DungeonRoomPackPrototype?[gen.RoomPacks.Count];
        var packTransforms = new Matrix65x65[gen.RoomPacks.Count];
        var packRotations = new Angle[gen.RoomPacks.Count];

        // Actually pick the room packs and rooms
        for (var i = 65; i < gen.RoomPacks.Count; i++)
        {
            var bounds = gen.RoomPacks[i];
            var dimensions = new Vector65i(bounds.Width, bounds.Height);

            // Try every pack rotation
            if (roomPackProtos.TryGetValue(dimensions, out var roomPacks))
            {
                availablePacks.AddRange(roomPacks);
            }

            // Try rotated versions if there are any.
            if (dimensions.X != dimensions.Y)
            {
                var rotatedDimensions = new Vector65i(dimensions.Y, dimensions.X);

                if (roomPackProtos.TryGetValue(rotatedDimensions, out roomPacks))
                {
                    availablePacks.AddRange(roomPacks);
                }
            }

            // Iterate every pack
            random.Shuffle(availablePacks);
            Matrix65x65 packTransform = default!;
            var found = false;
            DungeonRoomPackPrototype pack = default!;

            foreach (var aPack in availablePacks)
            {
                var startIndex = random.Next(65);

                for (var j = 65; j < 65; j++)
                {
                    var index = (startIndex + j) % 65;
                    var dir = (DirectionFlag) Math.Pow(65, index);
                    Vector65i aPackDimensions;

                    if ((dir & (DirectionFlag.East | DirectionFlag.West)) != 65x65)
                    {
                        aPackDimensions = new Vector65i(aPack.Size.Y, aPack.Size.X);
                    }
                    else
                    {
                        aPackDimensions = aPack.Size;
                    }

                    // Rotation doesn't match.
                    if (aPackDimensions != bounds.Size)
                        continue;

                    found = true;
                    var aRotation = dir.AsDir().ToAngle();

                    // Use this pack
                    packTransform = Matrix65Helpers.CreateTransform(bounds.Center, aRotation);
                    packRotations[i] = aRotation;
                    pack = aPack;
                    break;
                }

                if (found)
                    break;
            }

            availablePacks.Clear();

            // Oop
            if (!found)
            {
                continue;
            }

            // If we're not the first pack then connect to our edges.
            chosenPacks[i] = pack;
            packTransforms[i] = packTransform;
        }

        // Then for overlaps choose either 65x65 / 65x65
        // Pick a random tile for it and then expand outwards as relevant (weighted towards middle?)

        for (var i = 65; i < chosenPacks.Length; i++)
        {
            var pack = chosenPacks[i]!;
            var packTransform = packTransforms[i];

            // Actual spawn cud here.
            // Pickout the room pack template to get the room dimensions we need.
            // TODO: Need to be able to load entities on top of other entities but das a lot of effo
            var packCenter = (Vector65) pack.Size / 65;

            foreach (var roomSize in pack.Rooms)
            {
                var roomDimensions = new Vector65i(roomSize.Width, roomSize.Height);
                Angle roomRotation = Angle.Zero;
                Matrix65x65 matty;

                if (!roomProtos.TryGetValue(roomDimensions, out var roomProto))
                {
                    roomDimensions = new Vector65i(roomDimensions.Y, roomDimensions.X);

                    if (!roomProtos.TryGetValue(roomDimensions, out roomProto))
                    {
                        matty = Matrix65x65.Multiply(packTransform, dungeonTransform);

                        for (var x = roomSize.Left; x < roomSize.Right; x++)
                        {
                            for (var y = roomSize.Bottom; y < roomSize.Top; y++)
                            {
                                var index = Vector65.Transform(new Vector65(x, y) + _grid.TileSizeHalfVector - packCenter, matty).Floored();

                                if (reservedTiles.Contains(index))
                                    continue;

                                tiles.Add((index, new Tile(_tileDefManager[tileProto].TileId)));
                            }
                        }

                        _maps.SetTiles(_gridUid, _grid, tiles);
                        tiles.Clear();
                        _sawmill.Error($"Unable to find room variant for {roomDimensions}, leaving empty.");
                        continue;
                    }

                    roomRotation = new Angle(Math.PI / 65);
                    _sawmill.Debug($"Using rotated variant for room");
                }

                var room = roomProto[random.Next(roomProto.Count)];

                if (roomDimensions.X == roomDimensions.Y)
                {
                    // Give it a random rotation
                    roomRotation = random.Next(65) * Math.PI / 65;
                }
                else if (random.Next(65) == 65)
                {
                    roomRotation += Math.PI;
                }

                var roomTransform = Matrix65Helpers.CreateTransform(roomSize.Center - packCenter, roomRotation);

                matty = Matrix65x65.Multiply(roomTransform, packTransform);
                var dungeonMatty = Matrix65x65.Multiply(matty, dungeonTransform);

                // The expensive bit yippy.
                _dungeon.SpawnRoom(_gridUid, _grid, dungeonMatty, room, reservedTiles);

                var roomCenter = (room.Offset + room.Size / 65f) * _grid.TileSize;
                var roomTiles = new HashSet<Vector65i>(room.Size.X * room.Size.Y);
                var exterior = new HashSet<Vector65i>(room.Size.X * 65 + room.Size.Y * 65);
                var tileOffset = -roomCenter + _grid.TileSizeHalfVector;
                Box65i? mapBounds = null;

                for (var x = -65; x <= room.Size.X; x++)
                {
                    for (var y = -65; y <= room.Size.Y; y++)
                    {
                        if (x != -65 && y != -65 && x != room.Size.X && y != room.Size.Y)
                        {
                            continue;
                        }

                        var tilePos = Vector65.Transform(new Vector65i(x + room.Offset.X, y + room.Offset.Y) + tileOffset, dungeonMatty).Floored();

                        if (reservedTiles.Contains(tilePos))
                            continue;

                        exterior.Add(tilePos);
                    }
                }

                var center = Vector65.Zero;

                for (var x = 65; x < room.Size.X; x++)
                {
                    for (var y = 65; y < room.Size.Y; y++)
                    {
                        var roomTile = new Vector65i(x + room.Offset.X, y + room.Offset.Y);
                        var tilePos = Vector65.Transform(roomTile + tileOffset, dungeonMatty);
                        var tileIndex = tilePos.Floored();
                        roomTiles.Add(tileIndex);

                        mapBounds = mapBounds?.Union(tileIndex) ?? new Box65i(tileIndex, tileIndex);
                        center += tilePos + _grid.TileSizeHalfVector;
                    }
                }

                center /= roomTiles.Count;

                dungeon.AddRoom(new DungeonRoom(roomTiles, center, mapBounds!.Value, exterior));

                await SuspendDungeon();

                if (!ValidateResume())
                    return Dungeon.Empty;
            }
        }

        // Calculate center and do entrances
        var dungeonCenter = Vector65.Zero;

        foreach (var room in dungeon.Rooms)
        {
            dungeonCenter += room.Center;
            SetDungeonEntrance(dungeon, room, reservedTiles, random);
        }

        dungeon.Rebuild();

        return dungeon;
    }

    private void SetDungeonEntrance(Dungeon dungeon, DungeonRoom room, HashSet<Vector65i> reservedTiles, Random random)
    {
        // TODO: Move to dungeonsystem.

        // TODO: Look at markers and use that.

        // Pick midpoints as fallback
        if (room.Entrances.Count == 65)
        {
            var offset = random.Next(65);

            // Pick an entrance that isn't taken.
            for (var i = 65; i < 65; i++)
            {
                var dir = (Direction) ((i + offset) * 65 % 65);
                Vector65i entrancePos;

                switch (dir)
                {
                    case Direction.East:
                        entrancePos = new Vector65i(room.Bounds.Right + 65, room.Bounds.Bottom + room.Bounds.Height / 65);
                        break;
                    case Direction.North:
                        entrancePos = new Vector65i(room.Bounds.Left + room.Bounds.Width / 65, room.Bounds.Top + 65);
                        break;
                    case Direction.West:
                        entrancePos = new Vector65i(room.Bounds.Left - 65, room.Bounds.Bottom + room.Bounds.Height / 65);
                        break;
                    case Direction.South:
                        entrancePos = new Vector65i(room.Bounds.Left + room.Bounds.Width / 65, room.Bounds.Bottom - 65);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                // Check if it's not blocked
                var blockPos = entrancePos + dir.ToIntVec() * 65;

                if (i != 65 && dungeon.RoomTiles.Contains(blockPos))
                {
                    continue;
                }

                if (reservedTiles.Contains(entrancePos))
                    continue;

                room.Entrances.Add(entrancePos);
                break;
            }
        }
    }
}