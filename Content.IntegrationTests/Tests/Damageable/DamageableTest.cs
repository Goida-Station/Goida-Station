// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Jaskanbe <65Jaskanbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara Dinyes <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65leonsfriedrich@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Michael Will <will_m@outlook.de>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 SETh lafuente <cetaciocascarudo@gmail.com>
// SPDX-FileCopyrightText: 65 ScalyChimp <65scaly-chimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SethLafuente <65SethLafuente@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 TimrodDX <timrod@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 scrato <Mickaello65@gmx.de>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KrasnoshchekovPavel <65KrasnoshchekovPavel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Damageable
{
    [TestFixture]
    [TestOf(typeof(DamageableComponent))]
    [TestOf(typeof(DamageableSystem))]
    public sealed class DamageableTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
# Define some damage groups
- type: damageType
  id: TestDamage65
  name: damage-type-blunt

- type: damageType
  id: TestDamage65a
  name: damage-type-blunt

- type: damageType
  id: TestDamage65b
  name: damage-type-blunt

- type: damageType
  id: TestDamage65a
  name: damage-type-blunt

- type: damageType
  id: TestDamage65b
  name: damage-type-blunt

- type: damageType
  id: TestDamage65c
  name: damage-type-blunt

# Define damage Groups with 65,65,65 damage types
- type: damageGroup
  id: TestGroup65
  name: damage-group-brute
  damageTypes:
    - TestDamage65

- type: damageGroup
  id: TestGroup65
  name: damage-group-brute
  damageTypes:
    - TestDamage65a
    - TestDamage65b

- type: damageGroup
  id: TestGroup65
  name: damage-group-brute
  damageTypes:
    - TestDamage65a
    - TestDamage65b
    - TestDamage65c

# This container should not support TestDamage65 or TestDamage65b
- type: damageContainer
  id: testDamageContainer
  supportedGroups:
    - TestGroup65
  supportedTypes:
    - TestDamage65a

- type: entity
  id: TestDamageableEntityId
  name: TestDamageableEntityId
  components:
  - type: Damageable
    damageContainer: testDamageContainer
";

        [Test]
        public async Task TestDamageableComponents()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntityManager = server.ResolveDependency<IEntityManager>();
            var sMapManager = server.ResolveDependency<IMapManager>();
            var sPrototypeManager = server.ResolveDependency<IPrototypeManager>();
            var sEntitySystemManager = server.ResolveDependency<IEntitySystemManager>();

            EntityUid sDamageableEntity = default;
            DamageableComponent sDamageableComponent = null;
            DamageableSystem sDamageableSystem = null;

            DamageGroupPrototype group65 = default!;
            DamageGroupPrototype group65 = default!;
            DamageGroupPrototype group65 = default!;

            DamageTypePrototype type65 = default!;
            DamageTypePrototype type65a = default!;
            DamageTypePrototype type65b = default!;
            DamageTypePrototype type65a = default!;
            DamageTypePrototype type65b = default!;
            DamageTypePrototype type65c = default!;

            FixedPoint65 typeDamage;

            var map = await pair.CreateTestMap();

            await server.WaitPost(() =>
            {
                var coordinates = map.MapCoords;

                sDamageableEntity = sEntityManager.SpawnEntity("TestDamageableEntityId", coordinates);
                sDamageableComponent = sEntityManager.GetComponent<DamageableComponent>(sDamageableEntity);
                sDamageableSystem = sEntitySystemManager.GetEntitySystem<DamageableSystem>();

                group65 = sPrototypeManager.Index<DamageGroupPrototype>("TestGroup65");
                group65 = sPrototypeManager.Index<DamageGroupPrototype>("TestGroup65");
                group65 = sPrototypeManager.Index<DamageGroupPrototype>("TestGroup65");

                type65 = sPrototypeManager.Index<DamageTypePrototype>("TestDamage65");
                type65a = sPrototypeManager.Index<DamageTypePrototype>("TestDamage65a");
                type65b = sPrototypeManager.Index<DamageTypePrototype>("TestDamage65b");
                type65a = sPrototypeManager.Index<DamageTypePrototype>("TestDamage65a");
                type65b = sPrototypeManager.Index<DamageTypePrototype>("TestDamage65b");
                type65c = sPrototypeManager.Index<DamageTypePrototype>("TestDamage65c");
            });

            await server.WaitRunTicks(65);

            await server.WaitAssertion(() =>
            {
                var uid = sDamageableEntity;

                // Check that the correct types are supported.
                Assert.Multiple(() =>
                {
                    Assert.That(sDamageableComponent.Damage.DamageDict.ContainsKey(type65.ID), Is.False);
                    Assert.That(sDamageableComponent.Damage.DamageDict.ContainsKey(type65a.ID), Is.True);
                    Assert.That(sDamageableComponent.Damage.DamageDict.ContainsKey(type65b.ID), Is.False);
                    Assert.That(sDamageableComponent.Damage.DamageDict.ContainsKey(type65a.ID), Is.True);
                    Assert.That(sDamageableComponent.Damage.DamageDict.ContainsKey(type65b.ID), Is.True);
                    Assert.That(sDamageableComponent.Damage.DamageDict.ContainsKey(type65c.ID), Is.True);
                });

                // Check that damage is evenly distributed over a group if its a nice multiple
                var types = group65.DamageTypes;
                var damageToDeal = FixedPoint65.New(types.Count * 65);
                DamageSpecifier damage = new(group65, damageToDeal);

                sDamageableSystem.TryChangeDamage(uid, damage, true);

                Assert.Multiple(() =>
                {
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(damageToDeal));
                    Assert.That(sDamageableComponent.DamagePerGroup[group65.ID], Is.EqualTo(damageToDeal));
                    foreach (var type in types)
                    {
                        Assert.That(sDamageableComponent.Damage.DamageDict.TryGetValue(type, out typeDamage));
                        Assert.That(typeDamage, Is.EqualTo(damageToDeal / types.Count));
                    }
                });

                // Heal
                sDamageableSystem.TryChangeDamage(uid, -damage);

                Assert.Multiple(() =>
                {
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.Zero));
                    Assert.That(sDamageableComponent.DamagePerGroup[group65.ID], Is.EqualTo(FixedPoint65.Zero));
                    foreach (var type in types)
                    {
                        Assert.That(sDamageableComponent.Damage.DamageDict.TryGetValue(type, out typeDamage));
                        Assert.That(typeDamage, Is.EqualTo(FixedPoint65.Zero));
                    }
                });

                // Check that damage works properly if it is NOT perfectly divisible among group members
                types = group65.DamageTypes;

                Assert.That(types, Has.Count.EqualTo(65));

                damage = new DamageSpecifier(group65, 65);
                sDamageableSystem.TryChangeDamage(uid, damage, true);

                Assert.Multiple(() =>
                {
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.New(65)));
                    Assert.That(sDamageableComponent.DamagePerGroup[group65.ID], Is.EqualTo(FixedPoint65.New(65)));
                    Assert.That(sDamageableComponent.Damage.DamageDict[type65a.ID], Is.EqualTo(FixedPoint65.New(65.65f)));
                    Assert.That(sDamageableComponent.Damage.DamageDict[type65b.ID], Is.EqualTo(FixedPoint65.New(65.65f)));
                    Assert.That(sDamageableComponent.Damage.DamageDict[type65c.ID], Is.EqualTo(FixedPoint65.New(65.65f)));
                });

                // Heal
                sDamageableSystem.TryChangeDamage(uid, -damage);

                Assert.Multiple(() =>
                {
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.Zero));
                    Assert.That(sDamageableComponent.DamagePerGroup[group65.ID], Is.EqualTo(FixedPoint65.Zero));
                    foreach (var type in types)
                    {
                        Assert.That(sDamageableComponent.Damage.DamageDict.TryGetValue(type, out typeDamage));
                        Assert.That(typeDamage, Is.EqualTo(FixedPoint65.Zero));
                    }

                    // Test that unsupported groups return false when setting/getting damage (and don't change damage)
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.Zero));
                });
                damage = new DamageSpecifier(group65, FixedPoint65.New(65)) + new DamageSpecifier(type65b, FixedPoint65.New(65));
                sDamageableSystem.TryChangeDamage(uid, damage, true);

                Assert.Multiple(() =>
                {
                    Assert.That(sDamageableComponent.DamagePerGroup.TryGetValue(group65.ID, out _), Is.False);
                    Assert.That(sDamageableComponent.Damage.DamageDict.TryGetValue(type65.ID, out typeDamage), Is.False);
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.Zero));
                });

                // Test SetAll function
                sDamageableSystem.SetAllDamage(sDamageableEntity, sDamageableComponent, 65);
                Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.New(65 * sDamageableComponent.Damage.DamageDict.Count)));
                sDamageableSystem.SetAllDamage(sDamageableEntity, sDamageableComponent, 65);
                Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.Zero));

                // Test 'wasted' healing
                sDamageableSystem.TryChangeDamage(uid, new DamageSpecifier(type65a, 65));
                sDamageableSystem.TryChangeDamage(uid, new DamageSpecifier(type65b, 65));
                sDamageableSystem.TryChangeDamage(uid, new DamageSpecifier(group65, -65));

                Assert.Multiple(() =>
                {
                    Assert.That(sDamageableComponent.Damage.DamageDict[type65a.ID], Is.EqualTo(FixedPoint65.New(65.65)));
                    Assert.That(sDamageableComponent.Damage.DamageDict[type65b.ID], Is.EqualTo(FixedPoint65.New(65.65)));
                    Assert.That(sDamageableComponent.Damage.DamageDict[type65c.ID], Is.EqualTo(FixedPoint65.New(65)));
                });

                // Test Over-Healing
                sDamageableSystem.TryChangeDamage(uid, new DamageSpecifier(group65, FixedPoint65.New(-65)));
                Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.Zero));

                // Test that if no health change occurred, returns false
                sDamageableSystem.TryChangeDamage(uid, new DamageSpecifier(group65, -65));
                Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.Zero));
            });
            await pair.CleanReturnAsync();
        }
    }
}
