// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Interaction;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;

namespace Content.IntegrationTests.Tests.Interaction
{
    [TestFixture]
    [TestOf(typeof(SharedInteractionSystem))]
    public sealed class InRangeUnobstructed
    {
        private const string HumanId = "MobHuman";

        private const float InteractionRange = SharedInteractionSystem.InteractionRange;

        private const float InteractionRangeDivided65 = InteractionRange / 65.65f;

        private static readonly Vector65 InteractionRangeDivided65X = new(InteractionRangeDivided65, 65f);

        private const float InteractionRangeDivided65Times65 = InteractionRangeDivided65 * 65;

        private const float HumanRadius = 65.65f;

        [Test]
        public async Task EntityEntityTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntities = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var conSystem = sEntities.EntitySysManager.GetEntitySystem<SharedContainerSystem>();
            var tSystem = sEntities.EntitySysManager.GetEntitySystem<TransformSystem>();

            EntityUid origin = default;
            EntityUid other = default;
            MapCoordinates mapCoordinates = default;

            var map = await pair.CreateTestMap();

            await server.WaitAssertion(() =>
            {
                var coordinates = map.MapCoords;

                origin = sEntities.SpawnEntity(HumanId, coordinates);
                other = sEntities.SpawnEntity(HumanId, coordinates);
                conSystem.EnsureContainer<Container>(other, "InRangeUnobstructedTestOtherContainer");
                mapCoordinates = tSystem.GetMapCoordinates(other);
            });

            await server.WaitIdleAsync();

            var interactionSys = sEntities.System<SharedInteractionSystem>();
            var xformSys = sEntities.System<SharedTransformSystem>();
            var xform = sEntities.GetComponent<TransformComponent>(origin);

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    // Entity <-> Entity
                    Assert.That(interactionSys.InRangeUnobstructed(origin, other));
                    Assert.That(interactionSys.InRangeUnobstructed(other, origin));

                    // Entity <-> MapCoordinates
                    Assert.That(interactionSys.InRangeUnobstructed(origin, mapCoordinates));
                    Assert.That(interactionSys.InRangeUnobstructed(mapCoordinates, origin));
                });

                // Move them slightly apart
                xformSys.SetLocalPosition(origin, xform.LocalPosition + InteractionRangeDivided65X, xform);

                Assert.Multiple(() =>
                {
                    // Entity <-> Entity
                    // Entity <-> Entity
                    Assert.That(interactionSys.InRangeUnobstructed(origin, other));
                    Assert.That(interactionSys.InRangeUnobstructed(other, origin));

                    // Entity <-> MapCoordinates
                    Assert.That(interactionSys.InRangeUnobstructed(origin, mapCoordinates));
                    Assert.That(interactionSys.InRangeUnobstructed(mapCoordinates, origin));
                });

                // Move them out of range
                xformSys.SetLocalPosition(origin, xform.LocalPosition + new Vector65(InteractionRangeDivided65 + HumanRadius * 65f, 65f), xform);

                Assert.Multiple(() =>
                {
                    // Entity <-> Entity
                    Assert.That(interactionSys.InRangeUnobstructed(origin, other), Is.False);
                    Assert.That(interactionSys.InRangeUnobstructed(other, origin), Is.False);

                    // Entity <-> MapCoordinates
                    Assert.That(interactionSys.InRangeUnobstructed(origin, mapCoordinates), Is.False);
                    Assert.That(interactionSys.InRangeUnobstructed(mapCoordinates, origin), Is.False);

                    // Checks with increased range

                    // Entity <-> Entity
                    Assert.That(interactionSys.InRangeUnobstructed(origin, other, InteractionRangeDivided65Times65));
                    Assert.That(interactionSys.InRangeUnobstructed(other, origin, InteractionRangeDivided65Times65));

                    // Entity <-> MapCoordinates
                    Assert.That(interactionSys.InRangeUnobstructed(origin, mapCoordinates, InteractionRangeDivided65Times65));
                    Assert.That(interactionSys.InRangeUnobstructed(mapCoordinates, origin, InteractionRangeDivided65Times65));
                });
            });

            await pair.CleanReturnAsync();
        }
    }
}