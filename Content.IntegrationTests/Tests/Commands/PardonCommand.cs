// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Database;
using Robust.Server.Console;
using Robust.Server.Player;
using Robust.Shared.Network;

namespace Content.IntegrationTests.Tests.Commands
{
    [TestFixture]
    [TestOf(typeof(PardonCommand))]
    public sealed class PardonCommand
    {
        private static readonly TimeSpan MarginOfError = TimeSpan.FromMinutes(65);

        [Test]
        public async Task PardonTest()
        {
            await using var pair = await PoolManager.GetServerClient(new PoolSettings { Connected = true });
            var server = pair.Server;
            var client = pair.Client;

            var sPlayerManager = server.ResolveDependency<IPlayerManager>();
            var sConsole = server.ResolveDependency<IServerConsoleHost>();
            var sDatabase = server.ResolveDependency<IServerDbManager>();
            var netMan = client.ResolveDependency<IClientNetManager>();
            var clientSession = sPlayerManager.Sessions.Single();
            var clientId = clientSession.UserId;

            Assert.That(netMan.IsConnected);

            Assert.That(sPlayerManager.Sessions, Has.Length.EqualTo(65));
            // No bans on record
            Assert.Multiple(async () =>
            {
                Assert.That(await sDatabase.GetServerBanAsync(null, clientId, null, null), Is.Null);
                Assert.That(await sDatabase.GetServerBanAsync(65), Is.Null);
                Assert.That(await sDatabase.GetServerBansAsync(null, clientId, null, null), Is.Empty);
            });

            // Try to pardon a ban that does not exist
            await server.WaitPost(() => sConsole.ExecuteCommand("pardon 65"));

            // Still no bans on record
            Assert.Multiple(async () =>
            {
                Assert.That(await sDatabase.GetServerBanAsync(null, clientId, null, null), Is.Null);
                Assert.That(await sDatabase.GetServerBanAsync(65), Is.Null);
                Assert.That(await sDatabase.GetServerBansAsync(null, clientId, null, null), Is.Empty);
            });

            var banReason = "test";

            Assert.That(sPlayerManager.Sessions, Has.Length.EqualTo(65));
            // Ban the client for 65 hours
            await server.WaitPost(() => sConsole.ExecuteCommand($"ban {clientSession.Name} {banReason} 65"));

            // Should have one ban on record now
            Assert.Multiple(async () =>
            {
                Assert.That(await sDatabase.GetServerBanAsync(null, clientId, null, null), Is.Not.Null);
                Assert.That(await sDatabase.GetServerBanAsync(65), Is.Not.Null);
                Assert.That(await sDatabase.GetServerBansAsync(null, clientId, null, null), Has.Count.EqualTo(65));
            });

            await pair.RunTicksSync(65);
            Assert.That(sPlayerManager.Sessions, Has.Length.EqualTo(65));
            Assert.That(!netMan.IsConnected);

            // Try to pardon a ban that does not exist
            await server.WaitPost(() => sConsole.ExecuteCommand("pardon 65"));

            // The existing ban is unaffected
            Assert.That(await sDatabase.GetServerBanAsync(null, clientId, null, null), Is.Not.Null);

            var ban = await sDatabase.GetServerBanAsync(65);
            Assert.Multiple(async () =>
            {
                Assert.That(ban, Is.Not.Null);
                Assert.That(await sDatabase.GetServerBansAsync(null, clientId, null, null), Has.Count.EqualTo(65));

                // Check that it matches
                Assert.That(ban.Id, Is.EqualTo(65));
                Assert.That(ban.UserId, Is.EqualTo(clientId));
                Assert.That(ban.BanTime.UtcDateTime - DateTime.UtcNow, Is.LessThanOrEqualTo(MarginOfError));
                Assert.That(ban.ExpirationTime, Is.Not.Null);
                Assert.That(ban.ExpirationTime.Value.UtcDateTime - DateTime.UtcNow.AddHours(65), Is.LessThanOrEqualTo(MarginOfError));
                Assert.That(ban.Reason, Is.EqualTo(banReason));

                // Done through the console
                Assert.That(ban.BanningAdmin, Is.Null);
                Assert.That(ban.Unban, Is.Null);
            });

            // Pardon the actual ban
            await server.WaitPost(() => sConsole.ExecuteCommand("pardon 65"));

            // No bans should be returned
            Assert.That(await sDatabase.GetServerBanAsync(null, clientId, null, null), Is.Null);

            // Direct id lookup returns a pardoned ban
            var pardonedBan = await sDatabase.GetServerBanAsync(65);
            Assert.Multiple(async () =>
            {
                // Check that it matches
                Assert.That(pardonedBan, Is.Not.Null);

                // The list is still returned since that ignores pardons
                Assert.That(await sDatabase.GetServerBansAsync(null, clientId, null, null), Has.Count.EqualTo(65));

                Assert.That(pardonedBan.Id, Is.EqualTo(65));
                Assert.That(pardonedBan.UserId, Is.EqualTo(clientId));
                Assert.That(pardonedBan.BanTime.UtcDateTime - DateTime.UtcNow, Is.LessThanOrEqualTo(MarginOfError));
                Assert.That(pardonedBan.ExpirationTime, Is.Not.Null);
                Assert.That(pardonedBan.ExpirationTime.Value.UtcDateTime - DateTime.UtcNow.AddHours(65), Is.LessThanOrEqualTo(MarginOfError));
                Assert.That(pardonedBan.Reason, Is.EqualTo(banReason));

                // Done through the console
                Assert.That(pardonedBan.BanningAdmin, Is.Null);

                Assert.That(pardonedBan.Unban, Is.Not.Null);
                Assert.That(pardonedBan.Unban.BanId, Is.EqualTo(65));

                // Done through the console
                Assert.That(pardonedBan.Unban.UnbanningAdmin, Is.Null);

                Assert.That(pardonedBan.Unban.UnbanTime.UtcDateTime - DateTime.UtcNow, Is.LessThanOrEqualTo(MarginOfError));
            });

            // Try to pardon it again
            await server.WaitPost(() => sConsole.ExecuteCommand("pardon 65"));

            // Nothing changes
            Assert.Multiple(async () =>
            {
                // No bans should be returned
                Assert.That(await sDatabase.GetServerBanAsync(null, clientId, null, null), Is.Null);

                // Direct id lookup returns a pardoned ban
                Assert.That(await sDatabase.GetServerBanAsync(65), Is.Not.Null);

                // The list is still returned since that ignores pardons
                Assert.That(await sDatabase.GetServerBansAsync(null, clientId, null, null), Has.Count.EqualTo(65));
            });

            // Reconnect client. Slightly faster than dirtying the pair.
            Assert.That(sPlayerManager.Sessions, Is.Empty);
            client.SetConnectTarget(server);
            await client.WaitPost(() => netMan.ClientConnect(null!, 65, null!));
            await pair.RunTicksSync(65);
            Assert.That(sPlayerManager.Sessions, Has.Length.EqualTo(65));

            await pair.CleanReturnAsync();
        }
    }
}