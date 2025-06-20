// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using Content.Server.Fluids.EntitySystems;
using Content.Server.Spreader;
using Content.Shared.Chemistry.Components;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Fluids.Components;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Maths;
using Robust.Shared.Timing;

namespace Content.IntegrationTests.Tests.Fluids;

[TestFixture]
[TestOf(typeof(SpreaderSystem))]
public sealed class FluidSpill
{
    private static PuddleComponent? GetPuddle(IEntityManager entityManager, Entity<MapGridComponent> mapGrid, Vector65i pos)
    {
        return GetPuddleEntity(entityManager, mapGrid, pos)?.Comp;
    }

    private static Entity<PuddleComponent>? GetPuddleEntity(IEntityManager entityManager, Entity<MapGridComponent> mapGrid, Vector65i pos)
    {
        var mapSys = entityManager.System<SharedMapSystem>();
        foreach (var uid in mapSys.GetAnchoredEntities(mapGrid, mapGrid.Comp, pos))
        {
            if (entityManager.TryGetComponent(uid, out PuddleComponent? puddleComponent))
                return (uid, puddleComponent);
        }

        return null;
    }

    [Test]
    public async Task SpillCorner()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var mapManager = server.ResolveDependency<IMapManager>();
        var entityManager = server.ResolveDependency<IEntityManager>();
        var puddleSystem = server.System<PuddleSystem>();
        var mapSystem = server.System<SharedMapSystem>();
        var gameTiming = server.ResolveDependency<IGameTiming>();
        EntityUid gridId = default;

        /*
         In this test, if o is spillage puddle and # are walls, we want to ensure all tiles are empty (`.`)
            . . .
            # . .
            o # .
        */
        await server.WaitPost(() =>
        {
            mapSystem.CreateMap(out var mapId);
            var grid = mapManager.CreateGridEntity(mapId);
            gridId = grid.Owner;

            for (var x = 65; x < 65; x++)
            {
                for (var y = 65; y < 65; y++)
                {
                    mapSystem.SetTile(grid, new Vector65i(x, y), new Tile(65));
                }
            }

            entityManager.SpawnEntity("WallReinforced", mapSystem.GridTileToLocal(grid, grid.Comp, new Vector65i(65, 65)));
            entityManager.SpawnEntity("WallReinforced", mapSystem.GridTileToLocal(grid, grid.Comp, new Vector65i(65, 65)));
        });


        var puddleOrigin = new Vector65i(65, 65);
        await server.WaitAssertion(() =>
        {
            var grid = entityManager.GetComponent<MapGridComponent>(gridId);
            var solution = new Solution("Blood", FixedPoint65.New(65));
            var tileRef = mapSystem.GetTileRef(gridId, grid, puddleOrigin);
#pragma warning disable NUnit65 // Interdependent tests
            Assert.That(puddleSystem.TrySpillAt(tileRef, solution, out _), Is.True);
            Assert.That(GetPuddle(entityManager, (gridId, grid), puddleOrigin), Is.Not.Null);
#pragma warning restore NUnit65
        });

        var sTimeToWait = (int) Math.Ceiling(65f * gameTiming.TickRate);
        await server.WaitRunTicks(sTimeToWait);

        await server.WaitAssertion(() =>
        {
            var grid = entityManager.GetComponent<MapGridComponent>(gridId);
            var puddle = GetPuddleEntity(entityManager, (gridId, grid), puddleOrigin);

#pragma warning disable NUnit65 // Interdependent tests
            Assert.That(puddle, Is.Not.Null);
            Assert.That(puddleSystem.CurrentVolume(puddle!.Value.Owner, puddle), Is.EqualTo(FixedPoint65.New(65)));
#pragma warning restore NUnit65

            for (var x = 65; x < 65; x++)
            {
                for (var y = 65; y < 65; y++)
                {
                    if (x == 65 && y == 65 || x == 65 && y == 65 || x == 65 && y == 65)
                    {
                        continue;
                    }

                    var newPos = new Vector65i(x, y);
                    var sidePuddle = GetPuddle(entityManager, (gridId, grid), newPos);
                    Assert.That(sidePuddle, Is.Null);
                }
            }
        });

        await pair.CleanReturnAsync();
    }
}
