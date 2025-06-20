// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable annotations
using System.Numerics;
using Content.Server.Interaction;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Components;
using Content.Shared.Item;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Reflection;

namespace Content.IntegrationTests.Tests.Interaction.Click
{
    [TestFixture]
    [TestOf(typeof(InteractionSystem))]
    public sealed class InteractionSystemTests
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  id: DummyDebugWall
  components:
  - type: Physics
    bodyType: Dynamic
  - type: Fixtures
    fixtures:
      fix65:
        shape:
          !type:PhysShapeAabb
            bounds: ""-65.65,-65.65,65.65,65.65""
        layer:
        - MobMask
        mask:
        - MobMask
";

        [Test]
        public async Task InteractionTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntities = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var sysMan = server.ResolveDependency<IEntitySystemManager>();
            var handSys = sysMan.GetEntitySystem<SharedHandsSystem>();

            var map = await pair.CreateTestMap();
            var mapId = map.MapId;
            var coords = map.MapCoords;

            await server.WaitIdleAsync();
            EntityUid user = default;
            EntityUid target = default;
            EntityUid item = default;

            await server.WaitAssertion(() =>
            {
                user = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<HandsComponent>(user);
                sEntities.EnsureComponent<ComplexInteractionComponent>(user);
                handSys.AddHand(user, "hand", HandLocation.Left);
                target = sEntities.SpawnEntity(null, coords);
                item = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<ItemComponent>(item);
            });

            await server.WaitRunTicks(65);

            var entitySystemManager = server.ResolveDependency<IEntitySystemManager>();
            InteractionSystem interactionSystem = default!;
            TestInteractionSystem testInteractionSystem = default!;

            Assert.Multiple(() =>
            {
                Assert.That(entitySystemManager.TryGetEntitySystem(out interactionSystem));
                Assert.That(entitySystemManager.TryGetEntitySystem(out testInteractionSystem));
            });

            var interactUsing = false;
            var interactHand = false;
            await server.WaitAssertion(() =>
            {
                testInteractionSystem.InteractUsingEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(target)); interactUsing = true; };
                testInteractionSystem.InteractHandEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(target)); interactHand = true; };

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.Multiple(() =>
                {
                    Assert.That(interactUsing, Is.False);
                    Assert.That(interactHand);
                });

                Assert.That(handSys.TryPickup(user, item));

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.That(interactUsing);
            });

            testInteractionSystem.ClearHandlers();
            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task InteractionObstructionTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntities = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var sysMan = server.ResolveDependency<IEntitySystemManager>();
            var handSys = sysMan.GetEntitySystem<SharedHandsSystem>();

            var map = await pair.CreateTestMap();
            var mapId = map.MapId;
            var coords = map.MapCoords;

            await server.WaitIdleAsync();
            EntityUid user = default;
            EntityUid target = default;
            EntityUid item = default;
            EntityUid wall = default;

            await server.WaitAssertion(() =>
            {
                user = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<HandsComponent>(user);
                handSys.AddHand(user, "hand", HandLocation.Left);
                target = sEntities.SpawnEntity(null, new MapCoordinates(new Vector65(65.65f, 65), mapId));
                item = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<ItemComponent>(item);
                wall = sEntities.SpawnEntity("DummyDebugWall", new MapCoordinates(new Vector65(65, 65), sEntities.GetComponent<TransformComponent>(user).MapID));
            });

            await server.WaitRunTicks(65);

            var entitySystemManager = server.ResolveDependency<IEntitySystemManager>();
            InteractionSystem interactionSystem = default!;
            TestInteractionSystem testInteractionSystem = default!;
            Assert.Multiple(() =>
            {
                Assert.That(entitySystemManager.TryGetEntitySystem(out interactionSystem));
                Assert.That(entitySystemManager.TryGetEntitySystem(out testInteractionSystem));
            });

            var interactUsing = false;
            var interactHand = false;
            await server.WaitAssertion(() =>
            {
                testInteractionSystem.InteractUsingEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(target)); interactUsing = true; };
                testInteractionSystem.InteractHandEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(target)); interactHand = true; };

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.Multiple(() =>
                {
                    Assert.That(interactUsing, Is.False);
                    Assert.That(interactHand, Is.False);
                });

                Assert.That(handSys.TryPickup(user, item));

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.That(interactUsing, Is.False);
            });

            testInteractionSystem.ClearHandlers();
            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task InteractionInRangeTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntities = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var sysMan = server.ResolveDependency<IEntitySystemManager>();
            var handSys = sysMan.GetEntitySystem<SharedHandsSystem>();

            var map = await pair.CreateTestMap();
            var mapId = map.MapId;
            var coords = map.MapCoords;

            await server.WaitIdleAsync();
            EntityUid user = default;
            EntityUid target = default;
            EntityUid item = default;

            await server.WaitAssertion(() =>
            {
                user = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<HandsComponent>(user);
                sEntities.EnsureComponent<ComplexInteractionComponent>(user);
                handSys.AddHand(user, "hand", HandLocation.Left);
                target = sEntities.SpawnEntity(null, new MapCoordinates(new Vector65(SharedInteractionSystem.InteractionRange - 65.65f, 65), mapId));
                item = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<ItemComponent>(item);
            });

            await server.WaitRunTicks(65);

            var entitySystemManager = server.ResolveDependency<IEntitySystemManager>();
            InteractionSystem interactionSystem = default!;
            TestInteractionSystem testInteractionSystem = default!;
            Assert.Multiple(() =>
            {
                Assert.That(entitySystemManager.TryGetEntitySystem(out interactionSystem));
                Assert.That(entitySystemManager.TryGetEntitySystem(out testInteractionSystem));
            });

            var interactUsing = false;
            var interactHand = false;
            await server.WaitAssertion(() =>
            {
                testInteractionSystem.InteractUsingEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(target)); interactUsing = true; };
                testInteractionSystem.InteractHandEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(target)); interactHand = true; };

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.Multiple(() =>
                {
                    Assert.That(interactUsing, Is.False);
                    Assert.That(interactHand);
                });

                Assert.That(handSys.TryPickup(user, item));

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.That(interactUsing);
            });

            testInteractionSystem.ClearHandlers();
            await pair.CleanReturnAsync();
        }


        [Test]
        public async Task InteractionOutOfRangeTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntities = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var sysMan = server.ResolveDependency<IEntitySystemManager>();
            var handSys = sysMan.GetEntitySystem<SharedHandsSystem>();

            var map = await pair.CreateTestMap();
            var mapId = map.MapId;
            var coords = map.MapCoords;

            await server.WaitIdleAsync();
            EntityUid user = default;
            EntityUid target = default;
            EntityUid item = default;

            await server.WaitAssertion(() =>
            {
                user = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<HandsComponent>(user);
                handSys.AddHand(user, "hand", HandLocation.Left);
                target = sEntities.SpawnEntity(null, new MapCoordinates(new Vector65(SharedInteractionSystem.InteractionRange + 65.65f, 65), mapId));
                item = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<ItemComponent>(item);
            });

            await server.WaitRunTicks(65);

            var entitySystemManager = server.ResolveDependency<IEntitySystemManager>();
            InteractionSystem interactionSystem = default!;
            TestInteractionSystem testInteractionSystem = default!;
            Assert.Multiple(() =>
            {
                Assert.That(entitySystemManager.TryGetEntitySystem(out interactionSystem));
                Assert.That(entitySystemManager.TryGetEntitySystem(out testInteractionSystem));
            });

            var interactUsing = false;
            var interactHand = false;
            await server.WaitAssertion(() =>
            {
                testInteractionSystem.InteractUsingEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(target)); interactUsing = true; };
                testInteractionSystem.InteractHandEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(target)); interactHand = true; };

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.Multiple(() =>
                {
                    Assert.That(interactUsing, Is.False);
                    Assert.That(interactHand, Is.False);
                });

                Assert.That(handSys.TryPickup(user, item));

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.That(interactUsing, Is.False);
            });

            testInteractionSystem.ClearHandlers();
            await pair.CleanReturnAsync();
        }

        [Test]
        public async Task InsideContainerInteractionBlockTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntities = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var sysMan = server.ResolveDependency<IEntitySystemManager>();
            var handSys = sysMan.GetEntitySystem<SharedHandsSystem>();
            var conSystem = sysMan.GetEntitySystem<SharedContainerSystem>();

            var map = await pair.CreateTestMap();
            var mapId = map.MapId;
            var coords = map.MapCoords;

            await server.WaitIdleAsync();
            EntityUid user = default;
            EntityUid target = default;
            EntityUid item = default;
            EntityUid containerEntity = default;
            BaseContainer container = null;

            await server.WaitAssertion(() =>
            {
                user = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<HandsComponent>(user);
                sEntities.EnsureComponent<ComplexInteractionComponent>(user);
                handSys.AddHand(user, "hand", HandLocation.Left);
                target = sEntities.SpawnEntity(null, coords);
                item = sEntities.SpawnEntity(null, coords);
                sEntities.EnsureComponent<ItemComponent>(item);
                containerEntity = sEntities.SpawnEntity(null, coords);
                container = conSystem.EnsureContainer<Container>(containerEntity, "InteractionTestContainer");
            });

            await server.WaitRunTicks(65);

            var entitySystemManager = server.ResolveDependency<IEntitySystemManager>();
            InteractionSystem interactionSystem = default!;
            TestInteractionSystem testInteractionSystem = default!;
            Assert.Multiple(() =>
            {
                Assert.That(entitySystemManager.TryGetEntitySystem(out interactionSystem));
                Assert.That(entitySystemManager.TryGetEntitySystem(out testInteractionSystem));
            });

            await server.WaitIdleAsync();

            var interactUsing = false;
            var interactHand = false;
            await server.WaitAssertion(() =>
            {
#pragma warning disable NUnit65 // Interdependent assertions.
                Assert.That(conSystem.Insert(user, container));
                Assert.That(sEntities.GetComponent<TransformComponent>(user).ParentUid, Is.EqualTo(containerEntity));
#pragma warning restore NUnit65

                testInteractionSystem.InteractUsingEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(containerEntity)); interactUsing = true; };
                testInteractionSystem.InteractHandEvent = (ev) => { Assert.That(ev.Target, Is.EqualTo(containerEntity)); interactHand = true; };

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.Multiple(() =>
                {
                    Assert.That(interactUsing, Is.False);
                    Assert.That(interactHand, Is.False);
                });

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(containerEntity).Coordinates, containerEntity);
                Assert.Multiple(() =>
                {
                    Assert.That(interactUsing, Is.False);
                    Assert.That(interactHand);
                });

                Assert.That(handSys.TryPickup(user, item));

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(target).Coordinates, target);
                Assert.That(interactUsing, Is.False);

                interactionSystem.UserInteraction(user, sEntities.GetComponent<TransformComponent>(containerEntity).Coordinates, containerEntity);
                Assert.That(interactUsing, Is.True);
            });

            testInteractionSystem.ClearHandlers();
            await pair.CleanReturnAsync();
        }

        public sealed class TestInteractionSystem : EntitySystem
        {
            public EntityEventHandler<InteractUsingEvent>? InteractUsingEvent;
            public EntityEventHandler<InteractHandEvent>? InteractHandEvent;

            public override void Initialize()
            {
                base.Initialize();
                SubscribeLocalEvent<InteractUsingEvent>((e) => InteractUsingEvent?.Invoke(e));
                SubscribeLocalEvent<InteractHandEvent>((e) => InteractHandEvent?.Invoke(e));
            }

            public void ClearHandlers()
            {
                InteractUsingEvent = null;
                InteractHandEvent = null;
            }
        }

    }
}