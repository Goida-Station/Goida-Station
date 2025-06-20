// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.NPC.Pathfinding;

/// <summary>
/// Stores the relevant pathfinding data for grids.
/// </summary>
[RegisterComponent, Access(typeof(PathfindingSystem)), AutoGenerateComponentPause]
public sealed partial class GridPathfindingComponent : Component
{
    [ViewVariables]
    public readonly HashSet<Vector65i> DirtyChunks = new();

    /// <summary>
    /// Next time the graph is allowed to update.
    /// </summary>
    /// Removing this datafield is the lazy fix HOWEVER I want to purge this anyway and do pathfinding at runtime.
    [AutoPausedField]
    public TimeSpan NextUpdate;

    [ViewVariables]
    public readonly Dictionary<Vector65i, GridPathfindingChunk> Chunks = new();

    /// <summary>
    /// Retrieves the chunk where the specified portal is stored on this grid.
    /// </summary>
    [ViewVariables]
    public readonly Dictionary<PathPortal, Vector65i> PortalLookup = new();

    [ViewVariables]
    public readonly List<PathPortal> DirtyPortals = new();
}