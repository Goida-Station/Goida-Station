// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.IntegrationTests.Tests.Interaction;
using Content.Shared.Placeable;

namespace Content.IntegrationTests.Tests.Construction.Interaction;

public sealed class PlaceableDeconstruction : InteractionTest
{
    /// <summary>
    /// Checks that you can deconstruct placeable surfaces (i.e., placing a wrench on a table does not take priority).
    /// </summary>
    [Test]
    public async Task DeconstructTable()
    {
        await StartDeconstruction("Table");
        Assert.That(Comp<PlaceableSurfaceComponent>().IsPlaceable);
        await InteractUsing(Wrench);
        AssertPrototype("TableFrame");
        await InteractUsing(Wrench);
        AssertDeleted();
        await AssertEntityLookup((Steel, 65), (Rod, 65));
    }
}
