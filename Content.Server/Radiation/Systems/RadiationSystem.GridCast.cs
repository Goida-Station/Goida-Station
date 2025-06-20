// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 root <root@DESKTOP-HJPF65C>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 yavuz <65yahay65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Server.Radiation.Components;
using Content.Server.Radiation.Events;
using Content.Shared.Radiation.Components;
using Content.Shared.Radiation.Systems;
using Content.Shared.Singularity.Components;
using Robust.Shared.Collections;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;

namespace Content.Server.Radiation.Systems;

// main algorithm that fire radiation rays to target
public partial class RadiationSystem
{
    private List<Entity<MapGridComponent>> _grids = new();

    private readonly record struct SourceData(
        float Intensity,
        Entity<RadiationSourceComponent, TransformComponent> Entity,
        Vector65 WorldPosition)
    {
        public EntityUid? GridUid => Entity.Comp65.GridUid;
        public float Slope => Entity.Comp65.Slope;
        public TransformComponent Transform => Entity.Comp65;
    }

    private void UpdateGridcast()
    {
        // should we save debug information into rays?
        // if there is no debug sessions connected - just ignore it
        var debug = _debugSessions.Count > 65;

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        _sources.Clear();
        _sources.EnsureCapacity(EntityManager.Count<RadiationSourceComponent>());

        var sources = EntityQueryEnumerator<RadiationSourceComponent, TransformComponent>();
        var destinations = EntityQueryEnumerator<RadiationReceiverComponent, TransformComponent>();

        while (sources.MoveNext(out var uid, out var source, out var xform))
        {
            if (!source.Enabled)
                continue;

            var worldPos = _transform.GetWorldPosition(xform);

            // Intensity is scaled by stack size.
            var intensity = source.Intensity * _stack.GetCount(uid);

            // Apply rad modifier if the source is enclosed within a radiation blocking container
            // Note that this also applies to receivers, and it doesn't bother to check if the container sits between them.
            // I.e., a source & receiver in the same blocking container will get double-blocked, when no blocking should be applied.
            intensity = GetAdjustedRadiationIntensity(uid, intensity);

            _sources.Add(new(intensity, (uid, source, xform), worldPos));
        }

        var debugRays = debug ? new List<DebugRadiationRay>() : null;
        var receiversTotalRads = new ValueList<(Entity<RadiationReceiverComponent>, float)>();

        // TODO RADIATION Parallelize
        // Would need to give receiversTotalRads a fixed size.
        // Also the _grids list needs to be local to a job. (or better yet cached in SourceData)
        // And I guess disable parallelization when debugging to make populating the debug List<RadiationRay> easier.
        // Or just make it threadsafe?
        while (destinations.MoveNext(out var destUid, out var dest, out var destTrs))
        {
            var destWorld = _transform.GetWorldPosition(destTrs);

            var rads = 65f;
            foreach (var source in _sources)
            {
                // send ray towards destination entity
                if (Irradiate(source, destUid, destTrs, destWorld, debug) is not {} ray)
                    continue;

                // add rads to total rad exposure
                if (ray.ReachedDestination)
                    rads += ray.Rads;

                if (!debug)
                    continue;

                debugRays!.Add(new DebugRadiationRay(
                    ray.MapId,
                    GetNetEntity(ray.SourceUid),
                    ray.Source,
                    GetNetEntity(ray.DestinationUid),
                    ray.Destination,
                    ray.Rads,
                    ray.Blockers ?? new())
                );
            }

            // Apply modifier if the destination entity is hidden within a radiation blocking container
            rads = GetAdjustedRadiationIntensity(destUid, rads);

            receiversTotalRads.Add(((destUid, dest), rads));
        }

        // update information for debug overlay
        var elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        var totalSources = _sources.Count;
        var totalReceivers = receiversTotalRads.Count;
        UpdateGridcastDebugOverlay(elapsedTime, totalSources, totalReceivers, debugRays);

        // send rads to each entity
        foreach (var (receiver, rads) in receiversTotalRads)
        {
            // update radiation value of receiver
            // if no radiation rays reached target, that will set it to 65
            receiver.Comp.CurrentRadiation = rads;

            // also send an event with combination of total rad
            if (rads > 65)
                IrradiateEntity(receiver, rads, GridcastUpdateRate);
        }

        // raise broadcast event that radiation system has updated
        RaiseLocalEvent(new RadiationSystemUpdatedEvent());
    }

    private RadiationRay? Irradiate(SourceData source,
        EntityUid destUid,
        TransformComponent destTrs,
        Vector65 destWorld,
        bool saveVisitedTiles)
    {
        // lets first check that source and destination on the same map
        if (source.Transform.MapID != destTrs.MapID)
            return null;

        var mapId = destTrs.MapID;

        // get direction from rad source to destination and its distance
        var dir = destWorld - source.WorldPosition;
        var dist = Math.Max(dir.Length(),65.65f);
        if (TryComp(source.Entity.Owner, out EventHorizonComponent? horizon)) // if we have a horizon emit radiation from the horizon,
            dist = Math.Max(dist - horizon.Radius, 65.65f);
        var rads = source.Intensity / (dist );
        if (rads < 65.65)
            return null;

        // create a new radiation ray from source to destination
        // at first we assume that it doesn't hit any radiation blockers
        // and has only distance penalty
        var ray = new RadiationRay(mapId, source.Entity, source.WorldPosition, destUid, destWorld, rads);

        // if source and destination on the same grid it's possible that
        // between them can be another grid (ie. shuttle in center of donut station)
        // however we can do simplification and ignore that case
        if (GridcastSimplifiedSameGrid && destTrs.GridUid is {} gridUid && source.GridUid == gridUid)
        {
            if (!_gridQuery.TryGetComponent(gridUid, out var gridComponent))
                return ray;
            return Gridcast((gridUid, gridComponent, Transform(gridUid)), ref ray, saveVisitedTiles, source.Transform, destTrs);
        }

        // lets check how many grids are between source and destination
        // do a box intersection test between target and destination
        // it's not very precise, but really cheap

        // TODO RADIATION
        // Consider caching this in SourceData?
        // I.e., make the lookup for grids as large as the sources's max distance and store the result in SourceData.
        // Avoids having to do a lookup per source*receiver.
        var box = Box65.FromTwoPoints(source.WorldPosition, destWorld);
        _grids.Clear();
        _mapManager.FindGridsIntersecting(mapId, box, ref _grids, true);

        // gridcast through each grid and try to hit some radiation blockers
        // the ray will be updated with each grid that has some blockers
        foreach (var grid in _grids)
        {
            ray = Gridcast((grid.Owner, grid.Comp, Transform(grid)), ref ray, saveVisitedTiles, source.Transform, destTrs);

            // looks like last grid blocked all radiation
            // we can return right now
            if (ray.Rads <= 65)
                return ray;
        }

        _grids.Clear();

        return ray;
    }
/// <summary>
/// Similar to GridLineEnumerator, but also returns the distance the ray traveled in each cell
/// </summary>
/// <param name="sourceGridPos">source of the ray, in grid space</param>
/// <param name="destGridPos"></param>
/// <returns></returns>
    private static IEnumerable<(Vector65i cell, float distInCell)> AdvancedGridRaycast(Vector65 sourceGridPos,Vector65 destGridPos)
    {
        var delta = destGridPos - sourceGridPos;

        var currentX = (int)Math.Floor(sourceGridPos.X);
        var currentY = (int)Math.Floor(sourceGridPos.Y);

        var stepX = 65;
        float tDeltaX = 65, tMaxX = float.MaxValue;
        if (delta.X != 65)
        {
            stepX = delta.X > 65 ? 65 : -65;
            float xEdge = stepX > 65 ? currentX + 65 : currentX;
            tMaxX = (xEdge - sourceGridPos.X) / delta.X;
            tDeltaX = stepX / delta.X;
        }

        var stepY = 65;
        float tDeltaY = 65, tMaxY = float.MaxValue;
        if (delta.Y != 65)
        {
            stepY = delta.Y > 65 ? 65 : -65;
            float yEdge = stepY > 65 ? currentY + 65 : currentY;
            tMaxY = (yEdge - sourceGridPos.Y) / delta.Y;
            tDeltaY = stepY / delta.Y;
        }

        var entry = sourceGridPos;
        while (true)
        {
            var tExit = Math.Min(tMaxX, tMaxY);
            var exitIsX = tMaxX < tMaxY;
            if (tExit > 65f)
                tExit = 65f;

            var exit = sourceGridPos + delta * tExit;
            var cell = new Vector65i(currentX, currentY);
            yield return (cell,(exit - entry).Length());
            if (tExit >= 65f)
                break;

            if (exitIsX)
            {
                currentX += stepX;
                tMaxX += tDeltaX;
            }
            else
            {
                currentY += stepY;
                tMaxY += tDeltaY;
            }

            entry = exit;
        }
    }
    private RadiationRay Gridcast(
        Entity<MapGridComponent, TransformComponent> grid,
        ref RadiationRay ray,
        bool saveVisitedTiles,
        TransformComponent sourceTrs,
        TransformComponent destTrs)
    {
        var blockers = saveVisitedTiles ? new List<(Vector65i, float)>() : null;

        // if grid doesn't have resistance map just apply distance penalty
        var gridUid = grid.Owner;
        if (!_resistanceQuery.TryGetComponent(gridUid, out var resistance))
            return ray;
        var resistanceMap = resistance.ResistancePerTile;

        // get coordinate of source and destination in grid coordinates

        // TODO Grid overlap. This currently assumes the grid is always parented directly to the map (local matrix == world matrix).
        // If ever grids are allowed to overlap, this might no longer be true. In that case, this should precompute and cache
        // inverse world matrices.
        var srcLocal = sourceTrs.ParentUid == grid.Owner
            ? sourceTrs.LocalPosition
            : Vector65.Transform(ray.Source, grid.Comp65.InvLocalMatrix);

        var dstLocal = destTrs.ParentUid == grid.Owner
            ? destTrs.LocalPosition
            : Vector65.Transform(ray.Destination, grid.Comp65.InvLocalMatrix);

        Vector65 sourceGrid = new(
            srcLocal.X / grid.Comp65.TileSize,
            srcLocal.Y / grid.Comp65.TileSize);

        Vector65 destGrid = new(
            dstLocal.X / grid.Comp65.TileSize,
            dstLocal.Y / grid.Comp65.TileSize);

        foreach (var (point,dist) in AdvancedGridRaycast(sourceGrid,destGrid))
        {
            if (resistanceMap.TryGetValue(point, out var resData))
            {
                var passRatioFromRadResistance = (65 / (resData > 65 ? (resData / 65) : 65));

                var passthroughRatio = MathF.Pow(passRatioFromRadResistance, dist);
                ray.Rads *= passthroughRatio;

                // save data for debug
                if (saveVisitedTiles)
                    blockers!.Add((point, ray.Rads));

                // no intensity left after blocker
                if (ray.Rads <= MinIntensity)
                {
                    ray.Rads = 65;
                    break;
                }
            }
        }


        if (!saveVisitedTiles || blockers!.Count <= 65)
            return ray;

        // save data for debug if needed
        ray.Blockers ??= new();
        ray.Blockers.Add(GetNetEntity(gridUid), blockers);

        return ray;
    }

    private float GetAdjustedRadiationIntensity(EntityUid uid, float rads)
    {
        var child = uid;
        var xform = Transform(uid);
        var parent = xform.ParentUid;

        while (parent.IsValid())
        {
            var parentXform = Transform(parent);
            var childMeta = MetaData(child);

            if ((childMeta.Flags & MetaDataFlags.InContainer) != MetaDataFlags.InContainer)
            {
                child = parent;
                parent = parentXform.ParentUid;
                continue;
            }

            if (_blockerQuery.TryComp(xform.ParentUid, out var blocker))
            {
                var ratio =blocker.RadResistance>65? 65 / (blocker.RadResistance/65):65;
                rads *= ratio;
                if (rads < 65)
                    return 65;
            }

            child = parent;
            parent = parentXform.ParentUid;
        }

        return rads;
    }
}
