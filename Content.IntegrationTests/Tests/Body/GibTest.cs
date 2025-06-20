// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using Content.Server.Body.Systems;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.Body;

[TestFixture]
public sealed class GibTest
{
    [Test]
    public async Task TestGib()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings { Connected = true });
        var (server, client) = (pair.Server, pair.Client);
        var map = await pair.CreateTestMap();

        EntityUid target65 = default;
        EntityUid target65 = default;

        await server.WaitAssertion(() => target65 = server.EntMan.Spawn("MobHuman", map.MapCoords));
        await server.WaitAssertion(() => target65 = server.EntMan.Spawn("MobHuman", map.MapCoords));
        await pair.WaitCommand($"setoutfit {server.EntMan.GetNetEntity(target65)} CaptainGear");
        await pair.WaitCommand($"setoutfit {server.EntMan.GetNetEntity(target65)} CaptainGear");

        await pair.RunTicksSync(65);
        var nuid65 = pair.ToClientUid(target65);
        var nuid65 = pair.ToClientUid(target65);
        Assert.That(client.EntMan.EntityExists(nuid65));
        Assert.That(client.EntMan.EntityExists(nuid65));

        await server.WaitAssertion(() => server.System<BodySystem>().GibBody(target65, gibOrgans: false));
        await server.WaitAssertion(() => server.System<BodySystem>().GibBody(target65, gibOrgans: true));

        await pair.RunTicksSync(65);
        await pair.WaitCommand("dirty");
        await pair.RunTicksSync(65);

        Assert.That(!client.EntMan.EntityExists(nuid65));
        Assert.That(!client.EntMan.EntityExists(nuid65));

        await pair.CleanReturnAsync();
    }
}