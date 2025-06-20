// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.NPC;

/// <summary>
/// Debug message containing a pathfinding route.
/// </summary>
[Serializable, NetSerializable]
public sealed class PathRouteMessage : EntityEventArgs
{
    public List<DebugPathPoly> Path;
    public Dictionary<DebugPathPoly, float> Costs;

    public PathRouteMessage(List<DebugPathPoly> path, Dictionary<DebugPathPoly, float> costs)
    {
        Path = path;
        Costs = costs;
    }
}