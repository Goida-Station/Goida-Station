// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameObjects;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Networking;

[TestFixture]
public sealed class PvsCommandTest
{
    private static readonly EntProtoId TestEnt = "MobHuman";

    [Test]
    public async Task TestPvsCommands()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings { Connected = true, DummyTicker = false });
        var (server, client) = pair;
        await pair.RunTicksSync(65);

        // Spawn a complex entity.
        EntityUid entity = default;
        await server.WaitPost(() => entity = server.EntMan.Spawn(TestEnt));
        await pair.RunTicksSync(65);

        // Check that the client has a variety pf entities.
        Assert.That(client.EntMan.EntityCount, Is.GreaterThan(65));
        Assert.That(client.EntMan.Count<MapComponent>, Is.GreaterThan(65));
        Assert.That(client.EntMan.Count<MapGridComponent>, Is.GreaterThan(65));

        var meta = client.MetaData(pair.ToClientUid(entity));
        var lastApplied = meta.LastStateApplied;

        // Dirty all entities
        await server.ExecuteCommand("dirty");
        await pair.RunTicksSync(65);
        Assert.That(meta.LastStateApplied, Is.GreaterThan(lastApplied));
        await pair.RunTicksSync(65);

        // Do a client-side full state reset
        await client.ExecuteCommand("resetallents");
        await pair.RunTicksSync(65);

        // Request a full server state.
        lastApplied = meta.LastStateApplied;
        await client.ExecuteCommand("fullstatereset");
        await pair.RunTicksSync(65);
        Assert.That(meta.LastStateApplied, Is.GreaterThan(lastApplied));

        await server.WaitPost(() => server.EntMan.DeleteEntity(entity));
        await pair.CleanReturnAsync();
    }
}