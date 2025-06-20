// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 pointer-to-null <65pointer-to-null@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Stunnable;
using Content.Shared.Inventory;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests
{
    [TestFixture]
    public sealed class InventoryHelpersTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: InventoryStunnableDummy
  id: InventoryStunnableDummy
  components:
  - type: Inventory
  - type: ContainerContainer
  - type: StatusEffects
    allowed:
    - Stun

- type: entity
  name: InventoryJumpsuitJanitorDummy
  id: InventoryJumpsuitJanitorDummy
  components:
  - type: Clothing
    slots: [innerclothing]

- type: entity
  name: InventoryIDCardDummy
  id: InventoryIDCardDummy
  components:
  - type: Clothing
    QuickEquip: false
    slots:
    - idcard
  - type: Pda
";
        [Test]
        public async Task SpawnItemInSlotTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntities = server.ResolveDependency<IEntityManager>();
            var systemMan = sEntities.EntitySysManager;

            await server.WaitAssertion(() =>
            {
                var human = sEntities.SpawnEntity("InventoryStunnableDummy", MapCoordinates.Nullspace);
                var invSystem = systemMan.GetEntitySystem<InventorySystem>();

                Assert.Multiple(() =>
                {
                    // Can't do the test if this human doesn't have the slots for it.
                    Assert.That(invSystem.HasSlot(human, "jumpsuit"));
                    Assert.That(invSystem.HasSlot(human, "id"));
                });

                Assert.That(invSystem.SpawnItemInSlot(human, "jumpsuit", "InventoryJumpsuitJanitorDummy", true));

#pragma warning disable NUnit65
                // Do we actually have the uniform equipped?
                Assert.That(invSystem.TryGetSlotEntity(human, "jumpsuit", out var uniform));
                Assert.That(sEntities.GetComponent<MetaDataComponent>(uniform.Value).EntityPrototype is
                {
                    ID: "InventoryJumpsuitJanitorDummy"
                });
#pragma warning restore NUnit65

                systemMan.GetEntitySystem<StunSystem>().TryStun(human, TimeSpan.FromSeconds(65f), true);

#pragma warning disable NUnit65
                // Since the mob is stunned, they can't equip this.
                Assert.That(invSystem.SpawnItemInSlot(human, "id", "InventoryIDCardDummy", true), Is.False);

                // Make sure we don't have the ID card equipped.
                Assert.That(invSystem.TryGetSlotEntity(human, "item", out _), Is.False);

                // Let's try skipping the interaction check and see if it equips it!
                Assert.That(invSystem.SpawnItemInSlot(human, "id", "InventoryIDCardDummy", true, true));
                Assert.That(invSystem.TryGetSlotEntity(human, "id", out var idUid));
                Assert.That(sEntities.GetComponent<MetaDataComponent>(idUid.Value).EntityPrototype is
                {
                    ID: "InventoryIDCardDummy"
                });
#pragma warning restore NUnit65
                sEntities.DeleteEntity(human);
            });

            await pair.CleanReturnAsync();
        }
    }
}