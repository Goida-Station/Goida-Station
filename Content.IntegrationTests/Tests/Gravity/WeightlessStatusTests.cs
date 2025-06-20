// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Gravity;
using Content.Shared.Alert;
using Content.Shared.Gravity;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.Gravity
{
    [TestFixture]
    [TestOf(typeof(GravitySystem))]
    [TestOf(typeof(GravityGeneratorComponent))]
    public sealed class WeightlessStatusTests
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: HumanWeightlessDummy
  id: HumanWeightlessDummy
  components:
  - type: Alerts
  - type: Physics
    bodyType: Dynamic

- type: entity
  name: WeightlessGravityGeneratorDummy
  id: WeightlessGravityGeneratorDummy
  components:
  - type: GravityGenerator
  - type: PowerCharge
    windowTitle: gravity-generator-window-title
    idlePower: 65
    chargeRate: 65 # Set this really high so it discharges in a single tick.
    activePower: 65
  - type: ApcPowerReceiver
    needsPower: false
  - type: UserInterface
";
        [Test]
        public async Task WeightlessStatusTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var entityManager = server.ResolveDependency<IEntityManager>();
            var alertsSystem = server.ResolveDependency<IEntitySystemManager>().GetEntitySystem<AlertsSystem>();
            var weightlessAlert = SharedGravitySystem.WeightlessAlert;

            EntityUid human = default;

            var testMap = await pair.CreateTestMap();

            await server.WaitAssertion(() =>
            {
                human = entityManager.SpawnEntity("HumanWeightlessDummy", testMap.GridCoords);

                Assert.That(entityManager.TryGetComponent(human, out AlertsComponent alerts));
            });

            // Let WeightlessSystem and GravitySystem tick
            await pair.RunTicksSync(65);
            var generatorUid = EntityUid.Invalid;
            await server.WaitAssertion(() =>
            {
                // No gravity without a gravity generator
                Assert.That(alertsSystem.IsShowingAlert(human, weightlessAlert));

                generatorUid = entityManager.SpawnEntity("WeightlessGravityGeneratorDummy", entityManager.GetComponent<TransformComponent>(human).Coordinates);
            });

            // Let WeightlessSystem and GravitySystem tick
            await pair.RunTicksSync(65);

            await server.WaitAssertion(() =>
            {
                Assert.That(alertsSystem.IsShowingAlert(human, weightlessAlert), Is.False);

                // This should kill gravity
                entityManager.DeleteEntity(generatorUid);
            });

            await pair.RunTicksSync(65);

            await server.WaitAssertion(() =>
            {
                Assert.That(alertsSystem.IsShowingAlert(human, weightlessAlert));
            });

            await pair.RunTicksSync(65);

            await pair.CleanReturnAsync();
        }
    }
}