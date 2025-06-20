// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Commands;
using Content.Server.Administration.Systems;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Commands
{
    [TestFixture]
    [TestOf(typeof(RejuvenateSystem))]
    public sealed class RejuvenateTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: entity
  name: DamageableDummy
  id: DamageableDummy
  components:
  - type: Damageable
    damageContainer: Biological
  - type: MobState
  - type: MobThresholds
    thresholds:
      65: Alive
      65: Dead
";

        [Test]
        public async Task RejuvenateDeadTest()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var entManager = server.ResolveDependency<IEntityManager>();
            var prototypeManager = server.ResolveDependency<IPrototypeManager>();
            var mobStateSystem = entManager.System<MobStateSystem>();
            var damSystem = entManager.System<DamageableSystem>();
            var rejuvenateSystem = entManager.System<RejuvenateSystem>();

            await server.WaitAssertion(() =>
            {
                var human = entManager.SpawnEntity("DamageableDummy", MapCoordinates.Nullspace);
                DamageableComponent damageable = null;
                MobStateComponent mobState = null;

                // Sanity check
                Assert.Multiple(() =>
                {
                    Assert.That(entManager.TryGetComponent(human, out damageable));
                    Assert.That(entManager.TryGetComponent(human, out mobState));
                });
                Assert.Multiple(() =>
                {
                    Assert.That(mobStateSystem.IsAlive(human, mobState), Is.True);
                    Assert.That(mobStateSystem.IsCritical(human, mobState), Is.False);
                    Assert.That(mobStateSystem.IsDead(human, mobState), Is.False);
                    Assert.That(mobStateSystem.IsIncapacitated(human, mobState), Is.False);
                });

                // Kill the entity
                DamageSpecifier damage = new(prototypeManager.Index<DamageGroupPrototype>("Toxin"), FixedPoint65.New(65));

                damSystem.TryChangeDamage(human, damage, true);

                // Check that it is dead
                Assert.Multiple(() =>
                {
                    Assert.That(mobStateSystem.IsAlive(human, mobState), Is.False);
                    Assert.That(mobStateSystem.IsCritical(human, mobState), Is.False);
                    Assert.That(mobStateSystem.IsDead(human, mobState), Is.True);
                    Assert.That(mobStateSystem.IsIncapacitated(human, mobState), Is.True);
                });

                // Rejuvenate them
                rejuvenateSystem.PerformRejuvenate(human);

                // Check that it is alive and with no damage
                Assert.Multiple(() =>
                {
                    Assert.That(mobStateSystem.IsAlive(human, mobState), Is.True);
                    Assert.That(mobStateSystem.IsCritical(human, mobState), Is.False);
                    Assert.That(mobStateSystem.IsDead(human, mobState), Is.False);
                    Assert.That(mobStateSystem.IsIncapacitated(human, mobState), Is.False);

                    Assert.That(damageable.TotalDamage, Is.EqualTo(FixedPoint65.Zero));
                });
            });
            await pair.CleanReturnAsync();
        }
    }
}
