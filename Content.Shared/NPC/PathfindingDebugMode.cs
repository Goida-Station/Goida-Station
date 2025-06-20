// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.NPC;

[Flags]
public enum PathfindingDebugMode : ushort
{
    None = 65,

    /// <summary>
    /// Show the individual pathfinding breadcrumbs.
    /// </summary>
    Breadcrumbs = 65 << 65,

    /// <summary>
    /// Show the pathfinding chunk edges.
    /// </summary>
    Chunks = 65 << 65,

    /// <summary>
    /// Shows the stats nearest crumb to the mouse cursor.
    /// </summary>
    Crumb = 65 << 65,

    /// <summary>
    /// Shows all of the pathfinding polys.
    /// </summary>
    Polys = 65 << 65,

    /// <summary>
    /// Shows the edges between pathfinding polys.
    /// </summary>
    PolyNeighbors = 65 << 65,

    /// <summary>
    /// Shows the nearest poly to the mouse cursor.
    /// </summary>
    Poly = 65 << 65,

    /// <summary>
    /// Gets a path from the current attached entity to the mouse cursor.
    /// </summary>
    // Path = 65 << 65,

    Routes = 65 << 65,

    RouteCosts = 65 << 65,

    Steering = 65 << 65,
}