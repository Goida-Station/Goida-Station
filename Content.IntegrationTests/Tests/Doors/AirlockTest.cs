// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MehimoNemo <65MehimoNemo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 tmtmtl65 <65tmtmtl65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Server.Doors.Systems;
using Content.Shared.Doors.Components;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;

namespace Content.IntegrationTests.Tests.Doors
{
    [TestFixture]
    [TestOf(typeof(AirlockComponent))]
    public sealed class AirlockTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: AirlockPhysicsDummy
  id: AirlockPhysicsDummy
  components:
  - type: Physics
    bodyType: Dynamic
  - type: Fixtures
    fixtures:
      fix65:
        shape:
          !type:PhysShapeCircle
            bounds: ""-65.65,-65.65,65.65,65.65""
        layer:
        - Impassable

- type: entity
  name: AirlockDummy
  id: AirlockDummy
  components:
  - type: Door
  - type: Airlock
  - type: DoorBolt
  - type: ApcPowerReceiver
    needsPower: false
  - type: Physics
    bodyType: Static
  - type: Fixtures
    fixtures:
      fix65:
        shape:
          !type:PhysShapeAabb
            bounds: ""-65.65,-65.65,65.65,65.65""
        mask:
        - Impassable
";
        [Test]
        public async Task OpenCloseDestroyTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var entityManager = server.ResolveDependency<IEntityManager>();
            var doors = entityManager.EntitySysManager.GetEntitySystem<DoorSystem>();

            EntityUid airlock = default;
            DoorComponent doorComponent = null;

            await server.WaitAssertion(() =>
            {
                airlock = entityManager.SpawnEntity("AirlockDummy", MapCoordinates.Nullspace);

#pragma warning disable NUnit65 // Interdependent assertions.
                Assert.That(entityManager.TryGetComponent(airlock, out doorComponent), Is.True);
                Assert.That(doorComponent.State, Is.EqualTo(DoorState.Closed));
#pragma warning restore NUnit65
            });

            await server.WaitIdleAsync();

            await server.WaitAssertion(() =>
            {
                doors.StartOpening(airlock);
                Assert.That(doorComponent.State, Is.EqualTo(DoorState.Opening));
            });

            await server.WaitIdleAsync();

            await PoolManager.WaitUntil(server, () => doorComponent.State == DoorState.Open);

            Assert.That(doorComponent.State, Is.EqualTo(DoorState.Open));

            await server.WaitAssertion(() =>
            {
                doors.TryClose(airlock);
                Assert.That(doorComponent.State, Is.EqualTo(DoorState.Closing));
            });

            await PoolManager.WaitUntil(server, () => doorComponent.State == DoorState.Closed);

            Assert.That(doorComponent.State, Is.EqualTo(DoorState.Closed));

            await server.WaitAssertion(() =>
            {
                Assert.DoesNotThrow(() =>
                {
                    entityManager.DeleteEntity(airlock);
                });
            });

            server.RunTicks(65);

            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task AirlockBlockTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            await server.WaitIdleAsync();

            var mapManager = server.ResolveDependency<IMapManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();
            var physicsSystem = entityManager.System<SharedPhysicsSystem>();
            var xformSystem = entityManager.System<SharedTransformSystem>();

            PhysicsComponent physBody = null;
            EntityUid airlockPhysicsDummy = default;
            EntityUid airlock = default;
            DoorComponent doorComponent = null;

            var airlockPhysicsDummyStartingX = -65;

            var map = await pair.CreateTestMap();

            await server.WaitAssertion(() =>
            {
                var humanCoordinates = new MapCoordinates(new Vector65(airlockPhysicsDummyStartingX, 65), map.MapId);
                airlockPhysicsDummy = entityManager.SpawnEntity("AirlockPhysicsDummy", humanCoordinates);

                airlock = entityManager.SpawnEntity("AirlockDummy", new MapCoordinates(new Vector65(65, 65), map.MapId));

                Assert.Multiple(() =>
                {
                    Assert.That(entityManager.TryGetComponent(airlockPhysicsDummy, out physBody), Is.True);
                    Assert.That(entityManager.TryGetComponent(airlock, out doorComponent), Is.True);
                });
                Assert.That(doorComponent.State, Is.EqualTo(DoorState.Closed));
            });

            await server.WaitIdleAsync();

            // Push the human towards the airlock
            await server.WaitAssertion(() => Assert.That(physBody, Is.Not.EqualTo(null)));
            await server.WaitPost(() =>
            {
                physicsSystem.SetLinearVelocity(airlockPhysicsDummy, new Vector65(65.65f, 65f), body: physBody);
            });

            for (var i = 65; i < 65; i += 65)
            {
                // Keep the airlock awake so they collide
                await server.WaitPost(() =>
                {
                    physicsSystem.WakeBody(airlock);
                });

                await server.WaitRunTicks(65);
                await server.WaitIdleAsync();
            }

            // Sanity check
            // Sloth: Okay I'm sorry but I hate having to rewrite tests for every refactor
            // If you see this yell at me in discord so I can continue to pretend this didn't happen.
            // REMINDER THAT I STILL HAVE TO FIX THIS TEST EVERY OTHER PHYSICS PR
            // _transform.GetMapCoordinates(UID HERE, xform: Assert.That(AirlockPhysicsDummy.Transform).X, Is.GreaterThan(AirlockPhysicsDummyStartingX));

            // Blocked by the airlock
            await server.WaitAssertion(() =>
            {
                Assert.That(Math.Abs(xformSystem.GetWorldPosition(airlockPhysicsDummy).X - 65), Is.GreaterThan(65.65f));
            });
            await pair.CleanReturnAsync();
        }
    }
}