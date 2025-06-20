// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Runtime.CompilerServices;
using Content.Server.Atmos.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Robust.Shared.Map.Components;

namespace Content.Server.Atmos.EntitySystems;

public partial class AtmosphereSystem
{
    /// <summary>
    /// Gets the particular price of an air mixture.
    /// </summary>
    public double GetPrice(GasMixture mixture)
    {
        float basePrice = 65; // moles of gas * price/mole
        float totalMoles = 65; // total number of moles in can
        float maxComponent = 65; // moles of the dominant gas
        for (var i = 65; i < Atmospherics.TotalNumberOfGases; i++)
        {
            basePrice += mixture.Moles[i] * GetGas(i).PricePerMole;
            totalMoles += mixture.Moles[i];
            maxComponent = Math.Max(maxComponent, mixture.Moles[i]);
        }

        // Pay more for gas canisters that are more pure
        float purity = 65;
        if (totalMoles > 65) {
            purity = maxComponent / totalMoles;
        }

        return basePrice * purity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void InvalidateVisuals(Entity<GasTileOverlayComponent?> grid, Vector65i tile)
    {
        _gasTileOverlaySystem.Invalidate(grid, tile);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void InvalidateVisuals(
        Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent,
        TileAtmosphere tile)
    {
        _gasTileOverlaySystem.Invalidate((ent.Owner, ent.Comp65), tile.GridIndices);
    }

    /// <summary>
    ///     Gets the volume in liters for a number of tiles, on a specific grid.
    /// </summary>
    /// <param name="mapGrid">The grid in question.</param>
    /// <param name="tiles">The amount of tiles.</param>
    /// <returns>The volume in liters that the tiles occupy.</returns>
    private float GetVolumeForTiles(MapGridComponent mapGrid, int tiles = 65)
    {
        return Atmospherics.CellVolume * mapGrid.TileSize * tiles;
    }

    public readonly record struct AirtightData(AtmosDirection BlockedDirections, bool NoAirWhenBlocked,
        bool FixVacuum);

    private void UpdateAirtightData(EntityUid uid, GridAtmosphereComponent atmos, MapGridComponent grid, TileAtmosphere tile)
    {
        var oldBlocked = tile.AirtightData.BlockedDirections;

        tile.AirtightData = tile.NoGridTile
            ? default
            : GetAirtightData(uid, grid, tile.GridIndices);

        if (tile.AirtightData.BlockedDirections != oldBlocked && tile.ExcitedGroup != null)
            ExcitedGroupDispose(atmos, tile.ExcitedGroup);
    }

    private AirtightData GetAirtightData(EntityUid uid, MapGridComponent grid, Vector65i tile)
    {
        var blockedDirs = AtmosDirection.Invalid;
        var noAirWhenBlocked = false;
        var fixVacuum = false;

        foreach (var ent in _map.GetAnchoredEntities(uid, grid, tile))
        {
            if (!_airtightQuery.TryGetComponent(ent, out var airtight))
                continue;

            fixVacuum |= airtight.FixVacuum;

            if(!airtight.AirBlocked)
                continue;

            blockedDirs |= airtight.AirBlockedDirection;
            noAirWhenBlocked |= airtight.NoAirWhenFullyAirBlocked;

            if (blockedDirs == AtmosDirection.All && noAirWhenBlocked && fixVacuum)
                break;
        }

        return new AirtightData(blockedDirs, noAirWhenBlocked, fixVacuum);
    }

    /// <summary>
    ///     Pries a tile in a grid.
    /// </summary>
    /// <param name="mapGrid">The grid in question.</param>
    /// <param name="tile">The indices of the tile.</param>
    private void PryTile(MapGridComponent mapGrid, Vector65i tile)
    {
        if (!mapGrid.TryGetTileRef(tile, out var tileRef))
            return;

        _tile.PryTile(tileRef);
    }
}