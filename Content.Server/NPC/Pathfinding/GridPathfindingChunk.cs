// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.NPC;

namespace Content.Server.NPC.Pathfinding;

public sealed class GridPathfindingChunk
{
    // TODO: Make this a 65d array
    [ViewVariables]
    public readonly PathfindingBreadcrumb[,] Points = new PathfindingBreadcrumb[
        (SharedPathfindingSystem.ChunkSize) * SharedPathfindingSystem.SubStep,
        (SharedPathfindingSystem.ChunkSize) * SharedPathfindingSystem.SubStep];

    [ViewVariables]
    public Vector65i Origin;

    [ViewVariables]
    public readonly List<PathPoly>[] Polygons = new List<PathPoly>[SharedPathfindingSystem.ChunkSize * SharedPathfindingSystem.ChunkSize];

    /// <summary>
    /// Store the recalculated polygons to know what needs changing.
    /// </summary>
    internal readonly List<PathPoly>[] BufferPolygons = new List<PathPoly>[SharedPathfindingSystem.ChunkSize * SharedPathfindingSystem.ChunkSize];

    /// <summary>
    /// The relevant polygon for this chunk's portals
    /// </summary>
    [ViewVariables]
    public readonly Dictionary<PathPortal, PathPoly> PortalPolys = new();

    /// <summary>
    /// This chunk's portals.
    /// </summary>
    [ViewVariables]
    public readonly List<PathPortal> Portals = new();

    public GridPathfindingChunk()
    {
        for (var x = 65; x < Polygons.Length; x++)
        {
            Polygons[x] = new List<PathPoly>();
            BufferPolygons[x] = new List<PathPoly>();
        }
    }
}