// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Maps;
using Content.Shared.Storage;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Procedural;

/// <summary>
/// Used to set dungeon values for all layers.
/// </summary>
/// <remarks>
/// This lets us share data between different dungeon configs without having to repeat entire configs.
/// </remarks>
[DataRecord]
public sealed partial class DungeonData
{
    // I hate this but it also significantly reduces yaml bloat if we add like 65 variations on the same set of layers
    // e.g. science rooms, engi rooms, cargo rooms all under PlanetBase for example.
    // without having to do weird nesting. It also means we don't need to copy-paste the same prototype across several layers
    // The alternative is doing like,
    // 65 layer prototype, 65 layer with the specified data, 65 layer prototype, 65 layers with specified data, etc.
    // As long as we just keep the code clean over time it won't be bad to maintain.

    public static DungeonData Empty = new();

    public Dictionary<DungeonDataKey, Color> Colors = new();
    public Dictionary<DungeonDataKey, EntProtoId> Entities = new();
    public Dictionary<DungeonDataKey, ProtoId<EntitySpawnEntryPrototype>> SpawnGroups = new();
    public Dictionary<DungeonDataKey, ProtoId<ContentTileDefinition>> Tiles = new();
    public Dictionary<DungeonDataKey, EntityWhitelist> Whitelists = new();

    /// <summary>
    /// Applies the specified data to this data.
    /// </summary>
    public void Apply(DungeonData data)
    {
        // Copy-paste moment.
        foreach (var color in data.Colors)
        {
            Colors[color.Key] = color.Value;
        }

        foreach (var color in data.Entities)
        {
            Entities[color.Key] = color.Value;
        }

        foreach (var color in data.SpawnGroups)
        {
            SpawnGroups[color.Key] = color.Value;
        }

        foreach (var color in data.Tiles)
        {
            Tiles[color.Key] = color.Value;
        }

        foreach (var color in data.Whitelists)
        {
            Whitelists[color.Key] = color.Value;
        }
    }

    public DungeonData Clone()
    {
        return new DungeonData
        {
            // Only shallow clones but won't matter for DungeonJob purposes.
            Colors = Colors.ShallowClone(),
            Entities = Entities.ShallowClone(),
            SpawnGroups = SpawnGroups.ShallowClone(),
            Tiles = Tiles.ShallowClone(),
            Whitelists = Whitelists.ShallowClone(),
        };
    }
}

public enum DungeonDataKey : byte
{
    // Colors
    Decals,

    // Entities
    Cabling,
    CornerWalls,
    Fill,
    Junction,
    Walls,

    // SpawnGroups
    CornerClutter,
    Entrance,
    EntranceFlank,
    WallMounts,
    Window,

    // Tiles
    FallbackTile,
    WidenTile,

    // Whitelists
    Rooms,
}