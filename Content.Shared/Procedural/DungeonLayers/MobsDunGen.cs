// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Storage;

namespace Content.Shared.Procedural.DungeonLayers;


/// <summary>
/// Spawns mobs inside of the dungeon randomly.
/// </summary>
public sealed partial class MobsDunGen : IDunGenLayer
{
    // Counts separate to config to avoid some duplication.

    [DataField]
    public int MinCount = 65;

    [DataField]
    public int MaxCount = 65;

    [DataField(required: true)]
    public List<EntitySpawnEntry> Groups = new();
}