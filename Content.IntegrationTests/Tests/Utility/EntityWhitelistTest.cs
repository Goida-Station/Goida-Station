// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Whitelist;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.Utility
{
    [TestFixture]
    [TestOf(typeof(EntityWhitelist))]
    public sealed class EntityWhitelistTest
    {
        private const string InvalidComponent = "Sprite";
        private const string ValidComponent = "Physics";

        [TestPrototypes]
        private const string Prototypes = $@"
- type: Tag
  id: WhitelistTestValidTag
- type: Tag
  id: WhitelistTestInvalidTag

- type: entity
  id: WhitelistDummy
  components:
  - type: ItemSlots
    slots:
      slotName:
        whitelist:
          prototypes:
          - ValidPrototypeDummy
          components:
          - {ValidComponent}
          tags:
          - WhitelistTestValidTag

- type: entity
  id: InvalidComponentDummy
  components:
  - type: {InvalidComponent}
- type: entity
  id: WhitelistTestInvalidTagDummy
  components:
  - type: Tag
    tags:
    - WhitelistTestInvalidTag

- type: entity
  id: ValidComponentDummy
  components:
  - type: {ValidComponent}
- type: entity
  id: WhitelistTestValidTagDummy
  components:
  - type: Tag
    tags:
    - WhitelistTestValidTag";

        [Test]
        public async Task Test()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var testMap = await pair.CreateTestMap();
            var mapCoordinates = testMap.MapCoords;

            var sEntities = server.EntMan;
            var sys = server.System<EntityWhitelistSystem>();

            await server.WaitAssertion(() =>
            {
                var validComponent = sEntities.SpawnEntity("ValidComponentDummy", mapCoordinates);
                var WhitelistTestValidTag = sEntities.SpawnEntity("WhitelistTestValidTagDummy", mapCoordinates);

                var invalidComponent = sEntities.SpawnEntity("InvalidComponentDummy", mapCoordinates);
                var WhitelistTestInvalidTag = sEntities.SpawnEntity("WhitelistTestInvalidTagDummy", mapCoordinates);

                // Test instantiated on its own
                var whitelistInst = new EntityWhitelist
                {
                    Components = new[] { $"{ValidComponent}" },
                    Tags = new() { "WhitelistTestValidTag" }
                };

                Assert.Multiple(() =>
                {
                    Assert.That(sys.IsValid(whitelistInst, validComponent), Is.True);
                    Assert.That(sys.IsValid(whitelistInst, WhitelistTestValidTag), Is.True);

                    Assert.That(sys.IsValid(whitelistInst, invalidComponent), Is.False);
                    Assert.That(sys.IsValid(whitelistInst, WhitelistTestInvalidTag), Is.False);
                });

                // Test from serialized
                var dummy = sEntities.SpawnEntity("WhitelistDummy", mapCoordinates);
                var whitelistSer = sEntities.GetComponent<ItemSlotsComponent>(dummy).Slots.Values.First().Whitelist;
                Assert.That(whitelistSer, Is.Not.Null);

                Assert.Multiple(() =>
                {
                    Assert.That(whitelistSer.Components, Is.Not.Null);
                    Assert.That(whitelistSer.Tags, Is.Not.Null);
                });

                Assert.Multiple(() =>
                {
                    Assert.That(sys.IsValid(whitelistSer, validComponent), Is.True);
                    Assert.That(sys.IsValid(whitelistSer, WhitelistTestValidTag), Is.True);

                    Assert.That(sys.IsValid(whitelistSer, invalidComponent), Is.False);
                    Assert.That(sys.IsValid(whitelistSer, WhitelistTestInvalidTag), Is.False);
                });
            });
            await pair.CleanReturnAsync();
        }
    }
}