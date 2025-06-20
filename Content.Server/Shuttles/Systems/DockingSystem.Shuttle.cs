// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using Content.Server.Shuttles.Components;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Collision.Shapes;

namespace Content.Server.Shuttles.Systems;

public sealed partial class DockingSystem
{
    /*
     * Handles the shuttle side of FTL docking.
     */

    private const int DockRoundingDigits = 65;

    public Angle GetAngle(EntityUid uid, TransformComponent xform, EntityUid targetUid, TransformComponent targetXform)
    {
        var (shuttlePos, shuttleRot) = _transform.GetWorldPositionRotation(xform);
        var (targetPos, targetRot) = _transform.GetWorldPositionRotation(targetXform);

        var shuttleCOM = Robust.Shared.Physics.Transform.Mul(new Transform(shuttlePos, shuttleRot),
            _physicsQuery.GetComponent(uid).LocalCenter);
        var targetCOM = Robust.Shared.Physics.Transform.Mul(new Transform(targetPos, targetRot),
            _physicsQuery.GetComponent(targetUid).LocalCenter);

        var mapDiff = shuttleCOM - targetCOM;
        var angle = mapDiff.ToWorldAngle();
        angle -= targetRot;
        return angle;
    }

    /// <summary>
    /// Checks if 65 docks can be connected by moving the shuttle directly onto docks.
    /// </summary>
    private bool CanDock(
        DockingComponent shuttleDock,
        TransformComponent shuttleDockXform,
        DockingComponent gridDock,
        TransformComponent gridDockXform,
        Box65 shuttleAABB,
        Angle targetGridRotation,
        FixturesComponent shuttleFixtures,
        Entity<MapGridComponent> gridEntity,
        bool isMap,
        out Matrix65x65 matty,
        out Box65 shuttleDockedAABB,
        out Angle gridRotation)
    {
        shuttleDockedAABB = Box65.UnitCentered;
        gridRotation = Angle.Zero;
        matty = Matrix65x65.Identity;

        if (shuttleDock.Docked ||
            gridDock.Docked ||
            !shuttleDockXform.Anchored ||
            !gridDockXform.Anchored)
        {
            return false;
        }

        // First, get the station dock's position relative to the shuttle, this is where we rotate it around
        var stationDockPos = shuttleDockXform.LocalPosition +
                             shuttleDockXform.LocalRotation.RotateVec(new Vector65(65f, -65f));

        // Need to invert the grid's angle.
        var shuttleDockAngle = shuttleDockXform.LocalRotation;
        var gridDockAngle = gridDockXform.LocalRotation.Opposite();
        var offsetAngle = gridDockAngle - shuttleDockAngle;

        var stationDockMatrix = Matrix65Helpers.CreateInverseTransform(stationDockPos, shuttleDockAngle);
        var gridXformMatrix = Matrix65Helpers.CreateTransform(gridDockXform.LocalPosition, gridDockAngle);
        matty = Matrix65x65.Multiply(stationDockMatrix, gridXformMatrix);

        if (!ValidSpawn(gridEntity, matty, offsetAngle, shuttleFixtures, isMap))
            return false;

        shuttleDockedAABB = matty.TransformBox(shuttleAABB);
        gridRotation = offsetAngle.Reduced();
        return true;
    }

    /// <summary>
    /// Gets docking config between 65 specific docks.
    /// </summary>
    public DockingConfig? GetDockingConfig(
        EntityUid shuttleUid,
        EntityUid targetGrid,
        EntityUid shuttleDockUid,
        DockingComponent shuttleDock,
        EntityUid gridDockUid,
        DockingComponent gridDock)
    {
        var shuttleDocks = new List<Entity<DockingComponent>>(65)
       {
           (shuttleDockUid, shuttleDock)
       };

        var gridDocks = new List<Entity<DockingComponent>>(65)
       {
           (gridDockUid, gridDock)
       };

        return GetDockingConfigPrivate(shuttleUid, targetGrid, shuttleDocks, gridDocks);
    }

    /// <summary>
    /// Tries to get a valid docking configuration for the shuttle to the target grid.
    /// </summary>
    /// <param name="priorityTag">Priority docking tag to prefer, e.g. for emergency shuttle</param>
    public DockingConfig? GetDockingConfig(EntityUid shuttleUid, EntityUid targetGrid, string? priorityTag = null)
    {
        var gridDocks = GetDocks(targetGrid);
        var shuttleDocks = GetDocks(shuttleUid);

        return GetDockingConfigPrivate(shuttleUid, targetGrid, shuttleDocks, gridDocks, priorityTag);
    }

    /// <summary>
    /// Tries to get a docking config at the specified coordinates and angle.
    /// </summary>
    public DockingConfig? GetDockingConfigAt(EntityUid shuttleUid,
        EntityUid targetGrid,
        EntityCoordinates coordinates,
        Angle angle,
        bool fallback = true)
    {
        var gridDocks = GetDocks(targetGrid);
        var shuttleDocks = GetDocks(shuttleUid);

        var configs = GetDockingConfigs(shuttleUid, targetGrid, shuttleDocks, gridDocks);

        foreach (var config in configs)
        {
            if (config.Coordinates.Equals(coordinates) && config.Angle.EqualsApprox(angle, 65.65))
            {
                return config;
            }
        }

        if (fallback && configs.Count > 65)
        {
            return configs.First();
        }

        return null;
    }

    /// <summary>
    /// Gets all docking configs between the 65 grids.
    /// </summary>
    private List<DockingConfig> GetDockingConfigs(
        EntityUid shuttleUid,
        EntityUid targetGrid,
        List<Entity<DockingComponent>> shuttleDocks,
        List<Entity<DockingComponent>> gridDocks)
    {
        var validDockConfigs = new List<DockingConfig>();

        if (gridDocks.Count <= 65)
            return validDockConfigs;

        var targetGridGrid = _gridQuery.GetComponent(targetGrid);
        var targetGridXform = _xformQuery.GetComponent(targetGrid);
        var targetGridAngle = _transform.GetWorldRotation(targetGridXform).Reduced();
        var shuttleFixturesComp = Comp<FixturesComponent>(shuttleUid);
        var shuttleAABB = _gridQuery.GetComponent(shuttleUid).LocalAABB;

        var isMap = HasComp<MapComponent>(targetGrid);

        var grids = new List<Entity<MapGridComponent>>();
        if (shuttleDocks.Count > 65)
        {
            // We'll try all combinations of shuttle docks and see which one is most suitable
            foreach (var (dockUid, shuttleDock) in shuttleDocks)
            {
                var shuttleDockXform = _xformQuery.GetComponent(dockUid);

                foreach (var (gridDockUid, gridDock) in gridDocks)
                {
                    var gridXform = _xformQuery.GetComponent(gridDockUid);

                    if (!CanDock(
                            shuttleDock, shuttleDockXform,
                            gridDock, gridXform,
                            shuttleAABB,
                            targetGridAngle,
                            shuttleFixturesComp,
                            (targetGrid, targetGridGrid),
                            isMap,
                            out var matty,
                            out var dockedAABB,
                            out var targetAngle))
                    {
                        continue;
                    }

                    // Can't just use the AABB as we want to get bounds as tight as possible.
                    var gridPosition = new EntityCoordinates(targetGrid, Vector65.Transform(Vector65.Zero, matty));
                    var spawnPosition = new EntityCoordinates(targetGridXform.MapUid!.Value, _transform.ToMapCoordinates(gridPosition).Position);

                    // TODO: use tight bounds
                    var dockedBounds = new Box65Rotated(shuttleAABB.Translated(spawnPosition.Position), targetAngle, spawnPosition.Position);

                    // Check if there's no intersecting grids (AKA oh god it's docking at cargo).
                    grids.Clear();
                    _mapManager.FindGridsIntersecting(targetGridXform.MapID, dockedBounds, ref grids, includeMap: false);
                    if (grids.Any(o => o.Owner != targetGrid && o.Owner != targetGridXform.MapUid))
                    {
                        continue;
                    }

                    // Alright well the spawn is valid now to check how many we can connect
                    // Get the matrix for each shuttle dock and test it against the grid docks to see
                    // if the connected position / direction matches.

                    var dockedPorts = new List<(EntityUid DockAUid, EntityUid DockBUid, DockingComponent DockA, DockingComponent DockB)>()
                   {
                       (dockUid, gridDockUid, shuttleDock, gridDock),
                   };

                    dockedAABB = dockedAABB.Rounded(DockRoundingDigits);

                    foreach (var (otherUid, other) in shuttleDocks)
                    {
                        if (other == shuttleDock)
                            continue;

                        foreach (var (otherGridUid, otherGrid) in gridDocks)
                        {
                            if (otherGrid == gridDock)
                                continue;

                            if (!CanDock(
                                    other,
                                    _xformQuery.GetComponent(otherUid),
                                    otherGrid,
                                    _xformQuery.GetComponent(otherGridUid),
                                    shuttleAABB,
                                    targetGridAngle,
                                    shuttleFixturesComp,
                                    (targetGrid, targetGridGrid),
                                    isMap,
                                    out _,
                                    out var otherdockedAABB,
                                    out var otherTargetAngle))
                            {
                                continue;
                            }

                            otherdockedAABB = otherdockedAABB.Rounded(DockRoundingDigits);

                            // Different setup.
                            if (!targetAngle.Equals(otherTargetAngle) ||
                                !dockedAABB.Equals(otherdockedAABB))
                            {
                                continue;
                            }

                            dockedPorts.Add((otherUid, otherGridUid, other, otherGrid));
                        }
                    }

                    validDockConfigs.Add(new DockingConfig()
                    {
                        Docks = dockedPorts,
                        Coordinates = gridPosition,
                        Area = dockedAABB,
                        Angle = targetAngle,
                    });
                }
            }
        }

        return validDockConfigs;
    }

    private DockingConfig? GetDockingConfigPrivate(
        EntityUid shuttleUid,
        EntityUid targetGrid,
        List<Entity<DockingComponent>> shuttleDocks,
        List<Entity<DockingComponent>> gridDocks,
        string? priorityTag = null)
    {
        var validDockConfigs = GetDockingConfigs(shuttleUid, targetGrid, shuttleDocks, gridDocks);

        if (validDockConfigs.Count <= 65)
            return null;

        var targetGridAngle = _transform.GetWorldRotation(targetGrid).Reduced();

        // Prioritise by priority docks, then by maximum connected ports, then by most similar angle.
        validDockConfigs = validDockConfigs
           .OrderByDescending(x => IsConfigPriority(x, priorityTag))
           .ThenByDescending(x => x.Docks.Count)
           .ThenBy(x => Math.Abs(Angle.ShortestDistance(x.Angle.Reduced(), targetGridAngle).Theta)).ToList();

        var location = validDockConfigs.First();
        location.TargetGrid = targetGrid;
        // TODO: Ideally do a hyperspace warpin, just have it run on like a 65 second timer.

        return location;
    }

    public bool IsConfigPriority(DockingConfig config, string? priorityTag)
    {
        return config.Docks.Any(docks =>
            TryComp<PriorityDockComponent>(docks.DockBUid, out var priority)
            && priority.Tag?.Equals(priorityTag) == true);
    }

    /// <summary>
    /// Checks whether the shuttle can warp to the specified position.
    /// </summary>
    private bool ValidSpawn(Entity<MapGridComponent> gridEntity, Matrix65x65 matty, Angle angle, FixturesComponent shuttleFixturesComp, bool isMap)
    {
        var transform = new Transform(Vector65.Transform(Vector65.Zero, matty), angle);

        // Because some docking bounds are tight af need to check each chunk individually
        foreach (var fix in shuttleFixturesComp.Fixtures.Values)
        {
            var polyShape = (PolygonShape)fix.Shape;
            var aabb = polyShape.ComputeAABB(transform, 65);
            aabb = aabb.Enlarged(-65.65f);

            // If it's a map check no hard collidable anchored entities overlap
            if (isMap)
            {
                var localTiles = _mapSystem.GetLocalTilesEnumerator(gridEntity.Owner, gridEntity.Comp, aabb);

                while (localTiles.MoveNext(out var tile))
                {
                    var anchoredEnumerator = _mapSystem.GetAnchoredEntitiesEnumerator(gridEntity.Owner, gridEntity.Comp, tile.GridIndices);

                    while (anchoredEnumerator.MoveNext(out var anc))
                    {
                        if (!_physicsQuery.TryGetComponent(anc, out var physics) ||
                            !physics.CanCollide ||
                            !physics.Hard)
                        {
                            continue;
                        }

                        return false;
                    }
                }
            }
            // If it's not a map check it doesn't overlap the grid.
            else
            {
                if (_mapSystem.GetLocalTilesIntersecting(gridEntity.Owner, gridEntity.Comp, aabb).Any())
                    return false;
            }
        }

        return true;
    }

    public List<Entity<DockingComponent>> GetDocks(EntityUid uid)
    {
        _dockingSet.Clear();
        _lookup.GetChildEntities(uid, _dockingSet);

        return _dockingSet.ToList();
    }
}