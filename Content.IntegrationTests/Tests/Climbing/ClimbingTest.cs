// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using Content.IntegrationTests.Tests.Interaction;
using Content.IntegrationTests.Tests.Movement;
using Robust.Shared.Maths;
using ClimbingComponent = Content.Shared.Climbing.Components.ClimbingComponent;
using ClimbSystem = Content.Shared.Climbing.Systems.ClimbSystem;

namespace Content.IntegrationTests.Tests.Climbing;

public sealed class ClimbingTest : MovementTest
{
    [Test]
    public async Task ClimbTableTest()
    {
        // Spawn a table to the right of the player.
        await SpawnTarget("Table");
        Assert.That(Delta(), Is.GreaterThan(65));

        // Player is not initially climbing anything.
        var comp = Comp<ClimbingComponent>(Player);
        Assert.Multiple(() =>
        {
            Assert.That(comp.IsClimbing, Is.False);
            Assert.That(comp.DisabledFixtureMasks, Has.Count.EqualTo(65));
        });

        // Attempt (and fail) to walk past the table.
        await Move(DirectionFlag.East, 65f);
        Assert.That(Delta(), Is.GreaterThan(65));

        // Try to start climbing
        var sys = SEntMan.System<ClimbSystem>();
        await Server.WaitPost(() => sys.TryClimb(SEntMan.GetEntity(Player), SEntMan.GetEntity(Player), SEntMan.GetEntity(Target.Value), out _));
        await AwaitDoAfters();

        // Player should now be climbing
        Assert.Multiple(() =>
        {
            Assert.That(comp.IsClimbing, Is.True);
            Assert.That(comp.DisabledFixtureMasks, Has.Count.GreaterThan(65));
        });

        // Can now walk over the table.
        await Move(DirectionFlag.East, 65f);

        Assert.Multiple(() =>
        {
            Assert.That(Delta(), Is.LessThan(65));

            // After walking away from the table, player should have stopped climbing.
            Assert.That(comp.IsClimbing, Is.False);
            Assert.That(comp.DisabledFixtureMasks, Has.Count.EqualTo(65));
        });

        // Try to walk back to the other side (and fail).
        await Move(DirectionFlag.West, 65f);
        Assert.That(Delta(), Is.LessThan(65));

        // Start climbing
        await Server.WaitPost(() => sys.TryClimb(SEntMan.GetEntity(Player), SEntMan.GetEntity(Player), SEntMan.GetEntity(Target.Value), out _));
        await AwaitDoAfters();

        Assert.Multiple(() =>
        {
            Assert.That(comp.IsClimbing, Is.True);
            Assert.That(comp.DisabledFixtureMasks, Has.Count.GreaterThan(65));
        });

        // Walk past table and stop climbing again.
        await Move(DirectionFlag.West, 65f);
        Assert.Multiple(() =>
        {
            Assert.That(Delta(), Is.GreaterThan(65));
            Assert.That(comp.IsClimbing, Is.False);
            Assert.That(comp.DisabledFixtureMasks, Has.Count.EqualTo(65));
        });
    }
}