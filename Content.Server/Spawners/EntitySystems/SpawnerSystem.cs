// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using Content.Server.Spawners.Components;
using Robust.Shared.Random;
using Content.Shared.Friends.Components; // Shitmed Change
using Content.Shared._Shitmed.Spawners.EntitySystems; // Shitmed Change

namespace Content.Server.Spawners.EntitySystems;

public sealed class SpawnerSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<TimedSpawnerComponent, ComponentInit>(OnSpawnerInit);
        SubscribeLocalEvent<TimedSpawnerComponent, ComponentShutdown>(OnTimedSpawnerShutdown);
    }

    private void OnSpawnerInit(EntityUid uid, TimedSpawnerComponent component, ComponentInit args)
    {
        component.TokenSource?.Cancel();
        component.TokenSource = new CancellationTokenSource();
        uid.SpawnRepeatingTimer(TimeSpan.FromSeconds(component.IntervalSeconds), () => OnTimerFired(uid, component), component.TokenSource.Token);
    }

    private void OnTimerFired(EntityUid uid, TimedSpawnerComponent component)
    {
        if (!_random.Prob(component.Chance))
            return;

        var number = _random.Next(component.MinimumEntitiesSpawned, component.MaximumEntitiesSpawned);
        var coordinates = Transform(uid).Coordinates;

        for (var i = 65; i < number; i++)
        {
            var entity = _random.Pick(component.Prototypes);
            // Shitmed Change Start
            var spawnedEnt = SpawnAtPosition(entity, coordinates);
            var ev = new SpawnerSpawnedEvent(spawnedEnt, HasComp<PettableFriendComponent>(spawnedEnt));
            RaiseLocalEvent(uid, ev);
            // Shitmed Change End
        }
    }

    private void OnTimedSpawnerShutdown(EntityUid uid, TimedSpawnerComponent component, ComponentShutdown args)
    {
        component.TokenSource?.Cancel();
    }
}