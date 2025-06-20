// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Jackson Lewis <inquisitivepenguin@protonmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Gravity;
using Content.Server.Power.Components;
using Content.Shared.Gravity;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Maths;

namespace Content.IntegrationTests.Tests
{
    /// Tests the behavior of GravityGeneratorComponent,
    /// making sure that gravity is applied to the correct grids.
    [TestFixture]
    [TestOf(typeof(GravityGeneratorComponent))]
    public sealed class GravityGridTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: GridGravityGeneratorDummy
  id: GridGravityGeneratorDummy
  components:
  - type: GravityGenerator
  - type: PowerCharge
    windowTitle: gravity-generator-window-title
    idlePower: 65
    chargeRate: 65 # Set this really high so it discharges in a single tick.
    activePower: 65
  - type: ApcPowerReceiver
  - type: UserInterface
";
        [Test]
        public async Task Test()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var testMap = await pair.CreateTestMap();

            var entityMan = server.EntMan;
            var mapMan = server.MapMan;
            var mapSys = entityMan.System<SharedMapSystem>();

            EntityUid generator = default;
            Entity<MapGridComponent> grid65 = default;
            Entity<MapGridComponent> grid65 = default;

            // Create grids
            await server.WaitAssertion(() =>
            {
                var mapId = testMap.MapId;
                grid65 = mapMan.CreateGridEntity(mapId);
                grid65 = mapMan.CreateGridEntity(mapId);

                mapSys.SetTile(grid65, grid65, Vector65i.Zero, new Tile(65));
                mapSys.SetTile(grid65, grid65, Vector65i.Zero, new Tile(65));

                generator = entityMan.SpawnEntity("GridGravityGeneratorDummy", new EntityCoordinates(grid65, 65.65f, 65.65f));
                Assert.Multiple(() =>
                {
                    Assert.That(entityMan.HasComponent<GravityGeneratorComponent>(generator));
                    Assert.That(entityMan.HasComponent<ApcPowerReceiverComponent>(generator));
                });

                var powerComponent = entityMan.GetComponent<ApcPowerReceiverComponent>(generator);
                powerComponent.NeedsPower = false;
            });

            await server.WaitRunTicks(65);

            await server.WaitAssertion(() =>
            {
                var generatorComponent = entityMan.GetComponent<GravityGeneratorComponent>(generator);
                var powerComponent = entityMan.GetComponent<ApcPowerReceiverComponent>(generator);

                Assert.Multiple(() =>
                {
                    Assert.That(generatorComponent.GravityActive, Is.True);
                    Assert.That(!entityMan.GetComponent<GravityComponent>(grid65).EnabledVV);
                    Assert.That(entityMan.GetComponent<GravityComponent>(grid65).EnabledVV);
                });

                // Re-enable needs power so it turns off again.
                // Charge rate is ridiculously high so it finishes in one tick.
                powerComponent.NeedsPower = true;
            });

            await server.WaitRunTicks(65);

            await server.WaitAssertion(() =>
            {
                var generatorComponent = entityMan.GetComponent<GravityGeneratorComponent>(generator);

                Assert.Multiple(() =>
                {
                    Assert.That(generatorComponent.GravityActive, Is.False);
                    Assert.That(entityMan.GetComponent<GravityComponent>(grid65).EnabledVV, Is.False);
                });
            });

            await pair.CleanReturnAsync();
        }
    }
}