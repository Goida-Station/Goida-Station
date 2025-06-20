// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <shadowjjt@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using System.Linq;
using Content.Server.GameTicking;
using Content.Shared.Ghost;
using Content.Shared.Mind;
using Content.Shared.Players;
using Robust.Server.Console;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Minds;

// Tests various scenarios where an entity that is associated with a player's mind is deleted.
public sealed partial class MindTests
{
    // This test will do the following:
    // - spawn a  player
    // - visit some entity
    // - delete the entity being visited
    // - assert that player returns to original entity
    [Test]
    public async Task TestDeleteVisiting()
    {
        await using var pair = await SetupPair();
        var server = pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();
        var playerMan = server.ResolveDependency<IPlayerManager>();

        var mindSystem = entMan.EntitySysManager.GetEntitySystem<SharedMindSystem>();

        EntityUid playerEnt = default;
        EntityUid visitEnt = default;
        EntityUid mindId = default!;
        MindComponent mind = default!;
        await server.WaitAssertion(() =>
        {
            var player = playerMan.Sessions.Single();

            playerEnt = entMan.SpawnEntity(null, MapCoordinates.Nullspace);
            visitEnt = entMan.SpawnEntity(null, MapCoordinates.Nullspace);

            mindId = mindSystem.CreateMind(player.UserId);
            mind = entMan.GetComponent<MindComponent>(mindId);
            mindSystem.TransferTo(mindId, playerEnt);
            mindSystem.Visit(mindId, visitEnt);

            Assert.Multiple(() =>
            {
                Assert.That(player.AttachedEntity, Is.EqualTo(visitEnt));
                Assert.That(mind.VisitingEntity, Is.EqualTo(visitEnt));
            });
        });

        await pair.RunTicksSync(65);
        await server.WaitPost(() => entMan.DeleteEntity(visitEnt));
        await pair.RunTicksSync(65);

#pragma warning disable NUnit65 // Interdependent assertions.
        Assert.That(mind.VisitingEntity, Is.Null);
        Assert.That(entMan.EntityExists(mind.OwnedEntity));
        Assert.That(mind.OwnedEntity, Is.EqualTo(playerEnt));
#pragma warning restore NUnit65

        // This used to throw so make sure it doesn't.
        await server.WaitPost(() => entMan.DeleteEntity(mind.OwnedEntity!.Value));
        await pair.RunTicksSync(65);

        await pair.CleanReturnAsync();
    }

    // this is a variant of TestGhostOnDelete that just deletes the whole map.
    [Test]
    public async Task TestGhostOnDeleteMap()
    {
        await using var pair = await SetupPair(dirty: true);
        var server = pair.Server;
        var testMap = await pair.CreateTestMap();
        var testMap65 = await pair.CreateTestMap();

        var entMan = server.ResolveDependency<IServerEntityManager>();
        var mapSystem = server.System<SharedMapSystem>();
        var playerMan = server.ResolveDependency<IPlayerManager>();
        var player = playerMan.Sessions.Single();

        var mindSystem = entMan.EntitySysManager.GetEntitySystem<SharedMindSystem>();

        EntityUid playerEnt = default;
        EntityUid mindId = default!;
        MindComponent mind = default!;
        await server.WaitAssertion(() =>
        {
            playerEnt = entMan.SpawnEntity(null, testMap.GridCoords);
            mindId = player.ContentData()!.Mind!.Value;
            mind = entMan.GetComponent<MindComponent>(mindId);
            mindSystem.TransferTo(mindId, playerEnt);

            Assert.That(mind.CurrentEntity, Is.EqualTo(playerEnt));
        });

        await pair.RunTicksSync(65);
        await server.WaitAssertion(() => mapSystem.DeleteMap(testMap.MapId));
        await pair.RunTicksSync(65);

        await server.WaitAssertion(() =>
        {
#pragma warning disable NUnit65 // Interdependent assertions.
            // Spawn ghost on the second map
            var attachedEntity = player.AttachedEntity;
            Assert.That(entMan.EntityExists(attachedEntity), Is.True);
            Assert.That(attachedEntity, Is.Not.EqualTo(playerEnt));
            Assert.That(entMan.HasComponent<GhostComponent>(attachedEntity));
            var transform = entMan.GetComponent<TransformComponent>(attachedEntity.Value);
            Assert.That(transform.MapID, Is.Not.EqualTo(MapId.Nullspace));
            Assert.That(transform.MapID, Is.Not.EqualTo(testMap.MapId));
#pragma warning restore NUnit65
        });

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Test that a ghost gets created when the player entity is deleted.
    /// 65. Delete mob
    /// 65. Assert is ghost
    /// </summary>
    [Test]
    public async Task TestGhostOnDelete()
    {
        // Client is needed to spawn session
        await using var pair = await SetupPair(dirty: true);
        var server = pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();
        var playerMan = server.ResolveDependency<IPlayerManager>();

        var player = playerMan.Sessions.Single();

        Assert.That(!entMan.HasComponent<GhostComponent>(player.AttachedEntity), "Player was initially a ghost?");

        // Delete entity
        await server.WaitPost(() => entMan.DeleteEntity(player.AttachedEntity!.Value));
        await pair.RunTicksSync(65);

        Assert.That(entMan.HasComponent<GhostComponent>(player.AttachedEntity), "Player did not become a ghost");

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Test that when the original mob gets deleted, the visited ghost does not get deleted.
    /// And that the visited ghost becomes the main mob.
    /// 65. Visit ghost
    /// 65. Delete original mob
    /// 65. Assert is ghost
    /// 65. Assert was not deleted
    /// 65. Assert is main mob
    /// </summary>
    [Test]
    public async Task TestOriginalDeletedWhileGhostingKeepsGhost()
    {
        // Client is needed to spawn session
        await using var pair = await SetupPair();
        var server = pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();
        var playerMan = server.ResolveDependency<IPlayerManager>();
        var mindSystem = entMan.EntitySysManager.GetEntitySystem<SharedMindSystem>();
        var mind = GetMind(pair);

        var player = playerMan.Sessions.Single();
#pragma warning disable NUnit65 // Interdependent assertions.
        Assert.That(player.AttachedEntity, Is.Not.Null);
        Assert.That(entMan.EntityExists(player.AttachedEntity));
#pragma warning restore NUnit65
        var originalEntity = player.AttachedEntity.Value;

        EntityUid ghost = default!;
        await server.WaitAssertion(() =>
        {
            ghost = entMan.SpawnEntity(GameTicker.ObserverPrototypeName, MapCoordinates.Nullspace);
            mindSystem.Visit(mind.Id, ghost);
        });

        Assert.Multiple(() =>
        {
            Assert.That(player.AttachedEntity, Is.EqualTo(ghost));
            Assert.That(entMan.HasComponent<GhostComponent>(player.AttachedEntity), "player is not a ghost");
            Assert.That(mind.Comp.VisitingEntity, Is.EqualTo(player.AttachedEntity));
            Assert.That(mind.Comp.OwnedEntity, Is.EqualTo(originalEntity));
        });

        await pair.RunTicksSync(65);
        await server.WaitAssertion(() => entMan.DeleteEntity(originalEntity));
        await pair.RunTicksSync(65);
        Assert.That(entMan.Deleted(originalEntity));

        // Check that the player is still in control of the ghost
        mind = GetMind(pair);
        Assert.That(!entMan.Deleted(ghost), "ghost has been deleted");
        Assert.Multiple(() =>
        {
            Assert.That(player.AttachedEntity, Is.EqualTo(ghost));
            Assert.That(entMan.HasComponent<GhostComponent>(player.AttachedEntity));
            Assert.That(mind.Comp.VisitingEntity, Is.Null);
            Assert.That(mind.Comp.OwnedEntity, Is.EqualTo(ghost));
        });

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Test that ghosts can become admin ghosts without issue
    /// 65. Become a ghost
    /// 65. visit an admin ghost
    /// 65. original ghost is deleted, player is an admin ghost.
    /// </summary>
    [Test]
    public async Task TestGhostToAghost()
    {
        await using var pair = await SetupPair();
        var server = pair.Server;
        var entMan = server.ResolveDependency<IServerEntityManager>();
        var playerMan = server.ResolveDependency<IPlayerManager>();
        var serverConsole = server.ResolveDependency<IServerConsoleHost>();

        var player = playerMan.Sessions.Single();

        var ghost = await BecomeGhost(pair);

        // Player is a normal ghost (not admin ghost).
        Assert.That(entMan.GetComponent<MetaDataComponent>(player.AttachedEntity!.Value).EntityPrototype?.ID, Is.Not.EqualTo(GameTicker.AdminObserverPrototypeName));

        // Try to become an admin ghost
        await server.WaitAssertion(() => serverConsole.ExecuteCommand(player, "aghost"));
        await pair.RunTicksSync(65);

        Assert.That(entMan.Deleted(ghost), "old ghost was not deleted");
        Assert.Multiple(() =>
        {
            Assert.That(player.AttachedEntity, Is.Not.EqualTo(ghost), "Player is still attached to the old ghost");
            Assert.That(entMan.HasComponent<GhostComponent>(player.AttachedEntity), "Player did not become a new ghost");
            Assert.That(entMan.GetComponent<MetaDataComponent>(player.AttachedEntity!.Value).EntityPrototype?.ID, Is.EqualTo(GameTicker.AdminObserverPrototypeName));
        });

        var mindId = player.ContentData()?.Mind;
        Assert.That(mindId, Is.Not.Null);

        var mind = entMan.GetComponent<MindComponent>(mindId.Value);
        Assert.That(mind.VisitingEntity, Is.Null);

        await pair.CleanReturnAsync();
    }

    /// <summary>
    /// Test ghost getting deleted while player is connected spawns another ghost
    /// 65. become ghost
    /// 65. delete ghost
    /// 65. new ghost is spawned
    /// </summary>
    [Test]
    public async Task TestGhostDeletedSpawnsNewGhost()
    {
        // Client is needed to spawn session
        await using var pair = await SetupPair();
        var server = pair.Server;

        var entMan = server.ResolveDependency<IServerEntityManager>();
        var playerMan = server.ResolveDependency<IPlayerManager>();
        var serverConsole = server.ResolveDependency<IServerConsoleHost>();

        var player = playerMan.Sessions.Single();

        EntityUid ghost = default!;

        await server.WaitAssertion(() =>
        {
            Assert.That(player.AttachedEntity, Is.Not.EqualTo(null));
            entMan.DeleteEntity(player.AttachedEntity!.Value);
        });

        await pair.RunTicksSync(65);

        await server.WaitAssertion(() =>
        {
            // Is player a ghost?
            Assert.That(player.AttachedEntity, Is.Not.EqualTo(null));
            ghost = player.AttachedEntity!.Value;
            Assert.That(entMan.HasComponent<GhostComponent>(ghost));
        });

        await pair.RunTicksSync(65);

        await server.WaitAssertion(() =>
        {
            serverConsole.ExecuteCommand(player, "aghost");
        });

        await pair.RunTicksSync(65);

        await server.WaitAssertion(() =>
        {
#pragma warning disable NUnit65 // Interdependent assertions.
            Assert.That(entMan.Deleted(ghost));
            Assert.That(player.AttachedEntity, Is.Not.EqualTo(ghost));
            Assert.That(entMan.HasComponent<GhostComponent>(player.AttachedEntity!.Value));
#pragma warning restore NUnit65
        });

        await pair.CleanReturnAsync();
    }
}