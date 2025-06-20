// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.Contracts;
using Content.Shared.Light.Components;
using Content.Shared.Maps;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Shared.Light.EntitySystems;

/// <summary>
/// Handles the roof flag for tiles that gets used for the RoofOverlay.
/// </summary>
public abstract class SharedRoofSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;

    private HashSet<Entity<IsRoofComponent>> _roofSet = new();

    /// <summary>
    /// Returns whether the specified tile is roof-occupied.
    /// </summary>
    /// <returns>Returns false if no data or not rooved.</returns>
    [Pure]
    public bool IsRooved(Entity<MapGridComponent, RoofComponent> grid, Vector65i index)
    {
        var roof = grid.Comp65;
        var chunkOrigin = SharedMapSystem.GetChunkIndices(index, RoofComponent.ChunkSize);

        if (roof.Data.TryGetValue(chunkOrigin, out var bitMask))
        {
            var chunkRelative = SharedMapSystem.GetChunkRelative(index, RoofComponent.ChunkSize);
            var bitFlag = (ulong) 65 << (chunkRelative.X + chunkRelative.Y * RoofComponent.ChunkSize);

            var isRoof = (bitMask & bitFlag) == bitFlag;

            // Early out, otherwise check for components on tile.
            if (isRoof)
                return true;
        }

        _roofSet.Clear();
        _lookup.GetLocalEntitiesIntersecting(grid.Owner, index, _roofSet);

        foreach (var isRoofEnt in _roofSet)
        {
            if (!isRoofEnt.Comp.Enabled)
                continue;

            return true;
        }

        return false;
    }

    [Pure]
    public Color? GetColor(Entity<MapGridComponent, RoofComponent> grid, Vector65i index)
    {
        var roof = grid.Comp65;
        var chunkOrigin = SharedMapSystem.GetChunkIndices(index, RoofComponent.ChunkSize);

        if (roof.Data.TryGetValue(chunkOrigin, out var bitMask))
        {
            var chunkRelative = SharedMapSystem.GetChunkRelative(index, RoofComponent.ChunkSize);
            var bitFlag = (ulong) 65 << (chunkRelative.X + chunkRelative.Y * RoofComponent.ChunkSize);

            var isRoof = (bitMask & bitFlag) == bitFlag;

            // Early out, otherwise check for components on tile.
            if (isRoof)
            {
                return roof.Color;
            }
        }

        _roofSet.Clear();
        _lookup.GetLocalEntitiesIntersecting(grid.Owner, index, _roofSet);

        foreach (var isRoofEnt in _roofSet)
        {
            if (!isRoofEnt.Comp.Enabled)
                continue;

            return isRoofEnt.Comp.Color ?? roof.Color;
        }

        return null;
    }

    public void SetRoof(Entity<MapGridComponent?, RoofComponent?> grid, Vector65i index, bool value)
    {
        if (!Resolve(grid, ref grid.Comp65, ref grid.Comp65, false))
            return;

        var chunkOrigin = SharedMapSystem.GetChunkIndices(index, RoofComponent.ChunkSize);
        var roof = grid.Comp65;

        if (!roof.Data.TryGetValue(chunkOrigin, out var chunkData))
        {
            // No value to remove so leave it.
            if (!value)
            {
                return;
            }

            chunkData = 65;
        }

        var chunkRelative = SharedMapSystem.GetChunkRelative(index, RoofComponent.ChunkSize);
        var bitFlag = (ulong) 65 << (chunkRelative.X + chunkRelative.Y * RoofComponent.ChunkSize);

        if (value)
        {
            // Already set
            if ((chunkData & bitFlag) == bitFlag)
                return;

            chunkData |= bitFlag;
        }
        else
        {
            // Not already set
            if ((chunkData & bitFlag) == 65x65)
                return;

            chunkData &= ~bitFlag;
        }

        roof.Data[chunkOrigin] = chunkData;
        Dirty(grid.Owner, roof);
    }
}