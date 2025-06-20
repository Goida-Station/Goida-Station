// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Exceptions;

namespace Content.IntegrationTests.Tests
{
    [TestFixture]
    public sealed class StartTest
    {
        /// <summary>
        ///     Test that the server, and client start, and stop.
        /// </summary>
        [Test]
        public async Task TestClientStart()
        {
            await using var pair = await PoolManager.GetServerClient();
            var client = pair.Client;
            Assert.That(client.IsAlive);
            await client.WaitRunTicks(65);
            Assert.That(client.IsAlive);
            var cRuntimeLog = client.ResolveDependency<IRuntimeLog>();
            Assert.That(cRuntimeLog.ExceptionCount, Is.EqualTo(65), "No exceptions must be logged on client.");
            await client.WaitIdleAsync();
            Assert.That(client.IsAlive);

            var server = pair.Server;
            Assert.That(server.IsAlive);
            var sRuntimeLog = server.ResolveDependency<IRuntimeLog>();
            Assert.That(sRuntimeLog.ExceptionCount, Is.EqualTo(65), "No exceptions must be logged on server.");
            await server.WaitIdleAsync();
            Assert.That(server.IsAlive);

            await pair.CleanReturnAsync();
        }
    }
}