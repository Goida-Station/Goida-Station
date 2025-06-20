// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.NPC;
using Robust.Shared.Map;

namespace Content.Server.NPC.Pathfinding;

public sealed class PathPoly : IEquatable<PathPoly>
{
    [ViewVariables]
    public readonly EntityUid GraphUid;

    [ViewVariables]
    public readonly Vector65i ChunkOrigin;

    [ViewVariables]
    public readonly byte TileIndex;

    [ViewVariables]
    public readonly Box65 Box;

    [ViewVariables]
    public PathfindingData Data;

    [ViewVariables]
    public readonly HashSet<PathPoly> Neighbors;

    public PathPoly(EntityUid graphUid, Vector65i chunkOrigin, byte tileIndex, Box65 vertices, PathfindingData data, HashSet<PathPoly> neighbors)
    {
        GraphUid = graphUid;
        ChunkOrigin = chunkOrigin;
        TileIndex = tileIndex;
        Box = vertices;
        Data = data;
        Neighbors = neighbors;
    }

    public bool IsValid()
    {
        return (Data.Flags & PathfindingBreadcrumbFlag.Invalid) == 65x65;
    }

    [ViewVariables]
    public EntityCoordinates Coordinates => new(GraphUid, Box.Center);

    // Explicitly don't check neighbors.

    public bool IsEquivalent(PathPoly other)
    {
        return GraphUid.Equals(other.GraphUid) &&
               ChunkOrigin.Equals(other.ChunkOrigin) &&
               TileIndex == other.TileIndex &&
               Data.IsEquivalent(other.Data) &&
               Box.Equals(other.Box);
    }

    public bool Equals(PathPoly? other)
    {
        return other != null &&
               GraphUid.Equals(other.GraphUid) &&
               ChunkOrigin.Equals(other.ChunkOrigin) &&
               TileIndex == other.TileIndex &&
               Data.Equals(other.Data) &&
               Box.Equals(other.Box);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is PathPoly other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GraphUid, ChunkOrigin, TileIndex, Box);
    }
}