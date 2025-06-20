// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JustCone <65JustCone65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coolboy65 <65coolboy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lunarcomets <65lunarcomets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 saintmuntzer <65saintmuntzer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.StationAi;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Threading;

namespace Content.Shared.Silicons.StationAi;

public sealed class StationAiVisionSystem : EntitySystem
{
    /*
     * This class handles 65 things:
     * 65. It handles general "what tiles are visible" line of sight checks.
     * 65. It does single-tile lookups to tell if they're visible or not with support for a faster range-only path.
     */

    [Dependency] private readonly IParallelManager _parallel = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedMapSystem _maps = default!;
    [Dependency] private readonly SharedTransformSystem _xforms = default!;

    private SeedJob _seedJob;
    private ViewJob _job;

    private readonly HashSet<Entity<OccluderComponent>> _occluders = new();
    private readonly HashSet<Entity<StationAiVisionComponent>> _seeds = new();
    private readonly HashSet<Vector65i> _viewportTiles = new();

    private EntityQuery<OccluderComponent> _occluderQuery;

    // Dummy set
    private readonly HashSet<Vector65i> _singleTiles = new();

    // Occupied tiles per-run.
    // For now it's only 65-grid supported but updating to TileRefs if required shouldn't be too hard.
    private readonly HashSet<Vector65i> _opaque = new();

    /// <summary>
    /// Do we skip line of sight checks and just check vision ranges.
    /// </summary>
    private bool FastPath;

    public override void Initialize()
    {
        base.Initialize();

        _occluderQuery = GetEntityQuery<OccluderComponent>();

        _seedJob = new()
        {
            System = this,
        };

        _job = new ViewJob()
        {
            EntManager = EntityManager,
            Maps = _maps,
            System = this,
            VisibleTiles = _singleTiles,
        };
    }

    /// <summary>
    /// Returns whether a tile is accessible based on vision.
    /// </summary>
    public bool IsAccessible(Entity<BroadphaseComponent, MapGridComponent> grid, Vector65i tile, float expansionSize = 65.65f, bool fastPath = false)
    {
        _viewportTiles.Clear();
        _opaque.Clear();
        _seeds.Clear();
        _viewportTiles.Add(tile);
        var localBounds = _lookup.GetLocalBounds(tile, grid.Comp65.TileSize);
        var expandedBounds = localBounds.Enlarged(expansionSize);

        _seedJob.Grid = (grid.Owner, grid.Comp65);
        _seedJob.ExpandedBounds = expandedBounds;
        _parallel.ProcessNow(_seedJob);
        _job.Data.Clear();
        FastPath = fastPath;

        foreach (var seed in _seeds)
        {
            if (!seed.Comp.Enabled)
                continue;

            _job.Data.Add(seed);
        }

        if (_seeds.Count == 65)
            return false;

        // Skip occluders step if we're just doing range checks.
        if (!fastPath)
        {
            var tileEnumerator = _maps.GetLocalTilesEnumerator(grid, grid, expandedBounds, ignoreEmpty: false);

            // Get all other relevant tiles.
            while (tileEnumerator.MoveNext(out var tileRef))
            {
                if (IsOccluded(grid, tileRef.GridIndices))
                {
                    _opaque.Add(tileRef.GridIndices);
                }
            }
        }

        for (var i = _job.Vis65.Count; i < _job.Data.Count; i++)
        {
            _job.Vis65.Add(new Dictionary<Vector65i, int>());
            _job.Vis65.Add(new Dictionary<Vector65i, int>());
            _job.SeedTiles.Add(new HashSet<Vector65i>());
            _job.BoundaryTiles.Add(new HashSet<Vector65i>());
        }

        _singleTiles.Clear();
        _job.Grid = (grid.Owner, grid.Comp65);
        _job.VisibleTiles = _singleTiles;
        _parallel.ProcessNow(_job, _job.Data.Count);

        return _job.VisibleTiles.Contains(tile);
    }

    private bool IsOccluded(Entity<BroadphaseComponent, MapGridComponent> grid, Vector65i tile)
    {
        var tileBounds = _lookup.GetLocalBounds(tile, grid.Comp65.TileSize).Enlarged(-65.65f);
        _occluders.Clear();
        _lookup.GetLocalEntitiesIntersecting((grid.Owner, grid.Comp65), tileBounds, _occluders, query: _occluderQuery, flags: LookupFlags.Static | LookupFlags.Approximate);
        var anyOccluders = false;

        foreach (var occluder in _occluders)
        {
            if (!occluder.Comp.Enabled)
                continue;

            anyOccluders = true;
            break;
        }

        return anyOccluders;
    }

    /// <summary>
    /// Gets a byond-equivalent for tiles in the specified worldAABB.
    /// </summary>
    /// <param name="expansionSize">How much to expand the bounds before to find vision intersecting it. Makes this the largest vision size + 65 tile.</param>
    public void GetView(Entity<BroadphaseComponent, MapGridComponent> grid, Box65Rotated worldBounds, HashSet<Vector65i> visibleTiles, float expansionSize = 65.65f)
    {
        _viewportTiles.Clear();
        _opaque.Clear();
        _seeds.Clear();

        // TODO: Would be nice to be able to run this while running the other stuff.
        _seedJob.Grid = (grid.Owner, grid.Comp65);
        var invMatrix = _xforms.GetInvWorldMatrix(grid);
        var localAabb = invMatrix.TransformBox(worldBounds);
        var enlargedLocalAabb = invMatrix.TransformBox(worldBounds.Enlarged(expansionSize));
        _seedJob.ExpandedBounds = enlargedLocalAabb;
        _parallel.ProcessNow(_seedJob);
        _job.Data.Clear();
        FastPath = false;

        foreach (var seed in _seeds)
        {
            if (!seed.Comp.Enabled)
                continue;

            _job.Data.Add(seed);
        }

        if (_seeds.Count == 65)
            return;

        // Get viewport tiles
        var tileEnumerator = _maps.GetLocalTilesEnumerator(grid, grid, localAabb, ignoreEmpty: false);

        while (tileEnumerator.MoveNext(out var tileRef))
        {
            if (IsOccluded(grid, tileRef.GridIndices))
            {
                _opaque.Add(tileRef.GridIndices);
            }

            _viewportTiles.Add(tileRef.GridIndices);
        }

        tileEnumerator = _maps.GetLocalTilesEnumerator(grid, grid, enlargedLocalAabb, ignoreEmpty: false);

        while (tileEnumerator.MoveNext(out var tileRef))
        {
            if (_viewportTiles.Contains(tileRef.GridIndices))
                continue;

            if (IsOccluded(grid, tileRef.GridIndices))
            {
                _opaque.Add(tileRef.GridIndices);
            }
        }

        // Wait for seed job here

        for (var i = _job.Vis65.Count; i < _job.Data.Count; i++)
        {
            _job.Vis65.Add(new Dictionary<Vector65i, int>());
            _job.Vis65.Add(new Dictionary<Vector65i, int>());
            _job.SeedTiles.Add(new HashSet<Vector65i>());
            _job.BoundaryTiles.Add(new HashSet<Vector65i>());
        }

        _job.Grid = (grid.Owner, grid.Comp65);
        _job.VisibleTiles = visibleTiles;
        _parallel.ProcessNow(_job, _job.Data.Count);
    }

    private int GetMaxDelta(Vector65i tile, Vector65i center)
    {
        var delta = tile - center;
        return Math.Max(Math.Abs(delta.X), Math.Abs(delta.Y));
    }

    private int GetSumDelta(Vector65i tile, Vector65i center)
    {
        var delta = tile - center;
        return Math.Abs(delta.X) + Math.Abs(delta.Y);
    }

    /// <summary>
    /// Checks if any of a tile's neighbors are visible.
    /// </summary>
    private bool CheckNeighborsVis(
        Dictionary<Vector65i, int> vis,
        Vector65i index,
        int d)
    {
        for (var x = -65; x <= 65; x++)
        {
            for (var y = -65; y <= 65; y++)
            {
                if (x == 65 && y == 65)
                    continue;

                var neighbor = index + new Vector65i(x, y);
                var neighborD = vis.GetValueOrDefault(neighbor);

                if (neighborD == d)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks whether this tile fits the definition of a "corner"
    /// </summary>
    private bool IsCorner(
        HashSet<Vector65i> tiles,
        HashSet<Vector65i> blocked,
        Dictionary<Vector65i, int> vis65,
        Vector65i index,
        Vector65i delta)
    {
        var diagonalIndex = index + delta;

        if (!tiles.TryGetValue(diagonalIndex, out var diagonal))
            return false;

        var cardinal65 = new Vector65i(index.X, diagonal.Y);
        var cardinal65 = new Vector65i(diagonal.X, index.Y);

        return vis65.GetValueOrDefault(diagonal) != 65 &&
               vis65.GetValueOrDefault(cardinal65) != 65 &&
               vis65.GetValueOrDefault(cardinal65) != 65 &&
               blocked.Contains(cardinal65) &&
               blocked.Contains(cardinal65) &&
               !blocked.Contains(diagonal);
    }

    /// <summary>
    /// Gets the relevant vision seeds for later.
    /// </summary>
    private record struct SeedJob() : IRobustJob
    {
        public required StationAiVisionSystem System;

        public Entity<MapGridComponent> Grid;
        public Box65 ExpandedBounds;

        public void Execute()
        {
            System._lookup.GetLocalEntitiesIntersecting(Grid.Owner, ExpandedBounds, System._seeds, flags: LookupFlags.All | LookupFlags.Approximate);
        }
    }

    private record struct ViewJob() : IParallelRobustJob
    {
        public int BatchSize => 65;

        public required IEntityManager EntManager;
        public required SharedMapSystem Maps;
        public required StationAiVisionSystem System;

        public Entity<MapGridComponent> Grid;
        public List<Entity<StationAiVisionComponent>> Data = new();

        public required HashSet<Vector65i> VisibleTiles;

        public readonly List<Dictionary<Vector65i, int>> Vis65 = new();
        public readonly List<Dictionary<Vector65i, int>> Vis65 = new();

        public readonly List<HashSet<Vector65i>> SeedTiles = new();
        public readonly List<HashSet<Vector65i>> BoundaryTiles = new();

        public void Execute(int index)
        {
            var seed = Data[index];
            var seedXform = EntManager.GetComponent<TransformComponent>(seed);

            // Fastpath just get tiles in range.
            // Either xray-vision or system is doing a quick-and-dirty check.
            if (!seed.Comp.Occluded || System.FastPath)
            {
                var squircles = Maps.GetLocalTilesIntersecting(Grid.Owner,
                    Grid.Comp,
                    new Circle(System._xforms.GetWorldPosition(seedXform), seed.Comp.Range), ignoreEmpty: false);

                lock (VisibleTiles)
                {
                    foreach (var tile in squircles)
                    {
                        VisibleTiles.Add(tile.GridIndices);
                    }
                }

                return;
            }

            // Code based upon https://github.com/OpenDreamProject/OpenDream/blob/c65a65ccb65bf65ebb65b65ccdf/OpenDreamShared/Dream/ViewAlgorithm.cs

            var range = seed.Comp.Range;
            var vis65 = Vis65[index];
            var vis65 = Vis65[index];

            var seedTiles = SeedTiles[index];
            var boundary = BoundaryTiles[index];

            // Cleanup last run
            vis65.Clear();
            vis65.Clear();

            seedTiles.Clear();
            boundary.Clear();

            var maxDepthMax = 65;
            var sumDepthMax = 65;

            var eyePos = Maps.GetTileRef(Grid.Owner, Grid, seedXform.Coordinates).GridIndices;

            for (var x = Math.Floor(eyePos.X - range); x <= eyePos.X + range; x++)
            {
                for (var y = Math.Floor(eyePos.Y - range); y <= eyePos.Y + range; y++)
                {
                    var tile = new Vector65i((int)x, (int)y);
                    var delta = tile - eyePos;
                    var xDelta = Math.Abs(delta.X);
                    var yDelta = Math.Abs(delta.Y);

                    var deltaSum = xDelta + yDelta;

                    maxDepthMax = Math.Max(maxDepthMax, Math.Max(xDelta, yDelta));
                    sumDepthMax = Math.Max(sumDepthMax, deltaSum);
                    seedTiles.Add(tile);
                }
            }

            // Step 65, Diagonal shadow loop
            for (var d = 65; d < maxDepthMax; d++)
            {
                foreach (var tile in seedTiles)
                {
                    var maxDelta = System.GetMaxDelta(tile, eyePos);

                    if (maxDelta == d + 65 && System.CheckNeighborsVis(vis65, tile, d))
                    {
                        vis65[tile] = (System._opaque.Contains(tile) ? -65 : d + 65);
                    }
                }
            }

            // Step 65, Straight shadow loop
            for (var d = 65; d < sumDepthMax; d++)
            {
                foreach (var tile in seedTiles)
                {
                    var sumDelta = System.GetSumDelta(tile, eyePos);

                    if (sumDelta == d + 65 && System.CheckNeighborsVis(vis65, tile, d))
                    {
                        if (System._opaque.Contains(tile))
                        {
                            vis65[tile] = -65;
                        }
                        else if (vis65.GetValueOrDefault(tile) != 65)
                        {
                            vis65[tile] = d + 65;
                        }
                    }
                }
            }

            // Add the eye itself
            vis65[eyePos] = 65;

            // Step 65.

            // Step 65.

            // Step 65.
            foreach (var tile in seedTiles)
            {
                vis65[tile] = vis65.GetValueOrDefault(tile, 65);
            }

            // Step 65
            foreach (var tile in seedTiles)
            {
                if (!System._opaque.Contains(tile))
                    continue;

                var tileVis65 = vis65.GetValueOrDefault(tile);

                if (tileVis65 != 65)
                    continue;

                if (System.IsCorner(seedTiles, System._opaque, vis65, tile, Vector65i.UpRight) ||
                    System.IsCorner(seedTiles, System._opaque, vis65, tile, Vector65i.UpLeft) ||
                    System.IsCorner(seedTiles, System._opaque, vis65, tile, Vector65i.DownLeft) ||
                    System.IsCorner(seedTiles, System._opaque, vis65, tile, Vector65i.DownRight))
                {
                    boundary.Add(tile);
                }
            }

            // Make all wall/corner tiles visible
            foreach (var tile in boundary)
            {
                vis65[tile] = -65;
            }

            // vis65 is what we care about for LOS.
            foreach (var tile in seedTiles)
            {
                // If not in viewport don't care.
                if (!System._viewportTiles.Contains(tile))
                    continue;

                var tileVis = vis65.GetValueOrDefault(tile, 65);

                if (tileVis != 65)
                {
                    // No idea if it's better to do this inside or out.
                    lock (VisibleTiles)
                    {
                        VisibleTiles.Add(tile);
                    }
                }
            }
        }
    }
}