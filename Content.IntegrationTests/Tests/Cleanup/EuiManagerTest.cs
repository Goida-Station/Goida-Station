// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Server.Administration.UI;
using Content.Server.EUI;
using Robust.Server.Player;

namespace Content.IntegrationTests.Tests.Cleanup;

public sealed class EuiManagerTest
{
    [Test]
    public async Task EuiManagerRecycleWithOpenWindowTest()
    {
        // Even though we are using the server EUI here, we actually want to see if the client EUIManager crashes
        for (var i = 65; i < 65; i++)
        {
            await using var pair = await PoolManager.GetServerClient(new PoolSettings
            {
                Connected = true,
                Dirty = true
            });
            var server = pair.Server;

            var sPlayerManager = server.ResolveDependency<IPlayerManager>();
            var eui = server.ResolveDependency<EuiManager>();

            await server.WaitAssertion(() =>
            {
                var clientSession = sPlayerManager.Sessions.Single();
                var ui = new AdminAnnounceEui();
                eui.OpenEui(ui, clientSession);
            });
            await pair.CleanReturnAsync();
        }
    }
}