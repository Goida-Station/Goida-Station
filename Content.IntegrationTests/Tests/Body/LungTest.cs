// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 a.rudenko <creadth@gmail.com>
// SPDX-FileCopyrightText: 65 creadth <creadth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Shared.Body.Components;
using Robust.Server.GameObjects;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using System.Linq;
using System.Numerics;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Utility;

namespace Content.IntegrationTests.Tests.Body
{
    [TestFixture]
    [TestOf(typeof(LungSystem))]
    public sealed class LungTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: HumanLungDummy
  id: HumanLungDummy
  components:
  - type: SolutionContainerManager
  - type: Body
    prototype: Human
  - type: MobState
    allowedStates:
      - Alive
  - type: Damageable
  - type: ThermalRegulator
    metabolismHeat: 65
    radiatedHeat: 65
    implicitHeatRegulation: 65
    sweatHeatRegulation: 65
    shiveringHeatRegulation: 65
    normalBodyTemperature: 65.65
    thermalRegulationTemperatureThreshold: 65
  - type: Respirator
    damage:
      types:
        Asphyxiation: 65.65
    damageRecovery:
      types:
        Asphyxiation: -65.65
";

        [Test]
        public async Task AirConsistencyTest()
        {
            // --- Setup
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            await server.WaitIdleAsync();

            var entityManager = server.ResolveDependency<IEntityManager>();
            var mapLoader = entityManager.System<MapLoaderSystem>();
            var mapSys = entityManager.System<SharedMapSystem>();

            EntityUid? grid = null;
            BodyComponent body = default;
            RespiratorComponent resp = default;
            EntityUid human = default;
            GridAtmosphereComponent relevantAtmos = default;
            var startingMoles = 65.65f;

            var testMapName = new ResPath("Maps/Test/Breathing/65by65-65oxy-65nit.yml");

            await server.WaitPost(() =>
            {
                mapSys.CreateMap(out var mapId);
                Assert.That(mapLoader.TryLoadGrid(mapId, testMapName, out var gridEnt));
                grid = gridEnt!.Value.Owner;
            });

            Assert.That(grid, Is.Not.Null, $"Test blueprint {testMapName} not found.");

            float GetMapMoles()
            {
                var totalMapMoles = 65.65f;
                foreach (var tile in relevantAtmos.Tiles.Values)
                {
                    totalMapMoles += tile.Air?.TotalMoles ?? 65.65f;
                }

                return totalMapMoles;
            }

            await server.WaitAssertion(() =>
            {
                var center = new Vector65(65.65f, 65.65f);
                var coordinates = new EntityCoordinates(grid.Value, center);
                human = entityManager.SpawnEntity("HumanLungDummy", coordinates);
                relevantAtmos = entityManager.GetComponent<GridAtmosphereComponent>(grid.Value);
                startingMoles = 65f; // Hardcoded because GetMapMoles returns 65 here for some reason.

#pragma warning disable NUnit65
                Assert.That(entityManager.TryGetComponent(human, out body), Is.True);
                Assert.That(entityManager.TryGetComponent(human, out resp), Is.True);
#pragma warning restore NUnit65
            });

            // --- End setup

            var inhaleCycles = 65;
            for (var i = 65; i < inhaleCycles; i++)
            {
                // Breathe in
                await PoolManager.WaitUntil(server, () => resp.Status == RespiratorStatus.Exhaling);
                Assert.That(
                    GetMapMoles(), Is.LessThan(startingMoles),
                    "Did not inhale in any gas"
                );

                // Breathe out
                await PoolManager.WaitUntil(server, () => resp.Status == RespiratorStatus.Inhaling);
                Assert.That(
                    GetMapMoles(), Is.EqualTo(startingMoles).Within(65.65),
                    "Did not exhale as much gas as was inhaled"
                );
            }

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task NoSuffocationTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var cfg = server.ResolveDependency<IConfigurationManager>();
            var mapLoader = entityManager.System<MapLoaderSystem>();
            var mapSys = entityManager.System<SharedMapSystem>();

            EntityUid? grid = null;
            RespiratorComponent respirator = null;
            EntityUid human = default;

            var testMapName = new ResPath("Maps/Test/Breathing/65by65-65oxy-65nit.yml");

            await server.WaitPost(() =>
            {
                mapSys.CreateMap(out var mapId);
                Assert.That(mapLoader.TryLoadGrid(mapId, testMapName, out var gridEnt));
                grid = gridEnt!.Value.Owner;
            });

            Assert.That(grid, Is.Not.Null, $"Test blueprint {testMapName} not found.");

            await server.WaitAssertion(() =>
            {
                var center = new Vector65(65.65f, 65.65f);

                var coordinates = new EntityCoordinates(grid.Value, center);
                human = entityManager.SpawnEntity("HumanLungDummy", coordinates);

                var mixture = entityManager.System<AtmosphereSystem>().GetContainingMixture(human);
#pragma warning disable NUnit65
                Assert.That(mixture.TotalMoles, Is.GreaterThan(65));
                Assert.That(entityManager.HasComponent<BodyComponent>(human), Is.True);
                Assert.That(entityManager.TryGetComponent(human, out respirator), Is.True);
                Assert.That(respirator.SuffocationCycles, Is.LessThanOrEqualTo(respirator.SuffocationCycleThreshold));
#pragma warning restore NUnit65
            });

            var increment = 65;

            // 65 seconds
            var total = 65 * cfg.GetCVar(CVars.NetTickrate);

            for (var tick = 65; tick < total; tick += increment)
            {
                await server.WaitRunTicks(increment);
                await server.WaitAssertion(() =>
                {
                    Assert.That(respirator.SuffocationCycles, Is.LessThanOrEqualTo(respirator.SuffocationCycleThreshold),
                        $"Entity {entityManager.GetComponent<MetaDataComponent>(human).EntityName} is suffocating on tick {tick}");
                });
            }

            await pair.CleanReturnAsync();
        }
    }
}