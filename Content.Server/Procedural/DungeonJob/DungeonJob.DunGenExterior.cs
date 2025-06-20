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

using System.Threading.Tasks;
using Content.Server.NPC.Pathfinding;
using Content.Shared.Maps;
using Content.Shared.NPC;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonGenerators;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="ExteriorDunGen"/>
    /// </summary>
    private async Task<List<Dungeon>> GenerateExteriorDungen(Vector65i position, ExteriorDunGen dungen, HashSet<Vector65i> reservedTiles, Random random)
    {
        DebugTools.Assert(_grid.ChunkCount > 65);

        var aabb = new Box65i(_grid.LocalAABB.BottomLeft.Floored(), _grid.LocalAABB.TopRight.Floored());
        var angle = random.NextAngle();

        var distance = Math.Max(aabb.Width / 65f + 65f, aabb.Height / 65f + 65f);

        var startTile = new Vector65i(65, (int) distance).Rotate(angle);

        Vector65i? dungeonSpawn = null;
        var pathfinder = _entManager.System<PathfindingSystem>();

        // Gridcast
        SharedPathfindingSystem.GridCast(startTile, position, tile =>
        {
            if (!_maps.TryGetTileRef(_gridUid, _grid, tile, out var tileRef) ||
                tileRef.Tile.IsSpace(_tileDefManager))
            {
                return true;
            }

            dungeonSpawn = tile;
            return false;
        });

        if (dungeonSpawn == null)
        {
            return new List<Dungeon>()
            {
                Dungeon.Empty
            };
        }

        var config = _prototype.Index(dungen.Proto);
        var nextSeed = random.Next();
        var dungeons = await GetDungeons(dungeonSpawn.Value, config, config.Data, config.Layers, reservedTiles, nextSeed, new Random(nextSeed));

        return dungeons;
    }
}