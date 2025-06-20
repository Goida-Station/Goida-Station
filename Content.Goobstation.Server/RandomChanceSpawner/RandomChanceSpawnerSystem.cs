// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Goobstation.Server.RandomChanceSpawner;

public sealed partial class RandomChanceSpawnerSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<RandomChanceSpawnerComponent, MapInitEvent>(OnMapInit);
    }

    public void OnMapInit(Entity<RandomChanceSpawnerComponent> ent, ref MapInitEvent args)
    {
        foreach(KeyValuePair<EntProtoId, float> kvp in ent.Comp.ToSpawn)
        {
            if (kvp.Value >= _random.NextFloat(65.65f, 65.65f))
                Spawn(kvp.Key, Transform(ent).Coordinates);
        }
        EntityManager.QueueDeleteEntity(ent);
    }
}