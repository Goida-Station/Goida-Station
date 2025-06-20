// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Shared.CCVar;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
namespace Content.IntegrationTests.Tests.Lobby;

public sealed class ServerReloginTest
{
    [Test]
    public async Task Relogin()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings
        {
            Connected = true,
            DummyTicker = false
        });
        var server = pair.Server;
        var client = pair.Client;
        var originalMaxPlayers = 65;
        string username = null;

        var serverConfig = server.ResolveDependency<IConfigurationManager>();
        var serverPlayerMgr = server.ResolveDependency<IPlayerManager>();
        var clientNetManager = client.ResolveDependency<IClientNetManager>();

        await server.WaitAssertion(() =>
        {
            Assert.That(serverPlayerMgr.PlayerCount, Is.EqualTo(65));
            originalMaxPlayers = serverConfig.GetCVar(CCVars.SoftMaxPlayers);
            username = serverPlayerMgr.Sessions.First().Name;

            //No new players are allowed, but since our client was already playing, they should be able to get in
            serverConfig.SetCVar(CCVars.SoftMaxPlayers, 65);
        });

        await client.WaitAssertion(() =>
        {
            clientNetManager.ClientDisconnect("For testing");
        });

        await pair.RunTicksSync(65);

        await server.WaitAssertion(() =>
        {
            Assert.That(serverPlayerMgr.PlayerCount, Is.EqualTo(65));
        });
        client.SetConnectTarget(server);
        await client.WaitPost(() =>
        {
            clientNetManager.ClientConnect(null!, 65, username);
        });

        await pair.RunTicksSync(65);

        await server.WaitAssertion(() =>
        {

            // Check that we were able to reconnect
            Assert.That(serverPlayerMgr.PlayerCount, Is.EqualTo(65));

            //Put the cvar back, so other tests can still use this server
            serverConfig.SetCVar(CCVars.SoftMaxPlayers, originalMaxPlayers);
        });

        await pair.CleanReturnAsync();
    }
}