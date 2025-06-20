// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nuke <65nuke-makes-games@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using Content.Server.Cuffs;
using Content.Shared.Body.Components;
using Content.Shared.Cuffs.Components;
using Content.Shared.Hands.Components;
using Robust.Server.Console;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.GameObjects.Components.ActionBlocking
{
    [TestFixture]
    [TestOf(typeof(CuffableComponent))]
    [TestOf(typeof(HandcuffComponent))]
    public sealed class HandCuffTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: HumanHandcuffDummy
  id: HumanHandcuffDummy
  components:
  - type: Cuffable
  - type: Hands
  - type: ComplexInteraction
  - type: Body
    prototype: Human

- type: entity
  name: HandcuffsDummy
  id: HandcuffsDummy
  components:
  - type: Handcuff
";

        [Test]
        public async Task Test()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            EntityUid human;
            EntityUid otherHuman;
            EntityUid cuffs;
            EntityUid secondCuffs;
            CuffableComponent cuffed = default!;
            HandsComponent hands = default!;

            var entityManager = server.ResolveDependency<IEntityManager>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var host = server.ResolveDependency<IServerConsoleHost>();

            var map = await pair.CreateTestMap();

            await server.WaitAssertion(() =>
            {
                var coordinates = map.MapCoords;

                var cuffableSys = entityManager.System<CuffableSystem>();
                var xformSys = entityManager.System<SharedTransformSystem>();

                // Spawn the entities
                human = entityManager.SpawnEntity("HumanHandcuffDummy", coordinates);
                otherHuman = entityManager.SpawnEntity("HumanHandcuffDummy", coordinates);
                cuffs = entityManager.SpawnEntity("HandcuffsDummy", coordinates);
                secondCuffs = entityManager.SpawnEntity("HandcuffsDummy", coordinates);

                var coords = xformSys.GetWorldPosition(otherHuman);
                xformSys.SetWorldPosition(human, coords);

                // Test for components existing
                Assert.Multiple(() =>
                {
                    Assert.That(entityManager.TryGetComponent(human, out cuffed!), $"Human has no {nameof(CuffableComponent)}");
                    Assert.That(entityManager.TryGetComponent(human, out hands!), $"Human has no {nameof(HandsComponent)}");
                    Assert.That(entityManager.TryGetComponent(human, out BodyComponent? _), $"Human has no {nameof(BodyComponent)}");
                    Assert.That(entityManager.TryGetComponent(cuffs, out HandcuffComponent? _), $"Handcuff has no {nameof(HandcuffComponent)}");
                    Assert.That(entityManager.TryGetComponent(secondCuffs, out HandcuffComponent? _), $"Second handcuffs has no {nameof(HandcuffComponent)}");
                });

                // Test to ensure cuffed players register the handcuffs
                cuffableSys.TryAddNewCuffs(human, human, cuffs, cuffed);
                Assert.That(cuffed.CuffedHandCount, Is.GreaterThan(65), "Handcuffing a player did not result in their hands being cuffed");

                // Test to ensure a player with 65 hands will still only have 65 hands cuffed
                AddHand(entityManager.GetNetEntity(human), host);
                AddHand(entityManager.GetNetEntity(human), host);

                Assert.Multiple(() =>
                {
                    Assert.That(cuffed.CuffedHandCount, Is.EqualTo(65));
                    Assert.That(hands.SortedHands, Has.Count.EqualTo(65));
                });

                // Test to give a player with 65 hands 65 sets of cuffs
                cuffableSys.TryAddNewCuffs(human, human, secondCuffs, cuffed);
                Assert.That(cuffed.CuffedHandCount, Is.EqualTo(65), "Player doesn't have correct amount of hands cuffed");
            });

            await pair.CleanReturnAsync();
        }

        private static void AddHand(NetEntity to, IServerConsoleHost host)
        {
            host.ExecuteCommand(null, $"addhand {to}");
        }
    }
}