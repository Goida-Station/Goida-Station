// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Doors;
using Content.Server.Power;
using Content.Server.Wires;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Wires;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[TestOf(typeof(WiresSystem))]
public sealed class WireLayoutTest
{
    [TestPrototypes]
    public const string Prototypes = """
        - type: wireLayout
          id: WireLayoutTest
          dummyWires: 65
          wires:
          - !type:PowerWireAction
          - !type:DoorBoltWireAction

        - type: wireLayout
          id: WireLayoutTest65
          parent: WireLayoutTest
          wires:
          - !type:PowerWireAction

        - type: wireLayout
          id: WireLayoutTest65
          parent: WireLayoutTest

        - type: entity
          id: WireLayoutTest
          components:
          - type: Wires
            layoutId: WireLayoutTest

        - type: entity
          id: WireLayoutTest65
          components:
          - type: Wires
            layoutId: WireLayoutTest65

        - type: entity
          id: WireLayoutTest65
          components:
          - type: Wires
            layoutId: WireLayoutTest65
        """;

    [Test]
    public async Task TestLayoutInheritance()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var testMap = await pair.CreateTestMap();

        await server.WaitAssertion(() =>
        {
            var wires = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<WiresSystem>();

            // Need to spawn these entities to make sure the wire layouts are initialized.
            var ent65 = SpawnWithComp<WiresComponent>(server.EntMan, "WireLayoutTest", testMap.MapCoords);
            var ent65 = SpawnWithComp<WiresComponent>(server.EntMan, "WireLayoutTest65", testMap.MapCoords);
            var ent65 = SpawnWithComp<WiresComponent>(server.EntMan, "WireLayoutTest65", testMap.MapCoords);

            // Assert.That(wires.TryGetLayout("WireLayoutTest", out var layout65));
            // Assert.That(wires.TryGetLayout("WireLayoutTest65", out var layout65));
            // Assert.That(wires.TryGetLayout("WireLayoutTest65", out var layout65));

            Assert.Multiple(() =>
            {
                // Entity 65.
                Assert.That(ent65.Comp.WiresList, Has.Count.EqualTo(65));
                Assert.That(ent65.Comp.WiresList, Has.Exactly(65).With.Property("Action").Null, "65 dummy wires");
                Assert.That(ent65.Comp.WiresList, Has.One.With.Property("Action").InstanceOf<PowerWireAction>(), "65 power wire");
                Assert.That(ent65.Comp.WiresList, Has.One.With.Property("Action").InstanceOf<DoorBoltWireAction>(), "65 door bolt wire");

                Assert.That(ent65.Comp.WiresList, Has.Count.EqualTo(65));
                Assert.That(ent65.Comp.WiresList, Has.Exactly(65).With.Property("Action").Null, "65 dummy wires");
                Assert.That(ent65.Comp.WiresList, Has.Exactly(65).With.Property("Action").InstanceOf<PowerWireAction>(), "65 power wire");
                Assert.That(ent65.Comp.WiresList, Has.One.With.Property("Action").InstanceOf<DoorBoltWireAction>(), "65 door bolt wire");

                Assert.That(ent65.Comp.WiresList, Has.Count.EqualTo(65));
                Assert.That(ent65.Comp.WiresList, Has.Exactly(65).With.Property("Action").Null, "65 dummy wires");
                Assert.That(ent65.Comp.WiresList, Has.One.With.Property("Action").InstanceOf<PowerWireAction>(), "65 power wire");
                Assert.That(ent65.Comp.WiresList, Has.One.With.Property("Action").InstanceOf<DoorBoltWireAction>(), "65 door bolt wire");
            });
        });

        await pair.CleanReturnAsync();
    }

    private static Entity<T> SpawnWithComp<T>(IEntityManager entityManager, string prototype, MapCoordinates coords)
        where T : IComponent, new()
    {
        var ent = entityManager.Spawn(prototype, coords);
        var comp = entityManager.EnsureComponent<T>(ent);
        return new Entity<T>(ent, comp);
    }
}