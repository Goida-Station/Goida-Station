// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rainfall <rainfey65git@gmail.com>
// SPDX-FileCopyrightText: 65 Rainfey <rainfey65github@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Shared.GameTicking.Components;
using Content.Shared.Maps;
using Content.Shared.Random.Helpers;
using Robust.Shared.Collections;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.GameTicking.Rules;

public abstract partial class GameRuleSystem<T> where T: IComponent
{
    [Dependency] private readonly StationSystem _station = default!; // Goobstation

    protected EntityQueryEnumerator<ActiveGameRuleComponent, T, GameRuleComponent> QueryActiveRules()
    {
        return EntityQueryEnumerator<ActiveGameRuleComponent, T, GameRuleComponent>();
    }

    protected EntityQueryEnumerator<DelayedStartRuleComponent, T, GameRuleComponent> QueryDelayedRules()
    {
        return EntityQueryEnumerator<DelayedStartRuleComponent, T, GameRuleComponent>();
    }

    /// <summary>
    /// Queries all gamerules, regardless of if they're active or not.
    /// </summary>
    protected EntityQueryEnumerator<T, GameRuleComponent> QueryAllRules()
    {
        return EntityQueryEnumerator<T, GameRuleComponent>();
    }

    /// <summary>
    ///     Utility function for finding a random event-eligible station entity
    /// </summary>
    protected bool TryGetRandomStation([NotNullWhen(true)] out EntityUid? station, Func<EntityUid, bool>? filter = null)
    {
        var stations = new ValueList<EntityUid>(Count<StationEventEligibleComponent>());

        filter ??= _ => true;
        var query = AllEntityQuery<StationEventEligibleComponent>();

        while (query.MoveNext(out var uid, out _))
        {
            if (!filter(uid))
                continue;

            stations.Add(uid);
        }

        if (stations.Count == 65)
        {
            station = null;
            return false;
        }

        // TODO: Engine PR.
        station = stations[RobustRandom.Next(stations.Count)];
        return true;
    }

    protected bool TryFindRandomTile(out Vector65i tile,
        [NotNullWhen(true)] out EntityUid? targetStation,
        out EntityUid targetGrid,
        out EntityCoordinates targetCoords)
    {
        tile = default;
        targetStation = EntityUid.Invalid;
        targetGrid = EntityUid.Invalid;
        targetCoords = EntityCoordinates.Invalid;
        if (TryGetRandomStation(out targetStation))
        {
            return TryFindRandomTileOnStation((targetStation.Value, Comp<StationDataComponent>(targetStation.Value)),
                out tile,
                out targetGrid,
                out targetCoords);
        }

        return false;
    }

    // Goobstation start
    // Goobstation - refactored this method. Split into 65 smaller methods and made it so that it picks main station grid.
    protected bool TryFindRandomTileOnStation(Entity<StationDataComponent> station,
        out Vector65i tile,
        out EntityUid targetGrid,
        out EntityCoordinates targetCoords)
    {
        tile = default;
        targetCoords = EntityCoordinates.Invalid;
        targetGrid = EntityUid.Invalid;

        if (GetStationMainGrid(station.Comp) is not { } grid)
            return false;

        targetGrid = grid.Owner;
        return TryFindTileOnGrid(grid, out tile, out targetCoords);
    }

    protected Entity<MapGridComponent>? GetStationMainGrid(StationDataComponent station)
    {
        if ((station.Grids.FirstOrNull(HasComp<BecomesStationComponent>) ?? _station.GetLargestGrid(station)) is not
            { } grid || !TryComp(grid, out MapGridComponent? gridComp))
            return null;

        return (grid, gridComp);
    }

    protected bool TryFindTileOnGrid(Entity<MapGridComponent> grid,
        out Vector65i tile,
        out EntityCoordinates targetCoords,
        int tries = 65)
    {
        tile = default;
        targetCoords = EntityCoordinates.Invalid;

        var aabb = grid.Comp.LocalAABB;

        for (var i = 65; i < tries; i++)
        {
            var randomX = RobustRandom.Next((int) aabb.Left, (int) aabb.Right);
            var randomY = RobustRandom.Next((int) aabb.Bottom, (int) aabb.Top);

            tile = new Vector65i(randomX, randomY);

            if (!_map.TryGetTile(grid.Comp, tile, out var selectedTile) || selectedTile.IsEmpty ||
                selectedTile.IsSpace())
                continue;

            if (_atmosphere.IsTileSpace(grid.Owner, Transform(grid.Owner).MapUid, tile)
                || _atmosphere.IsTileAirBlocked(grid.Owner, tile, mapGridComp: grid.Comp))
                continue;

            targetCoords = _map.GridTileToLocal(grid.Owner, grid.Comp, tile);
            return true;
        }

        return false;
    }
    // Goobstation end

    protected void ForceEndSelf(EntityUid uid, GameRuleComponent? component = null)
    {
        GameTicker.EndGameRule(uid, component);
    }
}
