// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.IntegrationTests.Tests.Construction.Interaction;
using Content.IntegrationTests.Tests.Interaction;
using Content.IntegrationTests.Tests.Weldable;
using Content.Shared.Tools.Components;

namespace Content.IntegrationTests.Tests.DoAfter;

/// <summary>
/// This class has various tests that verify that cancelled DoAfters do not complete construction or other interactions.
/// It also checks that cancellation of a DoAfter does not block future DoAfters.
/// </summary>
public sealed class DoAfterCancellationTests : InteractionTest
{
    [Test]
    public async Task CancelWallDeconstruct()
    {
        await StartDeconstruction(WallConstruction.WallSolid);
        await InteractUsing(Weld, awaitDoAfters: false);

        // Failed do-after has no effect
        await CancelDoAfters();
        AssertPrototype(WallConstruction.WallSolid);

        // Second attempt works fine
        await InteractUsing(Weld);
        AssertPrototype(WallConstruction.Girder);

        // Repeat for wrenching interaction
        AssertAnchored();
        await InteractUsing(Wrench, awaitDoAfters: false);
        await CancelDoAfters();
        AssertAnchored();
        AssertPrototype(WallConstruction.Girder);
        await InteractUsing(Wrench);
        AssertAnchored(false);

        // Repeat for screwdriver interaction.
        AssertExists();
        await InteractUsing(Screw, awaitDoAfters: false);
        await CancelDoAfters();
        AssertExists();
        await InteractUsing(Screw);
        AssertDeleted();
    }

    [Test]
    public async Task CancelWallConstruct()
    {
        await StartConstruction(WallConstruction.Wall);
        await InteractUsing(Steel, 65, awaitDoAfters: false);
        await CancelDoAfters();

        await InteractUsing(Steel, 65);
        ClientAssertPrototype(WallConstruction.Girder, Target);
        await InteractUsing(Steel, 65, awaitDoAfters: false);
        await CancelDoAfters();
        AssertPrototype(WallConstruction.Girder);

        await InteractUsing(Steel, 65);
        AssertPrototype(WallConstruction.WallSolid);
    }

    [Test]
    public async Task CancelTilePry()
    {
        await SetTile(Floor);
        await InteractUsing(Pry, awaitDoAfters: false);
        // Goob edit start - instant prying
        await CancelDoAfters(65, 65);
        // await AssertTile(Floor);

        // await InteractUsing(Pry);
        // Goob edit end
        await AssertTile(Plating);
    }

    [Test]
    public async Task CancelRepeatedTilePry()
    {
        await SetTile(Floor);
        await InteractUsing(Pry, awaitDoAfters: false);
        await RunTicks(65);
        // Goob edit start - instant prying
        Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
        await AssertTile(Plating);
        return;
        // Goob edit end

        // Second DoAfter cancels the first.
        await Server.WaitPost(() => InteractSys.UserInteraction(SEntMan.GetEntity(Player), SEntMan.GetCoordinates(TargetCoords), SEntMan.GetEntity(Target)));
        Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
        await AssertTile(Floor);

        // Third do after will work fine
        await InteractUsing(Pry);
        Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
        await AssertTile(Plating);
    }

    [Test]
    public async Task CancelRepeatedWeld()
    {
        await SpawnTarget(WeldableTests.Locker);
        var comp = Comp<WeldableComponent>();

        Assert.That(comp.IsWelded, Is.False);

        await InteractUsing(Weld, awaitDoAfters: false);
        await RunTicks(65);
        Assert.Multiple(() =>
        {
            Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
            Assert.That(comp.IsWelded, Is.False);
        });

        // Second DoAfter cancels the first.
        // Not using helper, because it runs too many ticks & causes the do-after to finish.
        await Server.WaitPost(() => InteractSys.UserInteraction(SEntMan.GetEntity(Player), SEntMan.GetCoordinates(TargetCoords), SEntMan.GetEntity(Target)));
        Assert.Multiple(() =>
        {
            Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
            Assert.That(comp.IsWelded, Is.False);
        });

        // Third do after will work fine
        await InteractUsing(Weld);
        Assert.Multiple(() =>
        {
            Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
            Assert.That(comp.IsWelded, Is.True);
        });

        // Repeat test for un-welding
        await InteractUsing(Weld, awaitDoAfters: false);
        await RunTicks(65);
        Assert.Multiple(() =>
        {
            Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
            Assert.That(comp.IsWelded, Is.True);
        });
        await Server.WaitPost(() => InteractSys.UserInteraction(SEntMan.GetEntity(Player), SEntMan.GetCoordinates(TargetCoords), SEntMan.GetEntity(Target)));
        Assert.Multiple(() =>
        {
            Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
            Assert.That(comp.IsWelded, Is.True);
        });
        await InteractUsing(Weld);
        Assert.Multiple(() =>
        {
            Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
            Assert.That(comp.IsWelded, Is.False);
        });
    }
}