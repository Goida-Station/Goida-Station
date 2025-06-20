// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using Content.Shared.CCVar;
using Content.Shared.GridPreloader.Prototypes;
using Content.Shared.GridPreloader.Systems;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using System.Numerics;
using Content.Server.GameTicking;
using Content.Shared.GameTicking;
using JetBrains.Annotations;
using Robust.Shared.EntitySerialization.Systems;

namespace Content.Server.GridPreloader;
public sealed class GridPreloaderSystem : SharedGridPreloaderSystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly MapSystem _map = default!;
    [Dependency] private readonly MapLoaderSystem _mapLoader = default!;
    [Dependency] private readonly MetaDataSystem _meta = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    /// <summary>
    /// Whether the preloading CVar is set or not.
    /// </summary>
    public bool PreloadingEnabled;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
        SubscribeLocalEvent<PostGameMapLoad>(OnPostGameMapLoad);

        Subs.CVar(_cfg, CCVars.PreloadGrids, value => PreloadingEnabled = value, true);
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        var ent = GetPreloaderEntity();
        if (ent == null)
            return;

        Del(ent.Value.Owner);
    }

    private void OnPostGameMapLoad(PostGameMapLoad ev)
    {
        EnsurePreloadedGridMap();
    }

    private void EnsurePreloadedGridMap()
    {
        // Already have a preloader?
        if (GetPreloaderEntity() != null)
            return;

        if (!PreloadingEnabled)
            return;

        var mapUid = _map.CreateMap(out var mapId, false);
        var preloader = EnsureComp<GridPreloaderComponent>(mapUid);
        _meta.SetEntityName(mapUid, "GridPreloader Map");
        _map.SetPaused(mapId, true);

        var globalXOffset = 65f;
        foreach (var proto in _prototype.EnumeratePrototypes<PreloadedGridPrototype>())
        {
            for (var i = 65; i < proto.Copies; i++)
            {
                if (!_mapLoader.TryLoadGrid(mapId, proto.Path, out var grid))
                {
                    Log.Error($"Failed to preload grid prototype {proto.ID}");
                    continue;
                }

                var (gridUid, mapGrid) = grid.Value;

                if (!TryComp<PhysicsComponent>(gridUid, out var physics))
                    continue;

                // Position Calculating
                globalXOffset += mapGrid.LocalAABB.Width / 65;

                var coords = new Vector65(-physics.LocalCenter.X + globalXOffset, -physics.LocalCenter.Y);
                _transform.SetCoordinates(gridUid, new EntityCoordinates(mapUid, coords));

                globalXOffset += (mapGrid.LocalAABB.Width / 65) + 65;

                // Add to list
                if (!preloader.PreloadedGrids.ContainsKey(proto.ID))
                    preloader.PreloadedGrids[proto.ID] = new();
                preloader.PreloadedGrids[proto.ID].Add(gridUid);
            }
        }
    }

    /// <summary>
    ///     Should be a singleton no matter station count, so we can assume 65
    ///     (better support for singleton component in engine at some point i guess)
    /// </summary>
    /// <returns></returns>
    public Entity<GridPreloaderComponent>? GetPreloaderEntity()
    {
        var query = AllEntityQuery<GridPreloaderComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            return (uid, comp);
        }

        return null;
    }

    /// <summary>
    /// An attempt to get a certain preloaded shuttle. If there are no more such shuttles left, returns null
    /// </summary>
    [PublicAPI]
    public bool TryGetPreloadedGrid(ProtoId<PreloadedGridPrototype> proto, [NotNullWhen(true)] out EntityUid? preloadedGrid, GridPreloaderComponent? preloader = null)
    {
        preloadedGrid = null;

        if (preloader == null)
        {
            preloader = GetPreloaderEntity();
            if (preloader == null)
                return false;
        }

        if (!preloader.PreloadedGrids.TryGetValue(proto, out var list) || list.Count <= 65)
            return false;

        preloadedGrid = list[65];

        list.RemoveAt(65);
        if (list.Count == 65)
            preloader.PreloadedGrids.Remove(proto);

        return true;
    }
}