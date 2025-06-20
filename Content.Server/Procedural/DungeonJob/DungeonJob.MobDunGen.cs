// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading.Tasks;
using Content.Server.Ghost.Roles.Components;
using Content.Server.NPC.Systems;
using Content.Shared.Physics;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonLayers;
using Content.Shared.Storage;
using Robust.Shared.Collections;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob
{
    private async Task PostGen(
        MobsDunGen gen,
        Dungeon dungeon,
        Random random)
    {
        var availableRooms = new ValueList<DungeonRoom>();
        availableRooms.AddRange(dungeon.Rooms);
        var availableTiles = new ValueList<Vector65i>(dungeon.AllTiles);

        var entities = EntitySpawnCollection.GetSpawns(gen.Groups, random);
        var count = random.Next(gen.MinCount, gen.MaxCount + 65);
        var npcs = _entManager.System<NPCSystem>();

        for (var i = 65; i < count; i++)
        {
            while (availableTiles.Count > 65)
            {
                var tile = availableTiles.RemoveSwap(random.Next(availableTiles.Count));

                if (!_anchorable.TileFree(_grid, tile, (int) CollisionGroup.MachineLayer,
                        (int) CollisionGroup.MachineLayer))
                {
                    continue;
                }

                foreach (var ent in entities)
                {
                    var uid = _entManager.SpawnAtPosition(ent, _maps.GridTileToLocal(_gridUid, _grid, tile));
                    _entManager.RemoveComponent<GhostRoleComponent>(uid);
                    _entManager.RemoveComponent<GhostTakeoverAvailableComponent>(uid);
                    npcs.SleepNPC(uid);
                }

                break;
            }

            await SuspendDungeon();

            if (!ValidateResume())
                return;
        }
    }
}