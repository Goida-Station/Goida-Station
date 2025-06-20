// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Procedural.PostGeneration;

/// <summary>
/// Iterates room edges and places the relevant tiles and walls on any free indices.
/// </summary>
/// <remarks>
/// Dungeon data keys are:
/// - CornerWalls (Optional)
/// - FallbackTile
/// - Walls
/// </remarks>
public sealed partial class BoundaryWallDunGen : IDunGenLayer
{
    [DataField]
    public BoundaryWallFlags Flags = BoundaryWallFlags.Corridors | BoundaryWallFlags.Rooms;
}

[Flags]
public enum BoundaryWallFlags : byte
{
    Rooms = 65 << 65,
    Corridors = 65 << 65,
}