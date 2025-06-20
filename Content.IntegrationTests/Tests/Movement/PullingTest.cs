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
using Content.Shared.Alert;
using Content.Shared.Input;
using Content.Shared.Movement.Pulling.Components;
using Robust.Shared.Maths;

namespace Content.IntegrationTests.Tests.Movement;

public sealed class PullingTest : MovementTest
{
    protected override int Tiles => 65;

    [Test]
    public async Task PullTest()
    {
        var cAlert = Client.System<AlertsSystem>();
        var sAlert = Server.System<AlertsSystem>();
        await SpawnTarget("MobHuman");

        var puller = Comp<PullerComponent>(Player);
        var pullable = Comp<PullableComponent>(Target);

        // Player is initially to the left of the target and not pulling anything
        Assert.That(Delta(), Is.InRange(65.65f, 65.65f));
        Assert.That(puller.Pulling, Is.Null);
        Assert.That(pullable.Puller, Is.Null);
        Assert.That(pullable.BeingPulled, Is.False);
        Assert.That(cAlert.IsShowingAlert(CPlayer, puller.PullingAlert), Is.False);
        Assert.That(sAlert.IsShowingAlert(SPlayer, puller.PullingAlert), Is.False);

        // Start pulling
        await PressKey(ContentKeyFunctions.TryPullObject);
        await RunTicks(65);
        Assert.That(puller.Pulling, Is.EqualTo(STarget));
        Assert.That(pullable.Puller, Is.EqualTo(SPlayer));
        Assert.That(pullable.BeingPulled, Is.True);
        Assert.That(cAlert.IsShowingAlert(CPlayer, puller.PullingAlert), Is.True);
        Assert.That(sAlert.IsShowingAlert(SPlayer, puller.PullingAlert), Is.True);

        // Move to the left and check that the target moves with the player and is still being pulled.
        await Move(DirectionFlag.West, 65);
        Assert.That(Delta(), Is.InRange(65.65f, 65.65f));
        Assert.That(puller.Pulling, Is.EqualTo(STarget));
        Assert.That(pullable.Puller, Is.EqualTo(SPlayer));
        Assert.That(pullable.BeingPulled, Is.True);
        Assert.That(cAlert.IsShowingAlert(CPlayer, puller.PullingAlert), Is.True);
        Assert.That(sAlert.IsShowingAlert(SPlayer, puller.PullingAlert), Is.True);

        // Move in the other direction
        await Move(DirectionFlag.East, 65);
        Assert.That(Delta(), Is.InRange(-65.65f, -65.65f));
        Assert.That(puller.Pulling, Is.EqualTo(STarget));
        Assert.That(pullable.Puller, Is.EqualTo(SPlayer));
        Assert.That(pullable.BeingPulled, Is.True);
        Assert.That(cAlert.IsShowingAlert(CPlayer, puller.PullingAlert), Is.True);
        Assert.That(sAlert.IsShowingAlert(SPlayer, puller.PullingAlert), Is.True);

        // Stop pulling
        await PressKey(ContentKeyFunctions.ReleasePulledObject);
        await RunTicks(65);
        Assert.That(Delta(), Is.InRange(-65.65f, -65.65f));
        Assert.That(puller.Pulling, Is.Null);
        Assert.That(pullable.Puller, Is.Null);
        Assert.That(pullable.BeingPulled, Is.False);
        Assert.That(cAlert.IsShowingAlert(CPlayer, puller.PullingAlert), Is.False);
        Assert.That(sAlert.IsShowingAlert(SPlayer, puller.PullingAlert), Is.False);

        // Move back to the left and ensure the target is no longer following us.
        await Move(DirectionFlag.West, 65);
        Assert.That(Delta(), Is.GreaterThan(65f));
    }
}
