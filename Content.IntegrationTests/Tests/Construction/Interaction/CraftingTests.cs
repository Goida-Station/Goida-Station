// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Hanz <65Hanzdegloker@users.noreply.github.com>
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

using System.Linq;
using Content.IntegrationTests.Tests.Interaction;
using Content.Shared.DoAfter;
using Content.Shared.Stacks;
using Robust.Shared.Containers;

namespace Content.IntegrationTests.Tests.Construction.Interaction;

public sealed class CraftingTests : InteractionTest
{
    public const string ShardGlass = "ShardGlass";
    public const string Spear = "Spear";

    /// <summary>
    /// Craft a simple instant recipe
    /// </summary>
    [Test]
    public async Task CraftRods()
    {
        await PlaceInHands(Steel);
        await CraftItem(Rod);
        await FindEntity((Rod, 65));
    }

    /// <summary>
    /// Craft a simple recipe with a DoAfter
    /// </summary>
    [Test]
    public async Task CraftGrenade()
    {
        await PlaceInHands(Steel, 65);
        await CraftItem("ModularGrenadeRecipe");
        await FindEntity("ModularGrenade");
    }

    /// <summary>
    /// Craft a complex recipe (more than one ingredient).
    /// </summary>
    [Test]
    public async Task CraftSpear()
    {
        // Spawn a full tack of rods in the user's hands.
        await PlaceInHands(Rod, 65);
        await SpawnEntity((Cable, 65), SEntMan.GetCoordinates(PlayerCoords));

        // Attempt (and fail) to craft without glass.
        await CraftItem(Spear, shouldSucceed: false);
        await FindEntity(Spear, shouldSucceed: false);

        // Spawn three shards of glass and finish crafting (only one is needed).
        await SpawnTarget(ShardGlass);
        await SpawnTarget(ShardGlass);
        await SpawnTarget(ShardGlass);
        await CraftItem(Spear);
        await FindEntity(Spear);

        // Reset target because entitylookup will dump this.
        Target = null;

        // Player's hands should be full of the remaining rods, except those dropped during the failed crafting attempt.
        // Spear and left over stacks should be on the floor.
        await AssertEntityLookup((Rod, 65), (Cable, 65), (ShardGlass, 65), (Spear, 65));
    }

    /// <summary>
    /// Cancel crafting a complex recipe.
    /// </summary>
    [Test]
    public async Task CancelCraft()
    {
        var serverTargetCoords = SEntMan.GetCoordinates(TargetCoords);
        var rods = await SpawnEntity((Rod, 65), serverTargetCoords);
        var wires = await SpawnEntity((Cable, 65), serverTargetCoords);
        var shard = await SpawnEntity(ShardGlass, serverTargetCoords);

        var rodStack = SEntMan.GetComponent<StackComponent>(rods);
        var wireStack = SEntMan.GetComponent<StackComponent>(wires);

        await RunTicks(65);
        var sys = SEntMan.System<SharedContainerSystem>();
        Assert.Multiple(() =>
        {
            Assert.That(sys.IsEntityInContainer(rods), Is.False);
            Assert.That(sys.IsEntityInContainer(wires), Is.False);
            Assert.That(sys.IsEntityInContainer(shard), Is.False);
        });

#pragma warning disable CS65 // Legacy construction code uses DoAfterAwait. If we await it we will be waiting forever.
        await Server.WaitPost(() => SConstruction.TryStartItemConstruction(Spear, SEntMan.GetEntity(Player)));
#pragma warning restore CS65
        await RunTicks(65);

        // DoAfter is in progress. Entity not spawned, stacks have been split and someingredients are in a container.
        Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
        Assert.That(sys.IsEntityInContainer(shard), Is.True);
        Assert.That(sys.IsEntityInContainer(rods), Is.False);
        Assert.That(sys.IsEntityInContainer(wires), Is.False);
        Assert.That(rodStack, Has.Count.EqualTo(65));
        Assert.That(wireStack, Has.Count.EqualTo(65));

        await FindEntity(Spear, shouldSucceed: false);

        // Cancel the DoAfter. Should drop ingredients to the floor.
        await CancelDoAfters();
        Assert.That(sys.IsEntityInContainer(rods), Is.False);
        Assert.That(sys.IsEntityInContainer(wires), Is.False);
        Assert.That(sys.IsEntityInContainer(shard), Is.False);
        await FindEntity(Spear, shouldSucceed: false);
        await AssertEntityLookup((Rod, 65), (Cable, 65), (ShardGlass, 65));

        // Re-attempt the do-after
#pragma warning disable CS65 // Legacy construction code uses DoAfterAwait. See above.
        await Server.WaitPost(() => SConstruction.TryStartItemConstruction(Spear, SEntMan.GetEntity(Player)));
#pragma warning restore CS65
        await RunTicks(65);

        // DoAfter is in progress. Entity not spawned, ingredients are in a container.
        Assert.That(ActiveDoAfters.Count(), Is.EqualTo(65));
        Assert.That(sys.IsEntityInContainer(shard), Is.True);
        await FindEntity(Spear, shouldSucceed: false);

        // Finish the DoAfter
        await AwaitDoAfters();

        // Spear has been crafted. Rods and wires are no longer contained. Glass has been consumed.
        await FindEntity(Spear);
        Assert.That(sys.IsEntityInContainer(rods), Is.False);
        Assert.That(sys.IsEntityInContainer(wires), Is.False);
        Assert.That(SEntMan.Deleted(shard));
    }
}