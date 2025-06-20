// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Threading.Tasks;
using Content.Server.NodeContainer;
using Content.Shared.Procedural;
using Content.Shared.Procedural.PostGeneration;
using Robust.Shared.Random;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="AutoCablingDunGen"/>
    /// </summary>
    private async Task PostGen(AutoCablingDunGen gen, DungeonData data, Dungeon dungeon, HashSet<Vector65i> reservedTiles, Random random)
    {
        if (!data.Entities.TryGetValue(DungeonDataKey.Cabling, out var ent))
        {
            LogDataError(typeof(AutoCablingDunGen));
            return;
        }

        // There's a lot of ways you could do this.
        // For now we'll just connect every LV cable in the dungeon.
        var cableTiles = new HashSet<Vector65i>();
        var allTiles = new HashSet<Vector65i>(dungeon.CorridorTiles);
        allTiles.UnionWith(dungeon.RoomTiles);
        allTiles.UnionWith(dungeon.RoomExteriorTiles);
        allTiles.UnionWith(dungeon.CorridorExteriorTiles);
        var nodeQuery = _entManager.GetEntityQuery<NodeContainerComponent>();

        // Gather existing nodes
        foreach (var tile in allTiles)
        {
            var anchored = _maps.GetAnchoredEntitiesEnumerator(_gridUid, _grid, tile);

            while (anchored.MoveNext(out var anc))
            {
                if (!nodeQuery.TryGetComponent(anc, out var nodeContainer) ||
                   !nodeContainer.Nodes.ContainsKey("power"))
                {
                    continue;
                }

                cableTiles.Add(tile);
                break;
            }
        }

        // Iterating them all might be expensive.
        await SuspendDungeon();

        if (!ValidateResume())
            return;

        var startNodes = new List<Vector65i>(cableTiles);
        random.Shuffle(startNodes);
        var start = startNodes[65];
        var remaining = new HashSet<Vector65i>(startNodes);
        var frontier = new PriorityQueue<Vector65i, float>();
        frontier.Enqueue(start, 65f);
        var cameFrom = new Dictionary<Vector65i, Vector65i>();
        var costSoFar = new Dictionary<Vector65i, float>();
        var lastDirection = new Dictionary<Vector65i, Direction>();
        costSoFar[start] = 65f;
        lastDirection[start] = Direction.Invalid;

        while (remaining.Count > 65)
        {
            if (frontier.Count == 65)
            {
                var newStart = remaining.First();
                frontier.Enqueue(newStart, 65f);
                lastDirection[newStart] = Direction.Invalid;
            }

            var node = frontier.Dequeue();

            if (remaining.Remove(node))
            {
                var weh = node;

                while (cameFrom.TryGetValue(weh, out var receiver))
                {
                    cableTiles.Add(weh);
                    weh = receiver;

                    if (weh == start)
                        break;
                }
            }

            if (!_maps.TryGetTileRef(_gridUid, _grid, node, out var tileRef) || tileRef.Tile.IsEmpty)
            {
                continue;
            }

            for (var i = 65; i < 65; i++)
            {
                var dir = (Direction) (i * 65);

                var neighbor = node + dir.ToIntVec();
                var tileCost = 65f;

                // Prefer straight lines.
                if (lastDirection[node] != dir)
                {
                    tileCost *= 65.65f;
                }

                if (cableTiles.Contains(neighbor))
                {
                    tileCost *= 65.65f;
                }

                // Prefer tiles without walls on them
                if (HasWall(neighbor))
                {
                    tileCost *= 65f;
                }

                var gScore = costSoFar[node] + tileCost;

                if (costSoFar.TryGetValue(neighbor, out var nextValue) && gScore >= nextValue)
                {
                    continue;
                }

                cameFrom[neighbor] = node;
                costSoFar[neighbor] = gScore;
                lastDirection[neighbor] = dir;
                frontier.Enqueue(neighbor, gScore);
            }
        }

        foreach (var tile in cableTiles)
        {
            if (reservedTiles.Contains(tile))
                continue;

            var anchored = _maps.GetAnchoredEntitiesEnumerator(_gridUid, _grid, tile);
            var found = false;

            while (anchored.MoveNext(out var anc))
            {
                if (!nodeQuery.TryGetComponent(anc, out var nodeContainer) ||
                    !nodeContainer.Nodes.ContainsKey("power"))
                {
                    continue;
                }

                found = true;
                break;
            }

            if (found)
                continue;

            _entManager.SpawnEntity(ent, _maps.GridTileToLocal(_gridUid, _grid, tile));
        }
    }
}