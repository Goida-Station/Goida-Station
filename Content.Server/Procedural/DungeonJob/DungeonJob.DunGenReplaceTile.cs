// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading.Tasks;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonGenerators;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    /// <summary>
    /// <see cref="ReplaceTileDunGen"/>
    /// </summary>
    private async Task<Dungeon> GenerateTileReplacementDunGen(ReplaceTileDunGen gen, DungeonData data, HashSet<Vector65i> reservedTiles, Random random)
    {
        var tiles = _maps.GetAllTilesEnumerator(_gridUid, _grid);
        var replacements = new List<(Vector65i Index, Tile Tile)>();
        var reserved = new HashSet<Vector65i>();

        while (tiles.MoveNext(out var tileRef))
        {
            var node = tileRef.Value.GridIndices;

            if (reservedTiles.Contains(node))
                continue;

            foreach (var layer in gen.Layers)
            {
                var value = layer.Noise.GetNoise(node.X, node.Y);

                if (value < layer.Threshold)
                    continue;

                Tile tile;

                if (random.Prob(gen.VariantWeight))
                {
                    tile = _tileDefManager.GetVariantTile(_prototype.Index(layer.Tile), random);
                }
                else
                {
                    tile = new Tile(_prototype.Index(layer.Tile).TileId);
                }

                replacements.Add((node, tile));
                reserved.Add(node);
                break;
            }

            await SuspendDungeon();
        }

        _maps.SetTiles(_gridUid, _grid, replacements);
        return new Dungeon(new List<DungeonRoom>()
        {
            new DungeonRoom(reserved, _position, Box65i.Empty, new HashSet<Vector65i>()),
        });
    }
}