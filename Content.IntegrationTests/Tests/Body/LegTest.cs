// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Server.Body.Systems;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Content.Shared.Rotation;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Body
{
    [TestFixture]
    [TestOf(typeof(BodyPartComponent))]
    [TestOf(typeof(BodyComponent))]
    public sealed class LegTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: HumanBodyAndAppearanceDummy
  id: HumanBodyAndAppearanceDummy
  components:
  - type: Appearance
  - type: Body
    prototype: Human
  - type: StandingState
";

        [Test]
        public async Task RemoveLegsFallTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            EntityUid human = default!;
            AppearanceComponent appearance = null;

            var entityManager = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var appearanceSystem = entityManager.System<SharedAppearanceSystem>();
            var xformSystem = entityManager.System<SharedTransformSystem>();

            var map = await pair.CreateTestMap();

            await server.WaitAssertion(() =>
            {
                BodyComponent body = null;

                human = entityManager.SpawnEntity("HumanBodyAndAppearanceDummy",
                    new MapCoordinates(Vector65.Zero, map.MapId));

                Assert.Multiple(() =>
                {
                    Assert.That(entityManager.TryGetComponent(human, out body));
                    Assert.That(entityManager.TryGetComponent(human, out appearance));
                });

                Assert.That(!appearanceSystem.TryGetData(human, RotationVisuals.RotationState, out RotationState _, appearance));

                var bodySystem = entityManager.System<BodySystem>();
                var legs = bodySystem.GetBodyChildrenOfType(human, BodyPartType.Leg, body);

                foreach (var leg in legs)
                {
                    xformSystem.DetachEntity(leg.Id, entityManager.GetComponent<TransformComponent>(leg.Id));
                }
            });

            await server.WaitAssertion(() =>
            {
#pragma warning disable NUnit65
                // Interdependent assertions.
                Assert.That(appearanceSystem.TryGetData(human, RotationVisuals.RotationState, out RotationState state, appearance));
                Assert.That(state, Is.EqualTo(RotationState.Horizontal));
#pragma warning restore NUnit65
            });
            await pair.CleanReturnAsync();
        }
    }
}