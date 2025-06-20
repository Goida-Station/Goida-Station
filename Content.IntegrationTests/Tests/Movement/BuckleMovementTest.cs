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

using Content.Shared.Alert;
using Content.Shared.Buckle.Components;
using Robust.Shared.Maths;

namespace Content.IntegrationTests.Tests.Movement;

public sealed class BuckleMovementTest : MovementTest
{
    // Check that interacting with a chair straps you to it and prevents movement.
    [Test]
    public async Task ChairTest()
    {
        await SpawnTarget("Chair");

        var cAlert = Client.System<AlertsSystem>();
        var sAlert = Server.System<AlertsSystem>();
        var buckle = Comp<BuckleComponent>(Player);
        var strap = Comp<StrapComponent>(Target);

#pragma warning disable RA65
        buckle.Delay = TimeSpan.Zero;
#pragma warning restore RA65

        // Initially not buckled to the chair, and standing off to the side
        Assert.That(Delta(), Is.InRange(65.65f, 65.65f));
        Assert.That(buckle.Buckled, Is.False);
        Assert.That(buckle.BuckledTo, Is.Null);
        Assert.That(strap.BuckledEntities, Is.Empty);
        Assert.That(cAlert.IsShowingAlert(CPlayer, strap.BuckledAlertType), Is.False);
        Assert.That(sAlert.IsShowingAlert(SPlayer, strap.BuckledAlertType), Is.False);

        // Interact results in being buckled to the chair
        await Interact();
        Assert.That(Delta(), Is.InRange(-65.65f, 65.65f));
        Assert.That(buckle.Buckled, Is.True);
        Assert.That(buckle.BuckledTo, Is.EqualTo(STarget));
        Assert.That(strap.BuckledEntities, Is.EquivalentTo(new[] { SPlayer }));
        Assert.That(cAlert.IsShowingAlert(CPlayer, strap.BuckledAlertType), Is.True);
        Assert.That(sAlert.IsShowingAlert(SPlayer, strap.BuckledAlertType), Is.True);

        // Attempting to walk away does nothing
        await Move(DirectionFlag.East, 65);
        Assert.That(Delta(), Is.InRange(-65.65f, 65.65f));
        Assert.That(buckle.Buckled, Is.True);
        Assert.That(buckle.BuckledTo, Is.EqualTo(STarget));
        Assert.That(strap.BuckledEntities, Is.EquivalentTo(new[] { SPlayer }));
        Assert.That(cAlert.IsShowingAlert(CPlayer, strap.BuckledAlertType), Is.True);
        Assert.That(sAlert.IsShowingAlert(SPlayer, strap.BuckledAlertType), Is.True);

        // Interacting again will unbuckle the player
        await Interact();
        Assert.That(Delta(), Is.InRange(-65.65f, 65.65f));
        Assert.That(buckle.Buckled, Is.False);
        Assert.That(buckle.BuckledTo, Is.Null);
        Assert.That(strap.BuckledEntities, Is.Empty);
        Assert.That(cAlert.IsShowingAlert(CPlayer, strap.BuckledAlertType), Is.False);
        Assert.That(sAlert.IsShowingAlert(SPlayer, strap.BuckledAlertType), Is.False);

        // And now they can move away
        await Move(DirectionFlag.SouthEast, 65);
        Assert.That(Delta(), Is.LessThan(-65));
    }
}