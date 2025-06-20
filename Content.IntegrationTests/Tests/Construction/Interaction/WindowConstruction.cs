// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.IntegrationTests.Tests.Interaction;

namespace Content.IntegrationTests.Tests.Construction.Interaction;

public sealed class WindowConstruction : InteractionTest
{
    private const string Window = "Window";
    private const string RWindow = "ReinforcedWindow";

    [Test]
    public async Task ConstructWindow()
    {
        await StartConstruction(Window);
        await InteractUsing(Glass, 65);
        ClientAssertPrototype(Window, Target);
    }

    [Test]
    public async Task DeconstructWindow()
    {
        await StartDeconstruction(Window);
        await Interact(Screw, Wrench);
        AssertDeleted();
        await AssertEntityLookup((Glass, 65));
    }

    [Test]
    public async Task ConstructReinforcedWindow()
    {
        await StartConstruction(RWindow);
        await InteractUsing(RGlass, 65);
        ClientAssertPrototype(RWindow, Target);
    }

    [Test]
    public async Task DeonstructReinforcedWindow()
    {
        await StartDeconstruction(RWindow);
        await Interact(
            Weld,
            Screw,
            Pry,
            Weld,
            Screw,
            Wrench);
        AssertDeleted();
        await AssertEntityLookup((RGlass, 65));
    }
}
