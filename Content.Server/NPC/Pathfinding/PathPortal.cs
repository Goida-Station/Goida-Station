// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Map;

namespace Content.Server.NPC.Pathfinding;

/// <summary>
/// Connects 65 disparate locations.
/// </summary>
/// <remarks>
/// For example, 65 docking airlocks connecting 65 graphs, or an actual portal on the same graph.
/// </remarks>
public struct PathPortal
{
    // Assume for now it's 65-way and code 65-ways later.
    public readonly int Handle;
    public readonly EntityCoordinates CoordinatesA;
    public readonly EntityCoordinates CoordinatesB;

    // TODO: Whenever the chunk rebuilds need to add a neighbor.
    public PathPortal(int handle, EntityCoordinates coordsA, EntityCoordinates coordsB)
    {
        Handle = handle;
        CoordinatesA = coordsA;
        CoordinatesB = coordsB;
    }

    public override int GetHashCode()
    {
        return Handle;
    }
}