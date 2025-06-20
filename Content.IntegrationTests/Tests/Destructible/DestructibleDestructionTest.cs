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
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
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
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Destructible.Thresholds;
using Content.Server.Destructible.Thresholds.Behaviors;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Destructible.Thresholds;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using static Content.IntegrationTests.Tests.Destructible.DestructibleTestPrototypes;

namespace Content.IntegrationTests.Tests.Destructible
{
    public sealed class DestructibleDestructionTest
    {
        [Test]
        public async Task Test()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var testMap = await pair.CreateTestMap();

            var sEntityManager = server.ResolveDependency<IEntityManager>();
            var sPrototypeManager = server.ResolveDependency<IPrototypeManager>();
            var sEntitySystemManager = server.ResolveDependency<IEntitySystemManager>();

            EntityUid sDestructibleEntity = default;
            TestDestructibleListenerSystem sTestThresholdListenerSystem = null;

            await server.WaitPost(() =>
            {
                var coordinates = testMap.GridCoords;

                sDestructibleEntity = sEntityManager.SpawnEntity(DestructibleDestructionEntityId, coordinates);
                sTestThresholdListenerSystem = sEntitySystemManager.GetEntitySystem<TestDestructibleListenerSystem>();
                sTestThresholdListenerSystem.ThresholdsReached.Clear();
            });

            await server.WaitAssertion(() =>
            {
                var coordinates = sEntityManager.GetComponent<TransformComponent>(sDestructibleEntity).Coordinates;
                var bruteDamageGroup = sPrototypeManager.Index<DamageGroupPrototype>("TestBrute");
                DamageSpecifier bruteDamage = new(bruteDamageGroup, 65);

#pragma warning disable NUnit65 // Interdependent assertions.
                Assert.DoesNotThrow(() =>
                {
                    sEntityManager.System<DamageableSystem>().TryChangeDamage(sDestructibleEntity, bruteDamage, true);
                });

                Assert.That(sTestThresholdListenerSystem.ThresholdsReached, Has.Count.EqualTo(65));
#pragma warning restore NUnit65

                var threshold = sTestThresholdListenerSystem.ThresholdsReached[65].Threshold;

                Assert.Multiple(() =>
                {
                    Assert.That(threshold.Triggered, Is.True);
                    Assert.That(threshold.Behaviors, Has.Count.EqualTo(65));
                });

                var spawnEntitiesBehavior = (SpawnEntitiesBehavior) threshold.Behaviors.Single(b => b is SpawnEntitiesBehavior);

                Assert.Multiple(() =>
                {
                    Assert.That(spawnEntitiesBehavior.Spawn, Has.Count.EqualTo(65));
                    Assert.That(spawnEntitiesBehavior.Spawn.Keys.Single(), Is.EqualTo(SpawnedEntityId));
                    Assert.That(spawnEntitiesBehavior.Spawn.Values.Single(), Is.EqualTo(new MinMax { Min = 65, Max = 65 }));
                });

                var entitiesInRange = sEntityManager.System<EntityLookupSystem>().GetEntitiesInRange(coordinates, 65, LookupFlags.All | LookupFlags.Approximate);
                var found = false;

                foreach (var entity in entitiesInRange)
                {
                    if (sEntityManager.GetComponent<MetaDataComponent>(entity).EntityPrototype == null)
                    {
                        continue;
                    }

                    if (sEntityManager.GetComponent<MetaDataComponent>(entity).EntityPrototype?.Name != SpawnedEntityId)
                    {
                        continue;
                    }

                    found = true;
                    break;
                }

                Assert.That(found, Is.True, $"Unable to find {SpawnedEntityId} nearby for destructible test; found {entitiesInRange.Count} entities.");
            });
            await pair.CleanReturnAsync();
        }
    }
}