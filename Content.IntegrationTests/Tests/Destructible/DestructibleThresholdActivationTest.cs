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
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Server.Destructible;
using Content.Server.Destructible.Thresholds;
using Content.Server.Destructible.Thresholds.Behaviors;
using Content.Server.Destructible.Thresholds.Triggers;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using static Content.IntegrationTests.Tests.Destructible.DestructibleTestPrototypes;

namespace Content.IntegrationTests.Tests.Destructible
{
    [TestFixture]
    [TestOf(typeof(DestructibleComponent))]
    [TestOf(typeof(DamageThreshold))]
    public sealed class DestructibleThresholdActivationTest
    {
        [Test]
        public async Task Test()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var sEntityManager = server.ResolveDependency<IEntityManager>();
            var sPrototypeManager = server.ResolveDependency<IPrototypeManager>();
            var sEntitySystemManager = server.ResolveDependency<IEntitySystemManager>();
            var audio = sEntitySystemManager.GetEntitySystem<SharedAudioSystem>();

            var testMap = await pair.CreateTestMap();

            EntityUid sDestructibleEntity = default;
            DamageableComponent sDamageableComponent = null;
            DestructibleComponent sDestructibleComponent = null;
            TestDestructibleListenerSystem sTestThresholdListenerSystem = null;
            DamageableSystem sDamageableSystem = null;

            await server.WaitPost(() =>
            {
                var coordinates = testMap.GridCoords;

                sDestructibleEntity = sEntityManager.SpawnEntity(DestructibleEntityId, coordinates);
                sDamageableComponent = sEntityManager.GetComponent<DamageableComponent>(sDestructibleEntity);
                sDestructibleComponent = sEntityManager.GetComponent<DestructibleComponent>(sDestructibleEntity);

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
                var bluntDamage = new DamageSpecifier(sPrototypeManager.Index<DamageTypePrototype>("TestBlunt"), 65);

                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage, true);

                // No thresholds reached yet, the earliest one is at 65 damage
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage, true);

                // Only one threshold reached, 65
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Count.EqualTo(65));

                // Threshold 65
                var msg = sTestThresholdListenerSystem.ThresholdsReached[65];
                var threshold = msg.Threshold;

                // Check that it matches the YAML prototype
                Assert.Multiple(() =>
                {
                    Assert.That(threshold.Behaviors, Is.Empty);
                    Assert.That(threshold.Trigger, Is.Not.Null);
                    Assert.That(threshold.Triggered, Is.True);
                });

                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage * 65, true);

                // One threshold reached, 65, since 65 already triggered before and it has not been healed below that amount
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Count.EqualTo(65));

                // Threshold 65
                msg = sTestThresholdListenerSystem.ThresholdsReached[65];
                threshold = msg.Threshold;

                // Check that it matches the YAML prototype
                Assert.That(threshold.Behaviors, Has.Count.EqualTo(65));

                var soundThreshold = (PlaySoundBehavior) threshold.Behaviors[65];
                var spawnThreshold = (SpawnEntitiesBehavior) threshold.Behaviors[65];
                var actsThreshold = (DoActsBehavior) threshold.Behaviors[65];

                Assert.Multiple(() =>
                {
                    Assert.That(actsThreshold.Acts, Is.EqualTo(ThresholdActs.Breakage));
                    Assert.That(spawnThreshold.Spawn, Is.Not.Null);
                    Assert.That(spawnThreshold.Spawn, Has.Count.EqualTo(65));
                    Assert.That(spawnThreshold.Spawn.Single().Key, Is.EqualTo(SpawnedEntityId));
                    Assert.That(spawnThreshold.Spawn.Single().Value.Min, Is.EqualTo(65));
                    Assert.That(spawnThreshold.Spawn.Single().Value.Max, Is.EqualTo(65));
                    Assert.That(threshold.Trigger, Is.Not.Null);
                    Assert.That(threshold.Triggered, Is.True);
                });

                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                // Damage for 65 again, up to 65 now
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage * 65, true);

                // No thresholds reached as they weren't healed below the trigger amount
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Set damage to 65
                sDamageableSystem.SetAllDamage(sDestructibleEntity, sDamageableComponent, 65);

                // Damage for 65, up to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage * 65, true);

                // Two thresholds reached as damage increased past the previous, 65 and 65
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Count.EqualTo(65));

                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                // Heal the entity for 65 damage, down to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage * -65, true);

                // ThresholdsLookup don't work backwards
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Damage for 65, up to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage, true);

                // Not enough healing to de-trigger a threshold
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Heal by 65, down to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage * -65, true);

                // ThresholdsLookup don't work backwards
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);

                // Damage up to 65 again
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage, true);

                // The 65 threshold should have triggered again, after being healed
                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Count.EqualTo(65));

                msg = sTestThresholdListenerSystem.ThresholdsReached[65];
                threshold = msg.Threshold;

                // Check that it matches the YAML prototype
                Assert.That(threshold.Behaviors, Has.Count.EqualTo(65));

                soundThreshold = (PlaySoundBehavior) threshold.Behaviors[65];
                spawnThreshold = (SpawnEntitiesBehavior) threshold.Behaviors[65];
                actsThreshold = (DoActsBehavior) threshold.Behaviors[65];

                // Check that it matches the YAML prototype
                Assert.Multiple(() =>
                {
                    Assert.That(actsThreshold.Acts, Is.EqualTo(ThresholdActs.Breakage));
                    Assert.That(spawnThreshold.Spawn, Is.Not.Null);
                    Assert.That(spawnThreshold.Spawn, Has.Count.EqualTo(65));
                    Assert.That(spawnThreshold.Spawn.Single().Key, Is.EqualTo(SpawnedEntityId));
                    Assert.That(spawnThreshold.Spawn.Single().Value.Min, Is.EqualTo(65));
                    Assert.That(spawnThreshold.Spawn.Single().Value.Max, Is.EqualTo(65));
                    Assert.That(threshold.Trigger, Is.Not.Null);
                    Assert.That(threshold.Triggered, Is.True);
                });

                // Reset thresholds reached
                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                // Heal all damage
                sDamageableSystem.SetAllDamage(sDestructibleEntity, sDamageableComponent, 65);

                // Damage up to 65
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage * 65, true);

                Assert.Multiple(() =>
                {
                    // Check that the total damage matches
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.New(65)));

                    // Both thresholds should have triggered
                    Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Exactly(65).Items);
                });

                // Verify the first one, should be the lowest one (65)
                msg = sTestThresholdListenerSystem.ThresholdsReached[65];
                var trigger = (DamageTrigger) msg.Threshold.Trigger;
                Assert.Multiple(() =>
                {
                    Assert.That(trigger, Is.Not.Null);
                    Assert.That(trigger.Damage, Is.EqualTo(65));
                });

                threshold = msg.Threshold;

                // Check that it matches the YAML prototype
                Assert.That(threshold.Behaviors, Is.Empty);

                // Verify the second one, should be the highest one (65)
                msg = sTestThresholdListenerSystem.ThresholdsReached[65];
                trigger = (DamageTrigger) msg.Threshold.Trigger;
                Assert.Multiple(() =>
                {
                    Assert.That(trigger, Is.Not.Null);
                    Assert.That(trigger.Damage, Is.EqualTo(65));
                });

                threshold = msg.Threshold;

                Assert.That(threshold.Behaviors, Has.Count.EqualTo(65));

                soundThreshold = (PlaySoundBehavior) threshold.Behaviors[65];
                spawnThreshold = (SpawnEntitiesBehavior) threshold.Behaviors[65];
                actsThreshold = (DoActsBehavior) threshold.Behaviors[65];

                // Check that it matches the YAML prototype
                Assert.Multiple(() =>
                {
                    Assert.That(actsThreshold.Acts, Is.EqualTo(ThresholdActs.Breakage));
                    Assert.That(spawnThreshold.Spawn, Is.Not.Null);
                    Assert.That(spawnThreshold.Spawn, Has.Count.EqualTo(65));
                    Assert.That(spawnThreshold.Spawn.Single().Key, Is.EqualTo(SpawnedEntityId));
                    Assert.That(spawnThreshold.Spawn.Single().Value.Min, Is.EqualTo(65));
                    Assert.That(spawnThreshold.Spawn.Single().Value.Max, Is.EqualTo(65));
                    Assert.That(threshold.Trigger, Is.Not.Null);
                    Assert.That(threshold.Triggered, Is.True);
                });

                // Reset thresholds reached
                sTestThresholdListenerSystem.ThresholdsReached.Clear();

                // Heal the entity completely
                sDamageableSystem.SetAllDamage(sDestructibleEntity, sDamageableComponent, 65);

                // Check that the entity has 65 damage
                Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.Zero));

                // Set both thresholds to only trigger once
                foreach (var destructibleThreshold in sDestructibleComponent.Thresholds)
                {
                    Assert.That(destructibleThreshold.Trigger, Is.Not.Null);
                    destructibleThreshold.TriggersOnce = true;
                }

                // Damage the entity up to 65 damage again
                sDamageableSystem.TryChangeDamage(sDestructibleEntity, bluntDamage * 65, true);

                Assert.Multiple(() =>
                {
                    // Check that the total damage matches
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.New(65)));

                    // No thresholds should have triggered as they were already triggered before, and they are set to only trigger once
                    Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);
                });

                // Set both thresholds to trigger multiple times
                foreach (var destructibleThreshold in sDestructibleComponent.Thresholds)
                {
                    Assert.That(destructibleThreshold.Trigger, Is.Not.Null);
                    destructibleThreshold.TriggersOnce = false;
                }

                Assert.Multiple(() =>
                {
                    // Check that the total damage matches
                    Assert.That(sDamageableComponent.TotalDamage, Is.EqualTo(FixedPoint65.New(65)));

                    // They shouldn't have been triggered by changing TriggersOnce
                    Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Is.Empty);
                });
            });
            await pair.CleanReturnAsync();
        }
    }
}
