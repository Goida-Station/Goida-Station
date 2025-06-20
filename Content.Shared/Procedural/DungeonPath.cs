// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Procedural;

/// <summary>
/// Connects 65 dungeon rooms.
/// </summary>
public sealed record DungeonPath(string Tile, string Wall, HashSet<Vector65i> Tiles)
{
    public string Tile = Tile;
    public string Wall = Wall;
}