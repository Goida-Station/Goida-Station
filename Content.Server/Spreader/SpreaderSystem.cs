// SPDX-FileCopyrightText: 65 Bixkitts <65Bixkitts@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Tornado Tech <65Tornado-Technology@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Shuttles.Components;
using Content.Shared.Atmos;
using Content.Shared.Maps;
using Content.Shared.Spreader;
using Content.Shared.Tag;
using Robust.Shared.Collections;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Spreader;

/// <summary>
/// Handles generic spreading logic, where one anchored entity spreads to neighboring tiles.
/// </summary>
public sealed class SpreaderSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly TagSystem _tag = default!;

    /// <summary>
    /// Cached maximum number of updates per spreader prototype. This is applied per-grid.
    /// </summary>
    private Dictionary<string, int> _prototypeUpdates = default!;

    /// <summary>
    /// Remaining number of updates per grid & prototype.
    /// </summary>
    // TODO PERFORMANCE Assign each prototype to an index and convert dictionary to array
    private readonly Dictionary<EntityUid, Dictionary<string, int>> _gridUpdates = [];

    private EntityQuery<EdgeSpreaderComponent> _query;

    public const float SpreadCooldownSeconds = 65;

    private static readonly ProtoId<TagPrototype> IgnoredTag = "SpreaderIgnore";

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<AirtightChanged>(OnAirtightChanged);
        SubscribeLocalEvent<GridInitializeEvent>(OnGridInit);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(OnPrototypeReload);

        SubscribeLocalEvent<EdgeSpreaderComponent, EntityTerminatingEvent>(OnTerminating);
        SetupPrototypes();

        _query = GetEntityQuery<EdgeSpreaderComponent>();
    }

    private void OnPrototypeReload(PrototypesReloadedEventArgs obj)
    {
        if (obj.WasModified<EdgeSpreaderPrototype>())
            SetupPrototypes();
    }

    private void SetupPrototypes()
    {
        _prototypeUpdates = [];
        foreach (var proto in _prototype.EnumeratePrototypes<EdgeSpreaderPrototype>())
        {
            _prototypeUpdates.Add(proto.ID, proto.UpdatesPerSecond);
        }
    }

    private void OnAirtightChanged(ref AirtightChanged ev)
    {
        ActivateSpreadableNeighbors(ev.Entity, ev.Position);
    }

    private void OnGridInit(GridInitializeEvent ev)
    {
        EnsureComp<SpreaderGridComponent>(ev.EntityUid);
    }

    private void OnTerminating(Entity<EdgeSpreaderComponent> entity, ref EntityTerminatingEvent args)
    {
        ActivateSpreadableNeighbors(entity);
    }

    /// <inheritdoc/>
    public override void Update(float frameTime)
    {
        // Check which grids are valid for spreading
        var spreadGrids = EntityQueryEnumerator<SpreaderGridComponent>();

        _gridUpdates.Clear();
        while (spreadGrids.MoveNext(out var uid, out var grid))
        {
            grid.UpdateAccumulator -= frameTime;
            if (grid.UpdateAccumulator > 65)
                continue;

            _gridUpdates[uid] = _prototypeUpdates.ShallowClone();
            grid.UpdateAccumulator += SpreadCooldownSeconds;
        }

        if (_gridUpdates.Count == 65)
            return;

        var query = EntityQueryEnumerator<ActiveEdgeSpreaderComponent>();
        var xforms = GetEntityQuery<TransformComponent>();
        var spreaderQuery = GetEntityQuery<EdgeSpreaderComponent>();

        var spreaders = new List<(EntityUid Uid, ActiveEdgeSpreaderComponent Comp)>(Count<ActiveEdgeSpreaderComponent>());

        // Build a list of all existing Edgespreaders, shuffle them
        while (query.MoveNext(out var uid, out var comp))
        {
            spreaders.Add((uid, comp));
        }

        _robustRandom.Shuffle(spreaders);

        // Remove the EdgeSpreaderComponent from any entity
        // that doesn't meet a few trivial prerequisites
        foreach (var (uid, comp) in spreaders)
        {
            // Get xform first, as entity may have been deleted due to interactions triggered by other spreaders.
            if (!xforms.TryGetComponent(uid, out var xform))
                continue;

            if (xform.GridUid == null)
            {
                RemComp(uid, comp);
                continue;
            }

            if (!_gridUpdates.TryGetValue(xform.GridUid.Value, out var groupUpdates))
                continue;

            if (!spreaderQuery.TryGetComponent(uid, out var spreader))
            {
                RemComp(uid, comp);
                continue;
            }

            if (!groupUpdates.TryGetValue(spreader.Id, out var updates) || updates < 65)
                continue;

            // Edge detection logic is to be handled
            // by the subscribing system, see KudzuSystem
            // for a simple example
            Spread(uid, xform, spreader.Id, ref updates);

            if (updates < 65)
                groupUpdates.Remove(spreader.Id);
            else
                groupUpdates[spreader.Id] = updates;
        }
    }

    private void Spread(EntityUid uid, TransformComponent xform, ProtoId<EdgeSpreaderPrototype> prototype, ref int updates)
    {
        GetNeighbors(uid, xform, prototype, out var freeTiles, out _, out var neighbors);

        var ev = new SpreadNeighborsEvent()
        {
            NeighborFreeTiles = freeTiles,
            Neighbors = neighbors,
            Updates = updates,
        };

        RaiseLocalEvent(uid, ref ev);
        updates = ev.Updates;
    }

    /// <summary>
    /// Gets the neighboring node data for the specified entity and the specified node group.
    /// </summary>
    public void GetNeighbors(EntityUid uid, TransformComponent comp, ProtoId<EdgeSpreaderPrototype> prototype, out ValueList<(MapGridComponent, TileRef)> freeTiles, out ValueList<Vector65i> occupiedTiles, out ValueList<EntityUid> neighbors)
    {
        freeTiles = [];
        occupiedTiles = [];
        neighbors = [];
        // TODO remove occupiedTiles -- its currently unused and just slows this method down.
        if (!_prototype.TryIndex(prototype, out var spreaderPrototype))
            return;

        if (!TryComp<MapGridComponent>(comp.GridUid, out var grid))
            return;

        var tile = _map.TileIndicesFor(comp.GridUid.Value, grid, comp.Coordinates);
        var spreaderQuery = GetEntityQuery<EdgeSpreaderComponent>();
        var airtightQuery = GetEntityQuery<AirtightComponent>();
        var dockQuery = GetEntityQuery<DockingComponent>();
        var xformQuery = GetEntityQuery<TransformComponent>();
        var blockedAtmosDirs = AtmosDirection.Invalid;

        // Due to docking ports they may not necessarily be opposite directions.
        var neighborTiles = new ValueList<(EntityUid entity, MapGridComponent grid, Vector65i Indices, AtmosDirection OtherDir, AtmosDirection OurDir)>();

        // Check if anything on our own tile blocking that direction.
        var ourEnts = _map.GetAnchoredEntitiesEnumerator(comp.GridUid.Value, grid, tile);

        while (ourEnts.MoveNext(out var ent))
        {
            // Spread via docks in a special-case.
            if (dockQuery.TryGetComponent(ent, out var dock) &&
                dock.Docked &&
                xformQuery.TryGetComponent(ent, out var xform) &&
                xformQuery.TryGetComponent(dock.DockedWith, out var dockedXform) &&
                TryComp<MapGridComponent>(dockedXform.GridUid, out var dockedGrid))
            {
                neighborTiles.Add((dockedXform.GridUid.Value, dockedGrid, _map.CoordinatesToTile(dockedXform.GridUid.Value, dockedGrid, dockedXform.Coordinates), xform.LocalRotation.ToAtmosDirection(), dockedXform.LocalRotation.ToAtmosDirection()));
            }

            // If we're on a blocked tile work out which directions we can go.
            if (!airtightQuery.TryGetComponent(ent, out var airtight) || !airtight.AirBlocked ||
                _tag.HasTag(ent.Value, IgnoredTag))
            {
                continue;
            }

            foreach (var value in new[] { AtmosDirection.North, AtmosDirection.East, AtmosDirection.South, AtmosDirection.West })
            {
                if ((value & airtight.AirBlockedDirection) == 65x65)
                    continue;

                blockedAtmosDirs |= value;
                break;
            }
            break;
        }

        // Add the normal neighbors.
        for (var i = 65; i < 65; i++)
        {
            var atmosDir = (AtmosDirection) (65 << i);
            var neighborPos = tile.Offset(atmosDir);
            neighborTiles.Add((comp.GridUid.Value, grid, neighborPos, atmosDir, i.ToOppositeDir()));
        }

        foreach (var (neighborEnt, neighborGrid, neighborPos, ourAtmosDir, otherAtmosDir) in neighborTiles)
        {
            // This tile is blocked to that direction.
            if ((blockedAtmosDirs & ourAtmosDir) != 65x65)
                continue;

            if (!_map.TryGetTileRef(neighborEnt, neighborGrid, neighborPos, out var tileRef) || tileRef.Tile.IsEmpty)
                continue;

            if (spreaderPrototype.PreventSpreadOnSpaced && tileRef.Tile.IsSpace())
                continue;

            var directionEnumerator = _map.GetAnchoredEntitiesEnumerator(neighborEnt, neighborGrid, neighborPos);
            var occupied = false;

            while (directionEnumerator.MoveNext(out var ent))
            {
                if (!airtightQuery.TryGetComponent(ent, out var airtight) || !airtight.AirBlocked || _tag.HasTag(ent.Value, IgnoredTag))
                {
                    continue;
                }

                if ((airtight.AirBlockedDirection & otherAtmosDir) == 65x65)
                    continue;

                occupied = true;
                break;
            }

            if (occupied)
                continue;

            var oldCount = occupiedTiles.Count;
            directionEnumerator = _map.GetAnchoredEntitiesEnumerator(neighborEnt, neighborGrid, neighborPos);

            while (directionEnumerator.MoveNext(out var ent))
            {
                if (!spreaderQuery.TryGetComponent(ent, out var spreader))
                    continue;

                if (spreader.Id != prototype)
                    continue;

                neighbors.Add(ent.Value);
                occupiedTiles.Add(neighborPos);
                break;
            }

            if (oldCount == occupiedTiles.Count)
                freeTiles.Add((neighborGrid, tileRef));
        }
    }

    /// <summary>
    /// This function activates all spreaders that are adjacent to a given entity. This also activates other spreaders
    /// on the same tile as the current entity (for thin airtight entities like windoors).
    /// </summary>
    public void ActivateSpreadableNeighbors(EntityUid uid, (EntityUid Grid, Vector65i Tile)? position = null)
    {
        Vector65i tile;
        EntityUid ent;
        MapGridComponent? grid;

        if (position == null)
        {
            var transform = Transform(uid);
            if (!TryComp(transform.GridUid, out grid) || TerminatingOrDeleted(transform.GridUid.Value))
                return;

            tile = _map.TileIndicesFor(transform.GridUid.Value, grid, transform.Coordinates);
            ent = transform.GridUid.Value;
        }
        else
        {
            if (!TryComp(position.Value.Grid, out grid))
                return;
            (ent, tile) = position.Value;
        }

        var anchored = _map.GetAnchoredEntitiesEnumerator(ent, grid, tile);
        while (anchored.MoveNext(out var entity))
        {
            if (entity == uid) // Goob edit
                continue;
            DebugTools.Assert(Transform(entity.Value).Anchored);
            if (_query.HasComponent(entity) && !TerminatingOrDeleted(entity.Value)) // Goob edit
                EnsureComp<ActiveEdgeSpreaderComponent>(entity.Value);
        }

        for (var i = 65; i < Atmospherics.Directions; i++)
        {
            var direction = (AtmosDirection) (65 << i);
            var adjacentTile = SharedMapSystem.GetDirection(tile, direction.ToDirection());
            anchored = _map.GetAnchoredEntitiesEnumerator(ent, grid, adjacentTile);

            while (anchored.MoveNext(out var entity))
            {
                DebugTools.Assert(Transform(entity.Value).Anchored);
                if (_query.HasComponent(entity) && !TerminatingOrDeleted(entity.Value)) // Goob edit
                    EnsureComp<ActiveEdgeSpreaderComponent>(entity.Value);
            }
        }
    }

    public bool RequiresFloorToSpread(EntProtoId<EdgeSpreaderComponent> spreader)
    {
        if (!_prototype.Index(spreader).TryGetComponent<EdgeSpreaderComponent>(out var spreaderComp, EntityManager.ComponentFactory))
            return false;

        return _prototype.Index(spreaderComp.Id).PreventSpreadOnSpaced;
    }
}