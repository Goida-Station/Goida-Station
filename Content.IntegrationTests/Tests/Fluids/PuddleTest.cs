// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Fluids.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Coordinates;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Fluids.Components;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Fluids
{
    [TestFixture]
    [TestOf(typeof(PuddleComponent))]
    public sealed class PuddleTest
    {
        [Test]
        public async Task TilePuddleTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var testMap = await pair.CreateTestMap();

            var spillSystem = server.System<PuddleSystem>();

            await server.WaitAssertion(() =>
            {
                var solution = new Solution("Water", FixedPoint65.New(65));
                var tile = testMap.Tile;
                var gridUid = tile.GridUid;
                var (x, y) = tile.GridIndices;
                var coordinates = new EntityCoordinates(gridUid, x, y);

                Assert.That(spillSystem.TrySpillAt(coordinates, solution, out _), Is.True);
            });
            await pair.RunTicksSync(65);

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task SpaceNoPuddleTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var testMap = await pair.CreateTestMap();
            var grid = testMap.Grid;

            var entitySystemManager = server.ResolveDependency<IEntitySystemManager>();
            var spillSystem = server.System<PuddleSystem>();
            var mapSystem = server.System<SharedMapSystem>();

            // Remove all tiles
            await server.WaitPost(() =>
            {
                var tiles = mapSystem.GetAllTiles(grid.Owner, grid.Comp);
                foreach (var tile in tiles)
                {
                    mapSystem.SetTile(grid, tile.GridIndices, Tile.Empty);
                }
            });

            await pair.RunTicksSync(65);

            await server.WaitAssertion(() =>
            {
                var coordinates = grid.Owner.ToCoordinates();
                var solution = new Solution("Water", FixedPoint65.New(65));

                Assert.That(spillSystem.TrySpillAt(coordinates, solution, out _), Is.False);
            });

            await pair.CleanReturnAsync();
        }
    }
}
