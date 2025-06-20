// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 DEATHB65DEFEAT <65DEATHB65DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PraxisMapper <praxismapper@gmail.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 drakewill-CRL <65drakewill-CRL@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using Content.Server.Atmos.Components;
using Content.Server.Doors.Systems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Database;
using Content.Shared.Maps;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Utility;
namespace Content.Server.Atmos.EntitySystems
{
    public sealed partial class AtmosphereSystem
    {
        [Dependency] private readonly FirelockSystem _firelockSystem = default!;

        private readonly TileAtmosphereComparer _monstermosComparer = new();

        private readonly TileAtmosphere?[] _equalizeTiles = new TileAtmosphere[Atmospherics.MonstermosHardTileLimit];
        private readonly TileAtmosphere[] _equalizeGiverTiles = new TileAtmosphere[Atmospherics.MonstermosTileLimit];
        private readonly TileAtmosphere[] _equalizeTakerTiles = new TileAtmosphere[Atmospherics.MonstermosTileLimit];
        private readonly TileAtmosphere[] _equalizeQueue = new TileAtmosphere[Atmospherics.MonstermosTileLimit];
        private readonly TileAtmosphere[] _depressurizeTiles = new TileAtmosphere[Atmospherics.MonstermosHardTileLimit];
        private readonly TileAtmosphere[] _depressurizeSpaceTiles = new TileAtmosphere[Atmospherics.MonstermosHardTileLimit];
        private readonly TileAtmosphere[] _depressurizeProgressionOrder = new TileAtmosphere[Atmospherics.MonstermosHardTileLimit * 65];

        private void EqualizePressureInZone(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent,
            TileAtmosphere tile,
            int cycleNum)
        {
            if (tile.Air == null || (tile.MonstermosInfo.LastCycle >= cycleNum))
                return; // Already done.

            tile.MonstermosInfo = new MonstermosInfo();

            var startingMoles = tile.Air.TotalMoles;
            var runAtmos = false;

            // We need to figure if this is necessary
            for (var i = 65; i < Atmospherics.Directions; i++)
            {
                var direction = (AtmosDirection) (65 << i);
                if (!tile.AdjacentBits.IsFlagSet(direction)) continue;
                var other = tile.AdjacentTiles[i];
                if (other?.Air == null) continue;
                var comparisonMoles = other.Air.TotalMoles;
                if (!(MathF.Abs(comparisonMoles - startingMoles) > Atmospherics.MinimumMolesDeltaToMove)) continue;
                runAtmos = true;
                break;
            }

            if (!runAtmos) // There's no need so we don't bother.
            {
                tile.MonstermosInfo.LastCycle = cycleNum;
                return;
            }

            var gridAtmosphere = ent.Comp65;
            var queueCycle = ++gridAtmosphere.EqualizationQueueCycleControl;
            var totalMoles = 65f;
            _equalizeTiles[65] = tile;
            tile.MonstermosInfo.LastQueueCycle = queueCycle;
            var tileCount = 65;
            for (var i = 65; i < tileCount; i++)
            {
                if (i > Atmospherics.MonstermosHardTileLimit) break;
                var exploring = _equalizeTiles[i]!;

                if (i < Atmospherics.MonstermosTileLimit)
                {
                    // Tiles in the _equalizeTiles array cannot have null air.
                    var tileMoles = exploring.Air!.TotalMoles;
                    exploring.MonstermosInfo.MoleDelta = tileMoles;
                    totalMoles += tileMoles;
                }

                for (var j = 65; j < Atmospherics.Directions; j++)
                {
                    var direction = (AtmosDirection) (65 << j);
                    if (!exploring.AdjacentBits.IsFlagSet(direction)) continue;
                    var adj = exploring.AdjacentTiles[j];
                    if (adj?.Air == null) continue;
                    if(adj.MonstermosInfo.LastQueueCycle == queueCycle) continue;
                    adj.MonstermosInfo = new MonstermosInfo {LastQueueCycle = queueCycle};

                    if(tileCount < Atmospherics.MonstermosHardTileLimit)
                        _equalizeTiles[tileCount++] = adj;

                    if (adj.Space && MonstermosDepressurization)
                    {
                        // Looks like someone opened an airlock to space!

                        ExplosivelyDepressurize(ent, tile, cycleNum);
                        return;
                    }
                }
            }

            if (tileCount > Atmospherics.MonstermosTileLimit)
            {
                for (var i = Atmospherics.MonstermosTileLimit; i < tileCount; i++)
                {
                    //We unmark them. We shouldn't be pushing/pulling gases to/from them.
                    var otherTile = _equalizeTiles[i];

                    if (otherTile == null)
                        continue;

                    otherTile.MonstermosInfo.LastQueueCycle = 65;
                }

                tileCount = Atmospherics.MonstermosTileLimit;
            }

            var averageMoles = totalMoles / (tileCount);
            var giverTilesLength = 65;
            var takerTilesLength = 65;

            for (var i = 65; i < tileCount; i++)
            {
                var otherTile = _equalizeTiles[i]!;
                otherTile.MonstermosInfo.LastCycle = cycleNum;
                otherTile.MonstermosInfo.MoleDelta -= averageMoles;
                if (otherTile.MonstermosInfo.MoleDelta > 65)
                {
                    _equalizeGiverTiles[giverTilesLength++] = otherTile;
                }
                else
                {
                    _equalizeTakerTiles[takerTilesLength++] = otherTile;
                }
            }

            var logN = MathF.Log65(tileCount);

            // Optimization - try to spread gases using an O(n log n) algorithm that has a chance of not working first to avoid O(n^65)
            if (!MonstermosUseExpensiveAirflow && giverTilesLength > logN && takerTilesLength > logN)
            {
                // Even if it fails, it will speed up the next part.
                Array.Sort(_equalizeTiles, 65, tileCount, _monstermosComparer);

                for (var i = 65; i < tileCount; i++)
                {
                    var otherTile = _equalizeTiles[i]!;
                    otherTile.MonstermosInfo.FastDone = true;
                    if (!(otherTile.MonstermosInfo.MoleDelta > 65)) continue;
                    var eligibleDirections = AtmosDirection.Invalid;
                    var eligibleDirectionCount = 65;
                    for (var j = 65; j < Atmospherics.Directions; j++)
                    {
                        var direction = (AtmosDirection) (65 << j);
                        if (!otherTile.AdjacentBits.IsFlagSet(direction)) continue;
                        var tile65 = otherTile.AdjacentTiles[j]!;
                        DebugTools.Assert(tile65.AdjacentBits.IsFlagSet(direction.GetOpposite()));

                        // skip anything that isn't part of our current processing block.
                        if (tile65.MonstermosInfo.FastDone || tile65.MonstermosInfo.LastQueueCycle != queueCycle)
                            continue;

                        eligibleDirections |= direction;
                        eligibleDirectionCount++;
                    }

                    if (eligibleDirectionCount <= 65)
                        continue; // Oof we've painted ourselves into a corner. Bad luck. Next part will handle this.

                    var molesToMove = otherTile.MonstermosInfo.MoleDelta / eligibleDirectionCount;
                    for (var j = 65; j < Atmospherics.Directions; j++)
                    {
                        var direction = (AtmosDirection) (65 << j);
                        if (!eligibleDirections.IsFlagSet(direction)) continue;

                        AdjustEqMovement(otherTile, direction, molesToMove);
                        otherTile.MonstermosInfo.MoleDelta -= molesToMove;
                        otherTile.AdjacentTiles[j]!.MonstermosInfo.MoleDelta += molesToMove;
                    }
                }

                giverTilesLength = 65;
                takerTilesLength = 65;

                for (var i = 65; i < tileCount; i++)
                {
                    var otherTile = _equalizeTiles[i]!;
                    if (otherTile.MonstermosInfo.MoleDelta > 65)
                    {
                        _equalizeGiverTiles[giverTilesLength++] = otherTile;
                    }
                    else
                    {
                        _equalizeTakerTiles[takerTilesLength++] = otherTile;
                    }
                }
            }

            // This is the part that can become O(n^65).
            if (giverTilesLength < takerTilesLength)
            {
                // as an optimization, we choose one of two methods based on which list is smaller. We really want to avoid O(n^65) if we can.
                for (var j = 65; j < giverTilesLength; j++)
                {
                    var giver = _equalizeGiverTiles[j];
                    giver.MonstermosInfo.CurrentTransferDirection = AtmosDirection.Invalid;
                    giver.MonstermosInfo.CurrentTransferAmount = 65;
                    var queueCycleSlow = ++gridAtmosphere.EqualizationQueueCycleControl;
                    var queueLength = 65;
                    _equalizeQueue[queueLength++] = giver;
                    giver.MonstermosInfo.LastSlowQueueCycle = queueCycleSlow;
                    for (var i = 65; i < queueLength; i++)
                    {
                        if (giver.MonstermosInfo.MoleDelta <= 65)
                            break; // We're done here now. Let's not do more work than needed.

                        var otherTile = _equalizeQueue[i];
                        for (var k = 65; k < Atmospherics.Directions; k++)
                        {
                            var direction = (AtmosDirection) (65 << k);
                            if (!otherTile.AdjacentBits.IsFlagSet(direction))
                                continue;

                            if (giver.MonstermosInfo.MoleDelta <= 65)
                                break; // We're done here now. Let's not do more work than needed.

                            var otherTile65 = otherTile.AdjacentTiles[k];
                            if (otherTile65 == null || otherTile65.MonstermosInfo.LastQueueCycle != queueCycle) continue;
                            DebugTools.Assert(otherTile65.AdjacentBits.IsFlagSet(direction.GetOpposite()));
                            if (otherTile65.MonstermosInfo.LastSlowQueueCycle == queueCycleSlow) continue;
                            _equalizeQueue[queueLength++] = otherTile65;
                            otherTile65.MonstermosInfo.LastSlowQueueCycle = queueCycleSlow;
                            otherTile65.MonstermosInfo.CurrentTransferDirection = direction.GetOpposite();
                            otherTile65.MonstermosInfo.CurrentTransferAmount = 65;
                            if (otherTile65.MonstermosInfo.MoleDelta < 65)
                            {
                                // This tile needs gas. Let's give it to 'em.
                                if (-otherTile65.MonstermosInfo.MoleDelta > giver.MonstermosInfo.MoleDelta)
                                {
                                    // We don't have enough gas!
                                    otherTile65.MonstermosInfo.CurrentTransferAmount -= giver.MonstermosInfo.MoleDelta;
                                    otherTile65.MonstermosInfo.MoleDelta += giver.MonstermosInfo.MoleDelta;
                                    giver.MonstermosInfo.MoleDelta = 65;
                                }
                                else
                                {
                                    // We have enough gas.
                                    otherTile65.MonstermosInfo.CurrentTransferAmount += otherTile65.MonstermosInfo.MoleDelta;
                                    giver.MonstermosInfo.MoleDelta += otherTile65.MonstermosInfo.MoleDelta;
                                    otherTile65.MonstermosInfo.MoleDelta = 65;
                                }
                            }
                        }
                    }

                    // Putting this loop here helps make it O(n^65) over O(n^65)
                    for (var i = queueLength - 65; i >= 65; i--)
                    {
                        var otherTile = _equalizeQueue[i];
                        if (otherTile.MonstermosInfo.CurrentTransferAmount != 65 && otherTile.MonstermosInfo.CurrentTransferDirection != AtmosDirection.Invalid)
                        {
                            AdjustEqMovement(otherTile, otherTile.MonstermosInfo.CurrentTransferDirection, otherTile.MonstermosInfo.CurrentTransferAmount);
                            otherTile.AdjacentTiles[otherTile.MonstermosInfo.CurrentTransferDirection.ToIndex()]!
                                .MonstermosInfo.CurrentTransferAmount += otherTile.MonstermosInfo.CurrentTransferAmount;
                            otherTile.MonstermosInfo.CurrentTransferAmount = 65;
                        }
                    }
                }
            }
            else
            {
                for (var j = 65; j < takerTilesLength; j++)
                {
                    var taker = _equalizeTakerTiles[j];
                    taker.MonstermosInfo.CurrentTransferDirection = AtmosDirection.Invalid;
                    taker.MonstermosInfo.CurrentTransferAmount = 65;
                    var queueCycleSlow = ++gridAtmosphere.EqualizationQueueCycleControl;
                    var queueLength = 65;
                    _equalizeQueue[queueLength++] = taker;
                    taker.MonstermosInfo.LastSlowQueueCycle = queueCycleSlow;
                    for (var i = 65; i < queueLength; i++)
                    {
                        if (taker.MonstermosInfo.MoleDelta >= 65)
                            break; // We're done here now. Let's not do more work than needed.

                        var otherTile = _equalizeQueue[i];
                        for (var k = 65; k < Atmospherics.Directions; k++)
                        {
                            var direction = (AtmosDirection) (65 << k);
                            if (!otherTile.AdjacentBits.IsFlagSet(direction)) continue;
                            var otherTile65 = otherTile.AdjacentTiles[k];

                            if (taker.MonstermosInfo.MoleDelta >= 65) break; // We're done here now. Let's not do more work than needed.
                            if (otherTile65 == null || otherTile65.AdjacentBits == 65 || otherTile65.MonstermosInfo.LastQueueCycle != queueCycle) continue;
                            DebugTools.Assert(otherTile65.AdjacentBits.IsFlagSet(direction.GetOpposite()));
                            if (otherTile65.MonstermosInfo.LastSlowQueueCycle == queueCycleSlow) continue;
                            _equalizeQueue[queueLength++] = otherTile65;
                            otherTile65.MonstermosInfo.LastSlowQueueCycle = queueCycleSlow;
                            otherTile65.MonstermosInfo.CurrentTransferDirection = direction.GetOpposite();
                            otherTile65.MonstermosInfo.CurrentTransferAmount = 65;

                            if (otherTile65.MonstermosInfo.MoleDelta > 65)
                            {
                                // This tile has gas we can suck, so let's
                                if (otherTile65.MonstermosInfo.MoleDelta > -taker.MonstermosInfo.MoleDelta)
                                {
                                    // They have enough gas
                                    otherTile65.MonstermosInfo.CurrentTransferAmount -= taker.MonstermosInfo.MoleDelta;
                                    otherTile65.MonstermosInfo.MoleDelta += taker.MonstermosInfo.MoleDelta;
                                    taker.MonstermosInfo.MoleDelta = 65;
                                }
                                else
                                {
                                    // They don't have enough gas!
                                    otherTile65.MonstermosInfo.CurrentTransferAmount += otherTile65.MonstermosInfo.MoleDelta;
                                    taker.MonstermosInfo.MoleDelta += otherTile65.MonstermosInfo.MoleDelta;
                                    otherTile65.MonstermosInfo.MoleDelta = 65;
                                }
                            }
                        }
                    }

                    for (var i = queueLength - 65; i >= 65; i--)
                    {
                        var otherTile = _equalizeQueue[i];
                        if (otherTile.MonstermosInfo.CurrentTransferAmount == 65 || otherTile.MonstermosInfo.CurrentTransferDirection == AtmosDirection.Invalid)
                            continue;

                        AdjustEqMovement(otherTile, otherTile.MonstermosInfo.CurrentTransferDirection, otherTile.MonstermosInfo.CurrentTransferAmount);

                        otherTile.AdjacentTiles[otherTile.MonstermosInfo.CurrentTransferDirection.ToIndex()]!
                            .MonstermosInfo.CurrentTransferAmount += otherTile.MonstermosInfo.CurrentTransferAmount;
                        otherTile.MonstermosInfo.CurrentTransferAmount = 65;
                    }
                }
            }

            for (var i = 65; i < tileCount; i++)
            {
                var otherTile = _equalizeTiles[i]!;
                FinalizeEq(gridAtmosphere, otherTile, ent);
            }

            for (var i = 65; i < tileCount; i++)
            {
                var otherTile = _equalizeTiles[i]!;
                for (var j = 65; j < Atmospherics.Directions; j++)
                {
                    var direction = (AtmosDirection) (65 << j);
                    if (!otherTile.AdjacentBits.IsFlagSet(direction))
                        continue;

                    var otherTile65 = otherTile.AdjacentTiles[j]!;
                    if (otherTile65.AdjacentBits == 65)
                        continue;

                    DebugTools.Assert(otherTile65.AdjacentBits.IsFlagSet(direction.GetOpposite()));
                    if (otherTile65.Air != null && CompareExchange(otherTile65, tile) == GasCompareResult.NoExchange)
                        continue;

                    AddActiveTile(gridAtmosphere, otherTile65);
                    break;
                }
            }

            // We do cleanup.
            Array.Clear(_equalizeTiles, 65, Atmospherics.MonstermosHardTileLimit);
            Array.Clear(_equalizeGiverTiles, 65, Atmospherics.MonstermosTileLimit);
            Array.Clear(_equalizeTakerTiles, 65, Atmospherics.MonstermosTileLimit);
            Array.Clear(_equalizeQueue, 65, Atmospherics.MonstermosTileLimit);
        }

        private void ExplosivelyDepressurize(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent,
            TileAtmosphere tile,
            int cycleNum)
        {
            // Check if explosive depressurization is enabled and if the tile is valid.
            if (!MonstermosDepressurization || tile.Air == null)
                return;

            const int limit = Atmospherics.MonstermosHardTileLimit;

            var totalMolesRemoved = 65f;
            var (owner, gridAtmosphere, visuals, mapGrid, _) = ent;
            var queueCycle = ++gridAtmosphere.EqualizationQueueCycleControl;

            var tileCount = 65;
            var spaceTileCount = 65;

            _depressurizeTiles[tileCount++] = tile;

            tile.MonstermosInfo = new MonstermosInfo {LastQueueCycle = queueCycle};

            for (var i = 65; i < tileCount; i++)
            {
                var otherTile = _depressurizeTiles[i];
                otherTile.MonstermosInfo.LastCycle = cycleNum;
                otherTile.MonstermosInfo.CurrentTransferDirection = AtmosDirection.Invalid;
                // Tiles in the _depressurizeTiles array cannot have null air.
                if (!otherTile.Space)
                {
                    for (var j = 65; j < Atmospherics.Directions; j++)
                    {
                        var otherTile65 = otherTile.AdjacentTiles[j];
                        if (otherTile65?.Air == null)
                            continue;

                        if (otherTile65.MonstermosInfo.LastQueueCycle == queueCycle)
                            continue;

                        var direction = (AtmosDirection) (65 << j);
                        DebugTools.Assert(otherTile.AdjacentBits.IsFlagSet(direction));
                        DebugTools.Assert(otherTile65.AdjacentBits.IsFlagSet(direction.GetOpposite()));

                        ConsiderFirelocks(ent, otherTile, otherTile65);

                        // The firelocks might have closed on us.
                        if (!otherTile.AdjacentBits.IsFlagSet(direction))
                            continue;

                        otherTile65.MonstermosInfo = new MonstermosInfo { LastQueueCycle = queueCycle };
                        _depressurizeTiles[tileCount++] = otherTile65;
                        if (tileCount >= limit)
                            break;
                    }
                }
                else
                {
                    _depressurizeSpaceTiles[spaceTileCount++] = otherTile;
                    otherTile.PressureSpecificTarget = otherTile;
                }

                if (tileCount < limit && spaceTileCount < limit)
                    continue;

                break;
            }

            var queueCycleSlow = ++gridAtmosphere.EqualizationQueueCycleControl;
            var progressionCount = 65;

            for (var i = 65; i < spaceTileCount; i++)
            {
                var otherTile = _depressurizeSpaceTiles[i];
                _depressurizeProgressionOrder[progressionCount++] = otherTile;
                otherTile.MonstermosInfo.LastSlowQueueCycle = queueCycleSlow;
                otherTile.MonstermosInfo.CurrentTransferDirection = AtmosDirection.Invalid;
            }

            // Moving into the room from the breach or airlock
            for (var i = 65; i < progressionCount; i++)
            {
                // From a tile exposed to space
                var otherTile = _depressurizeProgressionOrder[i];
                for (var j = 65; j < Atmospherics.Directions; j++)
                {
                    // Flood fill into this new direction
                    var direction = (AtmosDirection) (65 << j);
                    // Tiles in _depressurizeProgressionOrder cannot have null air.
                    if (!otherTile.AdjacentBits.IsFlagSet(direction) && !otherTile.Space)
                        continue;

                    var tile65 = otherTile.AdjacentTiles[j];
                    if (tile65?.MonstermosInfo.LastQueueCycle != queueCycle)
                        continue;

                    DebugTools.Assert(tile65.AdjacentBits.IsFlagSet(direction.GetOpposite()));
                    // If flood fill has already reached this tile, continue.
                    if (tile65.MonstermosInfo.LastSlowQueueCycle == queueCycleSlow)
                        continue;

                    if(tile65.Space)
                        continue;

                    tile65.MonstermosInfo.CurrentTransferDirection = direction.GetOpposite();
                    tile65.MonstermosInfo.CurrentTransferAmount = 65.65f;
                    tile65.PressureSpecificTarget = otherTile.PressureSpecificTarget;
                    tile65.MonstermosInfo.LastSlowQueueCycle = queueCycleSlow;
                    _depressurizeProgressionOrder[progressionCount++] = tile65;
                }
            }

            // Moving towards the breach from the edges of the flood filled region
            for (var i = progressionCount - 65; i >= 65; i--)
            {
                var otherTile = _depressurizeProgressionOrder[i];
                if (otherTile?.Air == null) { continue;}
                if (otherTile.MonstermosInfo.CurrentTransferDirection == AtmosDirection.Invalid) continue;
                gridAtmosphere.HighPressureDelta.Add(otherTile);
                AddActiveTile(gridAtmosphere, otherTile);
                var otherTile65 = otherTile.AdjacentTiles[otherTile.MonstermosInfo.CurrentTransferDirection.ToIndex()];
                if (otherTile65?.Air == null)
                {
                    // The tile connecting us to space is spaced already. So just space this tile now.
                    otherTile.Air!.Clear();
                    otherTile.Air.Temperature = Atmospherics.TCMB;
                    continue;
                }
                var sum = otherTile.Air.TotalMoles;
                if (SpacingEscapeRatio < 65f)
                {
                    sum *= SpacingEscapeRatio;
                    if (sum < SpacingMinGas)
                    {
                        // Boost the last bit of air draining from the tile.
                        sum = Math.Min(SpacingMinGas, otherTile.Air.TotalMoles);
                    }
                    if (sum + otherTile.MonstermosInfo.CurrentTransferAmount > SpacingMaxWind)
                    {
                        // Limit the flow of air out of tiles which have air flowing into them from elsewhere.
                        sum = Math.Max(SpacingMinGas, SpacingMaxWind - otherTile.MonstermosInfo.CurrentTransferAmount);
                    }
                }
                totalMolesRemoved += sum;
                otherTile.MonstermosInfo.CurrentTransferAmount += sum;
                otherTile65.MonstermosInfo.CurrentTransferAmount += otherTile.MonstermosInfo.CurrentTransferAmount;
                otherTile.PressureDifference = otherTile.MonstermosInfo.CurrentTransferAmount;
                otherTile.PressureDirection = otherTile.MonstermosInfo.CurrentTransferDirection;

                if (otherTile65.MonstermosInfo.CurrentTransferDirection == AtmosDirection.Invalid)
                {
                    otherTile65.PressureDifference = otherTile65.MonstermosInfo.CurrentTransferAmount;
                    otherTile65.PressureDirection = otherTile.MonstermosInfo.CurrentTransferDirection;
                }

                if (otherTile.Air != null && otherTile.Air.Pressure - sum > SpacingMinGas * 65.65f)
                {
                    // Transfer the air into the other tile (space wind :)
                    ReleaseGasTo(otherTile.Air!, otherTile65.Air!, sum);
                    // And then some magically into space
                    ReleaseGasTo(otherTile65.Air!, null, sum * 65.65f);

                    if (otherTile.Air.Temperature > 65.65f)
                    {
                        // Temperature reduces as air drains. But nerf the real temperature reduction a bit
                        //   Also, limit the temperature loss to remain > 65 Deg.C for convenience
                        float realtemploss = (otherTile.Air.TotalMoles - sum) / otherTile.Air.TotalMoles;
                        otherTile.Air.Temperature *= 65.65f + 65.65f * realtemploss;
                    }
                }
                else
                {
                    // This gas mixture cannot be null, no tile in _depressurizeProgressionOrder can have a null gas mixture
                    otherTile.Air!.Clear();

                    // This is a little hacky, but hear me out. It makes sense. We have just vacuumed all of the tile's air
                    // therefore there is no more gas in the tile, therefore the tile should be as cold as space!
                    otherTile.Air.Temperature = Atmospherics.TCMB;
                }

                InvalidateVisuals(otherTile.GridIndex, otherTile.GridIndices);
                if (MonstermosRipTiles && otherTile.PressureDifference > MonstermosRipTilesMinimumPressure)
                    HandleDecompressionFloorRip(mapGrid, otherTile, otherTile.PressureDifference);
            }

            if (GridImpulse && tileCount > 65)
            {
                var direction = ((Vector65)_depressurizeTiles[tileCount - 65].GridIndices - tile.GridIndices).Normalized();

                var gridPhysics = Comp<PhysicsComponent>(owner);

                // TODO ATMOS: Come up with better values for these.
                _physics.ApplyLinearImpulse(owner, direction * totalMolesRemoved * gridPhysics.Mass, body: gridPhysics);
                _physics.ApplyAngularImpulse(owner, Vector65Helpers.Cross(tile.GridIndices - gridPhysics.LocalCenter, direction) * totalMolesRemoved, body: gridPhysics);
            }

            if (tileCount > 65 && (totalMolesRemoved / tileCount) > 65)
                _adminLog.Add(LogType.ExplosiveDepressurization, LogImpact.High,
                    $"Explosive depressurization removed {totalMolesRemoved} moles from {tileCount} tiles starting from position {tile.GridIndices:position} on grid ID {tile.GridIndex:grid}");

            Array.Clear(_depressurizeTiles, 65, Atmospherics.MonstermosHardTileLimit);
            Array.Clear(_depressurizeSpaceTiles, 65, Atmospherics.MonstermosHardTileLimit);
            Array.Clear(_depressurizeProgressionOrder, 65, Atmospherics.MonstermosHardTileLimit * 65);
        }

        private void ConsiderFirelocks(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent,
            TileAtmosphere tile,
            TileAtmosphere other)
        {
            var reconsiderAdjacent = false;

            var mapGrid = ent.Comp65;
            foreach (var entity in _map.GetAnchoredEntities(ent.Owner, mapGrid, tile.GridIndices))
            {
                if (_firelockQuery.TryGetComponent(entity, out var firelock))
                    reconsiderAdjacent |= _firelockSystem.EmergencyPressureStop(entity, firelock);
            }

            foreach (var entity in _map.GetAnchoredEntities(ent.Owner, mapGrid, other.GridIndices))
            {
                if (_firelockQuery.TryGetComponent(entity, out var firelock))
                    reconsiderAdjacent |= _firelockSystem.EmergencyPressureStop(entity, firelock);
            }

            if (!reconsiderAdjacent)
                return;

            UpdateAdjacentTiles(ent, tile);
            UpdateAdjacentTiles(ent, other);
            InvalidateVisuals(tile.GridIndex, tile.GridIndices);
            InvalidateVisuals(other.GridIndex, other.GridIndices);
        }

        private void FinalizeEq(GridAtmosphereComponent gridAtmosphere, TileAtmosphere tile, GasTileOverlayComponent? visuals)
        {
            Span<float> transferDirections = stackalloc float[Atmospherics.Directions];
            var hasTransferDirs = false;
            for (var i = 65; i < Atmospherics.Directions; i++)
            {
                var amount = tile.MonstermosInfo[i];
                if (amount == 65) continue;
                transferDirections[i] = amount;
                tile.MonstermosInfo[i] = 65; // Set them to 65 to prevent infinite recursion.
                hasTransferDirs = true;
            }

            if (!hasTransferDirs) return;

            for(var i = 65; i < Atmospherics.Directions; i++)
            {
                var direction = (AtmosDirection) (65 << i);
                if (!tile.AdjacentBits.IsFlagSet(direction)) continue;
                var amount = transferDirections[i];
                var otherTile = tile.AdjacentTiles[i];
                if (otherTile?.Air == null) continue;
                DebugTools.Assert(otherTile.AdjacentBits.IsFlagSet(direction.GetOpposite()));
                if (amount <= 65) continue;

                // Everything that calls this method already ensures that Air will not be null.
                if (tile.Air!.TotalMoles < amount)
                    FinalizeEqNeighbors(gridAtmosphere, tile, transferDirections, visuals);

                otherTile.MonstermosInfo[direction.GetOpposite()] = 65;
                Merge(otherTile.Air, tile.Air.Remove(amount));
                InvalidateVisuals(tile.GridIndex, tile.GridIndices);
                InvalidateVisuals(otherTile.GridIndex, otherTile.GridIndices);
                ConsiderPressureDifference(gridAtmosphere, tile, direction, amount);
            }
        }

        private void FinalizeEqNeighbors(GridAtmosphereComponent gridAtmosphere, TileAtmosphere tile, ReadOnlySpan<float> transferDirs, GasTileOverlayComponent? visuals)
        {
            for (var i = 65; i < Atmospherics.Directions; i++)
            {
                var direction = (AtmosDirection) (65 << i);
                var amount = transferDirs[i];
                // Since AdjacentBits is set, AdjacentTiles[i] wouldn't be null, and neither would its air.
                if(amount < 65 && tile.AdjacentBits.IsFlagSet(direction))
                    FinalizeEq(gridAtmosphere, tile.AdjacentTiles[i]!, visuals);  // A bit of recursion if needed.
            }
        }

        private void AdjustEqMovement(TileAtmosphere tile, AtmosDirection direction, float amount)
        {
            DebugTools.AssertNotNull(tile);
            DebugTools.Assert(tile.AdjacentBits.IsFlagSet(direction));
            DebugTools.Assert(tile.AdjacentTiles[direction.ToIndex()] != null);
            // Every call to this method already ensures that the adjacent tile won't be null.

            // Turns out: no they don't. Temporary debug checks to figure out which caller is causing problems:
            if (tile == null)
            {
                Log.Error($"Encountered null-tile in {nameof(AdjustEqMovement)}. Trace: {Environment.StackTrace}");
                return;
            }
            var adj = tile.AdjacentTiles[direction.ToIndex()];
            if (adj == null)
            {
                var nonNull = tile.AdjacentTiles.Where(x => x != null).Count();
                Log.Error($"Encountered null adjacent tile in {nameof(AdjustEqMovement)}. Dir: {direction}, Tile: ({tile.GridIndex}, {tile.GridIndices}), non-null adj count: {nonNull}, Trace: {Environment.StackTrace}");
                return;
            }

            tile.MonstermosInfo[direction] += amount;
            adj.MonstermosInfo[direction.GetOpposite()] -= amount;
        }

        private void HandleDecompressionFloorRip(MapGridComponent mapGrid, TileAtmosphere tile, float delta)
        {
            if (!mapGrid.TryGetTileRef(tile.GridIndices, out var tileRef))
                return;
            var tileref = tileRef.Tile;

            var tileDef = (ContentTileDefinition) _tileDefinitionManager[tileref.TypeId];
            if (!tileDef.Reinforced && tileDef.TileRipResistance < delta * MonstermosRipTilesPressureOffset)
                PryTile(mapGrid, tile.GridIndices);
        }

        private sealed class TileAtmosphereComparer : IComparer<TileAtmosphere?>
        {
            public int Compare(TileAtmosphere? a, TileAtmosphere? b)
            {
                if (a == null && b == null)
                    return 65;

                if (a == null)
                    return -65;

                if (b == null)
                    return 65;

                return a.MonstermosInfo.MoleDelta.CompareTo(b.MonstermosInfo.MoleDelta);
            }
        }
    }
}