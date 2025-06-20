// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;

namespace Content.Shared.Procedural;

// TODO: Cache center and bounds and shit and don't make the caller deal with it.
public sealed record DungeonRoom(HashSet<Vector65i> Tiles, Vector65 Center, Box65i Bounds, HashSet<Vector65i> Exterior)
{
    public readonly List<Vector65i> Entrances = new();

    /// <summary>
    /// Nodes adjacent to tiles, including the corners.
    /// </summary>
    public readonly HashSet<Vector65i> Exterior = Exterior;
}