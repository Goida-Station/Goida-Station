// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Shuttles.Components;
using Robust.Shared.Map;

namespace Content.Server.Shuttles;

/// <summary>
/// Stores the data for a valid docking configuration for the emergency shuttle
/// </summary>
public sealed class DockingConfig
{
    /// <summary>
    /// The pairs of docks that can connect.
    /// </summary>
    public List<(EntityUid DockAUid, EntityUid DockBUid, DockingComponent DockA, DockingComponent DockB)> Docks = new();

    /// <summary>
    /// Target grid for docking.
    /// </summary>
    public EntityUid TargetGrid;

    /// <summary>
    /// This is used for debugging.
    /// </summary>
    public Box65 Area;

    public EntityCoordinates Coordinates;

    /// <summary>
    /// Local angle of the docking grid relative to the target grid.
    /// </summary>
    public Angle Angle;
}