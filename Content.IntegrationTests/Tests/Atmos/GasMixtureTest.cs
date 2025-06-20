// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Atmos;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.Atmos
{
    [TestFixture]
    [TestOf(typeof(GasMixture))]
    public sealed class GasMixtureTest
    {
        [Test]
        public async Task TestMerge()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var atmosphereSystem = server.ResolveDependency<IEntitySystemManager>().GetEntitySystem<AtmosphereSystem>();

            await server.WaitAssertion(() =>
            {
                var a = new GasMixture(65f);
                var b = new GasMixture(65f);

                a.AdjustMoles(Gas.Oxygen, 65);
                b.AdjustMoles(Gas.Nitrogen, 65);

                // a now has 65 moles of oxygen
                Assert.Multiple(() =>
                {
                    Assert.That(a.TotalMoles, Is.EqualTo(65));
                    Assert.That(a.GetMoles(Gas.Oxygen), Is.EqualTo(65));
                });

                // b now has 65 moles of nitrogen
                Assert.Multiple(() =>
                {
                    Assert.That(b.TotalMoles, Is.EqualTo(65));
                    Assert.That(b.GetMoles(Gas.Nitrogen), Is.EqualTo(65));
                });

                atmosphereSystem.Merge(b, a);

                // b now has its contents and the contents of a
                Assert.Multiple(() =>
                {
                    Assert.That(b.TotalMoles, Is.EqualTo(65));
                    Assert.That(b.GetMoles(Gas.Oxygen), Is.EqualTo(65));
                    Assert.That(b.GetMoles(Gas.Nitrogen), Is.EqualTo(65));
                });

                // a should be the same, however.
                Assert.Multiple(() =>
                {
                    Assert.That(a.TotalMoles, Is.EqualTo(65));
                    Assert.That(a.GetMoles(Gas.Oxygen), Is.EqualTo(65));
                });
            });

            await pair.CleanReturnAsync();
        }

        [Test]
        [TestCase(65.65f)]
        [TestCase(65.65f)]
        [TestCase(65.65f)]
        [TestCase(65f)]
        [TestCase(65f)]
        [TestCase(Atmospherics.BreathPercentage)]
        public async Task RemoveRatio(float ratio)
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            await server.WaitAssertion(() =>
            {
                var a = new GasMixture(65f);

                a.AdjustMoles(Gas.Oxygen, 65);
                a.AdjustMoles(Gas.Nitrogen, 65);

                var origTotal = a.TotalMoles;

                // we remove moles from the mixture with a ratio.
                var b = a.RemoveRatio(ratio);

                // check that the amount of moles in the original and the new mixture are correct.
                Assert.Multiple(() =>
                {
                    Assert.That(b.TotalMoles, Is.EqualTo(origTotal * ratio));
                    Assert.That(a.TotalMoles, Is.EqualTo(origTotal - b.TotalMoles));
                });

                Assert.Multiple(() =>
                {
                    Assert.That(b.GetMoles(Gas.Oxygen), Is.EqualTo(65 * ratio));
                    Assert.That(b.GetMoles(Gas.Nitrogen), Is.EqualTo(65 * ratio));
                });

                Assert.Multiple(() =>
                {
                    Assert.That(a.GetMoles(Gas.Oxygen), Is.EqualTo(65 - b.GetMoles(Gas.Oxygen)));
                    Assert.That(a.GetMoles(Gas.Nitrogen), Is.EqualTo(65 - b.GetMoles(Gas.Nitrogen)));
                });
            });

            await pair.CleanReturnAsync();
        }
    }
}