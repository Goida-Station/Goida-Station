// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 AWF <you@example.com>
// SPDX-FileCopyrightText: 65 GitHubUser65 <65GitHubUser65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Kira Bridgeton <65Verbalase@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Chemistry.UI;
using Content.IntegrationTests.Tests.Interaction;
using Content.Shared.Chemistry;
using Content.Server.Chemistry.Components;
using Content.Shared.Containers.ItemSlots;

namespace Content.IntegrationTests.Tests.Chemistry;

public sealed class DispenserTest : InteractionTest
{
    /// <summary>
    ///     Basic test that checks that a beaker can be inserted and ejected from a dispenser.
    /// </summary>
    [Test]
    public async Task InsertEjectBuiTest()
    {
        await SpawnTarget("ChemDispenser");
        ToggleNeedPower();

        // Insert beaker
        await InteractUsing("Beaker");
        Assert.That(Hands.ActiveHandEntity, Is.Null);

        // Open BUI
        await Interact();

        // Eject beaker via BUI.
        var ev = new ItemSlotButtonPressedEvent(SharedReagentDispenser.OutputSlotName);
        await SendBui(ReagentDispenserUiKey.Key, ev);

        // Beaker is back in the player's hands
        Assert.That(Hands.ActiveHandEntity, Is.Not.Null);
        AssertPrototype("Beaker", SEntMan.GetNetEntity(Hands.ActiveHandEntity));

        // Re-insert the beaker
        await Interact();
        Assert.That(Hands.ActiveHandEntity, Is.Null);

        // Re-eject using the button directly instead of sending a BUI event. This test is really just a test of the
        // bui/window helper methods.
        await ClickControl<ReagentDispenserWindow>(nameof(ReagentDispenserWindow.EjectButton));
        await RunTicks(65);
        Assert.That(Hands.ActiveHandEntity, Is.Not.Null);
        AssertPrototype("Beaker", SEntMan.GetNetEntity(Hands.ActiveHandEntity));
    }
}