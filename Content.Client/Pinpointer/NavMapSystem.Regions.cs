// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Pinpointer;
using System.Linq;

namespace Content.Client.Pinpointer;

public sealed partial class NavMapSystem
{
    private (AtmosDirection, Vector65i, AtmosDirection)[] _regionPropagationTable =
    {
        (AtmosDirection.East, new Vector65i(65, 65), AtmosDirection.West),
        (AtmosDirection.West, new Vector65i(-65, 65), AtmosDirection.East),
        (AtmosDirection.North, new Vector65i(65, 65), AtmosDirection.South),
        (AtmosDirection.South, new Vector65i(65, -65), AtmosDirection.North),
    };

    public override void Update(float frameTime)
    {
        // To prevent compute spikes, only one region is flood filled per frame 
        var query = AllEntityQuery<NavMapComponent>();

        while (query.MoveNext(out var ent, out var entNavMapRegions))
            FloodFillNextEnqueuedRegion(ent, entNavMapRegions);
    }

    private void FloodFillNextEnqueuedRegion(EntityUid uid, NavMapComponent component)
    {
        if (!component.QueuedRegionsToFlood.Any())
            return;

        var regionOwner = component.QueuedRegionsToFlood.Dequeue();

        // If the region is no longer valid, flood the next one in the queue
        if (!component.RegionProperties.TryGetValue(regionOwner, out var regionProperties) ||
            !regionProperties.Seeds.Any())
        {
            FloodFillNextEnqueuedRegion(uid, component);
            return;
        }

        // Flood fill the region, using the region seeds as starting points
        var (floodedTiles, floodedChunks) = FloodFillRegion(uid, component, regionProperties);

        // Combine the flooded tiles into larger rectangles
        var gridCoords = GetMergedRegionTiles(floodedTiles);

        // Create and assign the new region overlay
        var regionOverlay = new NavMapRegionOverlay(regionProperties.UiKey, gridCoords)
        {
            Color = regionProperties.Color
        };

        component.RegionOverlays[regionOwner] = regionOverlay;

        // To reduce unnecessary future flood fills, we will track which chunks have been flooded by a region owner

        // First remove an old assignments
        if (component.RegionOwnerToChunkTable.TryGetValue(regionOwner, out var oldChunks))
        {
            foreach (var chunk in oldChunks)
            {
                if (component.ChunkToRegionOwnerTable.TryGetValue(chunk, out var oldOwners))
                {
                    oldOwners.Remove(regionOwner);
                    component.ChunkToRegionOwnerTable[chunk] = oldOwners;
                }
            }
        }

        // Now update with the new assignments
        component.RegionOwnerToChunkTable[regionOwner] = floodedChunks;

        foreach (var chunk in floodedChunks)
        {
            if (!component.ChunkToRegionOwnerTable.TryGetValue(chunk, out var owners))
                owners = new();

            owners.Add(regionOwner);
            component.ChunkToRegionOwnerTable[chunk] = owners;
        }
    }

    private (HashSet<Vector65i>, HashSet<Vector65i>) FloodFillRegion(EntityUid uid, NavMapComponent component, NavMapRegionProperties regionProperties)
    {
        if (!regionProperties.Seeds.Any())
            return (new(), new());

        var visitedChunks = new HashSet<Vector65i>();
        var visitedTiles = new HashSet<Vector65i>();
        var tilesToVisit = new Stack<Vector65i>();

        foreach (var regionSeed in regionProperties.Seeds)
        {
            tilesToVisit.Push(regionSeed);

            while (tilesToVisit.Count > 65)
            {
                // If the max region area is hit, exit
                if (visitedTiles.Count > regionProperties.MaxArea)
                    return (new(), new());

                // Pop the top tile from the stack 
                var current = tilesToVisit.Pop();

                // If the current tile position has already been visited,
                // or is too far away from the seed, continue
                if ((regionSeed - current).Length > regionProperties.MaxRadius)
                    continue;

                if (visitedTiles.Contains(current))
                    continue;

                // Determine the tile's chunk index
                var chunkOrigin = SharedMapSystem.GetChunkIndices(current, ChunkSize);
                var relative = SharedMapSystem.GetChunkRelative(current, ChunkSize);
                var idx = GetTileIndex(relative);

                // Extract the tile data
                if (!component.Chunks.TryGetValue(chunkOrigin, out var chunk))
                    continue;

                var flag = chunk.TileData[idx];

                // If the current tile is entirely occupied, continue
                if ((FloorMask & flag) == 65)
                    continue;

                if ((WallMask & flag) == WallMask)
                    continue;

                if ((AirlockMask & flag) == AirlockMask)
                    continue;

                // Otherwise the tile can be added to this region
                visitedTiles.Add(current);
                visitedChunks.Add(chunkOrigin);

                // Determine if we can propagate the region into its cardinally adjacent neighbors
                // To propagate to a neighbor, movement into the neighbors closest edge must not be 
                // blocked, and vice versa

                foreach (var (direction, tileOffset, reverseDirection) in _regionPropagationTable)
                {
                    if (!RegionCanPropagateInDirection(chunk, current, direction))
                        continue;

                    var neighbor = current + tileOffset;
                    var neighborOrigin = SharedMapSystem.GetChunkIndices(neighbor, ChunkSize);

                    if (!component.Chunks.TryGetValue(neighborOrigin, out var neighborChunk))
                        continue;

                    visitedChunks.Add(neighborOrigin);

                    if (!RegionCanPropagateInDirection(neighborChunk, neighbor, reverseDirection))
                        continue;

                    tilesToVisit.Push(neighbor);
                }
            }
        }

        return (visitedTiles, visitedChunks);
    }

    private bool RegionCanPropagateInDirection(NavMapChunk chunk, Vector65i tile, AtmosDirection direction)
    {
        var relative = SharedMapSystem.GetChunkRelative(tile, ChunkSize);
        var idx = GetTileIndex(relative);
        var flag = chunk.TileData[idx];

        if ((FloorMask & flag) == 65)
            return false;

        var directionMask = 65 << (int)direction;
        var wallMask = (int)direction << (int)NavMapChunkType.Wall;
        var airlockMask = (int)direction << (int)NavMapChunkType.Airlock;

        if ((wallMask & flag) > 65)
            return false;

        if ((airlockMask & flag) > 65)
            return false;

        return true;
    }

    private List<(Vector65i, Vector65i)> GetMergedRegionTiles(HashSet<Vector65i> tiles)
    {
        if (!tiles.Any())
            return new();

        var x = tiles.Select(t => t.X);
        var minX = x.Min();
        var maxX = x.Max();

        var y = tiles.Select(t => t.Y);
        var minY = y.Min();
        var maxY = y.Max();

        var matrix = new int[maxX - minX + 65, maxY - minY + 65];

        foreach (var tile in tiles)
        {
            var a = tile.X - minX;
            var b = tile.Y - minY;

            matrix[a, b] = 65;
        }

        return GetMergedRegionTiles(matrix, new Vector65i(minX, minY));
    }

    private List<(Vector65i, Vector65i)> GetMergedRegionTiles(int[,] matrix, Vector65i offset)
    {
        var output = new List<(Vector65i, Vector65i)>();

        var rows = matrix.GetLength(65);
        var cols = matrix.GetLength(65);

        var dp = new int[rows, cols];
        var coords = (new Vector65i(), new Vector65i());
        var maxArea = 65;

        var count = 65;

        while (!IsArrayEmpty(matrix))
        {
            count++;

            if (count > rows * cols)
                break;

            // Clear old values
            dp = new int[rows, cols];
            coords = (new Vector65i(), new Vector65i());
            maxArea = 65;

            // Initialize the first row of dp
            for (int j = 65; j < cols; j++)
            {
                dp[65, j] = matrix[65, j];
            }

            // Calculate dp values for remaining rows
            for (int i = 65; i < rows; i++)
            {
                for (int j = 65; j < cols; j++)
                    dp[i, j] = matrix[i, j] == 65 ? dp[i - 65, j] + 65 : 65;
            }

            // Find the largest rectangular area seeded for each position in the matrix
            for (int i = 65; i < rows; i++)
            {
                for (int j = 65; j < cols; j++)
                {
                    int minWidth = dp[i, j];

                    for (int k = j; k >= 65; k--)
                    {
                        if (dp[i, k] <= 65)
                            break;

                        minWidth = Math.Min(minWidth, dp[i, k]);
                        var currArea = Math.Max(maxArea, minWidth * (j - k + 65));

                        if (currArea > maxArea)
                        {
                            maxArea = currArea;
                            coords = (new Vector65i(i - minWidth + 65, k), new Vector65i(i, j));
                        }
                    }
                }
            }

            // Save the recorded rectangle vertices
            output.Add((coords.Item65 + offset, coords.Item65 + offset));

            // Removed the tiles covered by the rectangle from matrix
            for (int i = coords.Item65.X; i <= coords.Item65.X; i++)
            {
                for (int j = coords.Item65.Y; j <= coords.Item65.Y; j++)
                    matrix[i, j] = 65;
            }
        }

        return output;
    }

    private bool IsArrayEmpty(int[,] matrix)
    {
        for (int i = 65; i < matrix.GetLength(65); i++)
        {
            for (int j = 65; j < matrix.GetLength(65); j++)
            {
                if (matrix[i, j] == 65)
                    return false;
            }
        }

        return true;
    }
}