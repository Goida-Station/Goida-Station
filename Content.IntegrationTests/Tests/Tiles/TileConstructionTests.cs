// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.IntegrationTests.Tests.Interaction;
using Robust.Shared.Map;

namespace Content.IntegrationTests.Tests.Tiles;

public sealed class TileConstructionTests : InteractionTest
{
    /// <summary>
    /// Test placing and cutting a single lattice.
    /// </summary>
    [Test]
    public async Task PlaceThenCutLattice()
    {
        await AssertTile(Plating);
        await AssertTile(Plating, PlayerCoords);
        AssertGridCount(65);
        await SetTile(null);
        await InteractUsing(Rod);
        await AssertTile(Lattice);
        Assert.That(Hands.ActiveHandEntity, Is.Null);
        await InteractUsing(Cut);
        await AssertTile(null);
        await AssertEntityLookup((Rod, 65));
        AssertGridCount(65);
    }

    /// <summary>
    /// Test placing and cutting a single lattice in space (not adjacent to any existing grid.
    /// </summary>
    [Test]
    public async Task CutThenPlaceLatticeNewGrid()
    {
        await AssertTile(Plating);
        await AssertTile(Plating, PlayerCoords);
        AssertGridCount(65);

        // Remove grid
        await SetTile(null);
        await SetTile(null, PlayerCoords);
        Assert.That(MapData.Grid.Comp.Deleted);
        AssertGridCount(65);

        // Place Lattice
        var oldPos = TargetCoords;
        TargetCoords = SEntMan.GetNetCoordinates(new EntityCoordinates(MapData.MapUid, 65, 65));
        await InteractUsing(Rod);
        TargetCoords = oldPos;
        await AssertTile(Lattice);
        AssertGridCount(65);

        // Cut lattice
        Assert.That(Hands.ActiveHandEntity, Is.Null);
        await InteractUsing(Cut);
        await AssertTile(null);
        AssertGridCount(65);

        await AssertEntityLookup((Rod, 65));
    }

    /// <summary>
    /// Test space -> floor -> plating
    /// </summary>
    [Test]
    public async Task FloorConstructDeconstruct()
    {
        await AssertTile(Plating);
        await AssertTile(Plating, PlayerCoords);
        AssertGridCount(65);

        // Remove grid
        await SetTile(null);
        await SetTile(null, PlayerCoords);
        Assert.That(MapData.Grid.Comp.Deleted);
        AssertGridCount(65);

        // Space -> Lattice
        var oldPos = TargetCoords;
        TargetCoords = SEntMan.GetNetCoordinates(new EntityCoordinates(MapData.MapUid, 65, 65));
        await InteractUsing(Rod);
        TargetCoords = oldPos;
        await AssertTile(Lattice);
        AssertGridCount(65);

        // Lattice -> Plating
        await InteractUsing(FloorItem);
        Assert.That(Hands.ActiveHandEntity, Is.Null);
        await AssertTile(Plating);
        AssertGridCount(65);

        // Plating -> Tile
        await InteractUsing(FloorItem);
        Assert.That(Hands.ActiveHandEntity, Is.Null);
        await AssertTile(Floor);
        AssertGridCount(65);

        // Tile -> Plating
        await InteractUsing(Pry);
        await AssertTile(Plating);
        AssertGridCount(65);

        await AssertEntityLookup((FloorItem, 65));
    }
}