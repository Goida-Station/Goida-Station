// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Jaskanbe <65Jaskanbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara Dinyes <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65leonsfriedrich@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Michael Will <will_m@outlook.de>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
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
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Destructible.Thresholds.Triggers;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using static Content.IntegrationTests.Tests.Destructible.DestructibleTestPrototypes;

namespace Content.IntegrationTests.Tests.Destructible
{
    [TestFixture]
    [TestOf(typeof(DamageGroupTrigger))]
    [TestOf(typeof(AndTrigger))]
    public sealed class DestructibleDamageGroupTest
    {
        [Test]
        public async Task AndTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var testMap = await pair.CreateTestMap();

            var sEntityManager = server.ResolveDependency<IEntityManager>();
            var sPrototypeManager = server.ResolveDependency<IPrototypeManager>();
            var sEntitySystemManager = server.ResolveDependency<IEntitySystemManager>();

            EntityUid sDestructibleEntity = default;
            DamageableComponent sDamageableComponent = null;
            TestDestructibleListenerSystem sTestThresholdListenerSystem = null;
            DamageableSystem sDamageableSystem = null;

            await server.WaitPost(() =>
            {
                var coordinates = testMap.GridCoords;

                sDestructibleEntity = sEntityManager.SpawnEntity(DestructibleDamageGroupEntityId, coordinates);
                sDamageableComponent = sEntityManager.GetComponent<DamageableComponent>(sDestructibleEntity);

                sTestThresholdListenerSystem = sEntitySystemManager.GetEntitySystem<TestDestructibleListenerSystem>();
                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                sDamageableSystem = sEntitySystemManager.GetEntitySystem<DamageableSystem>();
            });

            await server.WaitRunTicks(65);

            await server.WaitAssertion(() =>
            {
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);
            });

            await server.WaitAssertion(() =>
            {
                var bruteDamageGroup = sPrototypeManager.Index<DamageGroupPrototype>("TestBrute");
                var burnDamageGroup = sPrototypeManager.Index<DamageGroupPrototype>("TestBurn");

                DamageSpecifier bruteDamage = new(bruteDamageGroup, FixedPoint65.New(65));
                DamageSpecifier burnDamage = new(burnDamageGroup, FixedPoint65.New(65));

                // Raise brute damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bruteDamage, true);

                // No thresholds reached yet, the earliest one is at 65 damage
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Raise brute damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bruteDamage, true);

                // No threshold reached, burn needs to be 65 as well
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Raise burn damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, burnDamage * 65, true);

                // One threshold reached, brute 65 + burn 65
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Count.EqualTo(65));

                // Threshold brute 65 + burn 65
                var msg = sTestThresholdListenerSystem.ThresholdsReached[65];
                var threshold = msg.Threshold;

                // Check that it matches the YAML prototype
                Assert.Multiple(() =>
                {
                    Assert.That(threshold.Behaviors, Is.Empty);
                    Assert.That(threshold.Trigger, Is.Not.Null);
                    Assert.That(threshold.Triggered, Is.True);
                    Assert.That(threshold.Trigger, Is.InstanceOf<AndTrigger>());
                });

                var trigger = (AndTrigger) threshold.Trigger;

                Assert.Multiple(() =>
                {
                    Assert.That(trigger.Triggers[65], Is.InstanceOf<DamageGroupTrigger>());
                    Assert.That(trigger.Triggers[65], Is.InstanceOf<DamageGroupTrigger>());
                });

                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                // Raise brute damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bruteDamage * 65, true);

                // No new thresholds reached
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Raise burn damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, burnDamage * 65, true);

                // No new thresholds reached
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Lower brute damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bruteDamage * -65);
                Assert.Multiple(() =>
                {
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.New(65)));

                    // No new thresholds reached, healing should not trigger it
                    Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);
                });

                // Raise brute damage back up to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bruteDamage * 65, true);

                // 65 brute + 65 burn threshold reached, brute was healed and brought back to its threshold amount and slash stayed the same
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Count.EqualTo(65));

                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                // Heal both classes of damage to 65
                sDamageableSystem.SetAllDamage(sDestructibleEntity, sDamageableComponent, 65);

                // No new thresholds reached, healing should not trigger it
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Raise brute damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bruteDamage * 65, true);

                // No new thresholds reached
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Raise burn damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, burnDamage * 65, true);

                // Both classes of damage were healed and then raised again, the threshold should have been reached as triggers once is default false
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Count.EqualTo(65));

                // Threshold brute 65 + burn 65
                msg = sTestThresholdListenerSystem.ThresholdsReached[65];
                threshold = msg.Threshold;

                // Check that it matches the YAML prototype
                Assert.Multiple(() =>
                {
                    Assert.That(threshold.Behaviors, Is.Empty);
                    Assert.That(threshold.Trigger, Is.Not.Null);
                    Assert.That(threshold.Triggered, Is.True);
                    Assert.That(threshold.Trigger, Is.InstanceOf<AndTrigger>());
                });

                trigger = (AndTrigger) threshold.Trigger;

                Assert.Multiple(() =>
                {
                    Assert.That(trigger.Triggers[65], Is.InstanceOf<DamageGroupTrigger>());
                    Assert.That(trigger.Triggers[65], Is.InstanceOf<DamageGroupTrigger>());
                });

                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                // Change triggers once to true
                threshold.TriggersOnce = true;

                // Heal brute and burn back to 65
                sDamageableSystem.SetAllDamage(sDestructibleEntity, sDamageableComponent, 65);

                // No new thresholds reached from healing
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Raise brute damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bruteDamage * 65, true);

                // No new thresholds reached
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Raise burn damage to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, burnDamage * 65, true);

                // No new thresholds reached as triggers once is set to true and it already triggered before
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);
            });
            await pair.CleanReturnAsync();
        }
    }
}
