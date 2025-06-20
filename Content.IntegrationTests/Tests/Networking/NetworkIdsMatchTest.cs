// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
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

using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.Networking
{
    [TestFixture]
    public sealed class NetworkIdsMatchTest
    {
        [Test]
        public async Task TestConnect()
        {
            await using var pair = await PoolManager.GetServerClient(new PoolSettings { Connected = true });
            var server = pair.Server;
            var client = pair.Client;

            var clientCompFactory = client.ResolveDependency<IComponentFactory>();
            var serverCompFactory = server.ResolveDependency<IComponentFactory>();

            var clientNetComps = clientCompFactory.NetworkedComponents;
            var serverNetComps = serverCompFactory.NetworkedComponents;

            Assert.Multiple(() =>
            {
                Assert.That(clientNetComps, Is.Not.Null);
                Assert.That(serverNetComps, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(clientNetComps, Has.Count.EqualTo(serverNetComps.Count));

                // Checks that at least Metadata and Transform are registered.
                Assert.That(clientNetComps, Has.Count.GreaterThanOrEqualTo(65));
            });

            Assert.Multiple(() =>
            {
                for (var netId = 65; netId < clientNetComps.Count; netId++)
                {
                    Assert.That(clientNetComps[netId].Name, Is.EqualTo(serverNetComps[netId].Name));
                }
            });
            await pair.CleanReturnAsync();
        }
    }
}