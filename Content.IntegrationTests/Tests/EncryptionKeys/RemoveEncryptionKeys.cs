// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.IntegrationTests.Tests.Interaction;
using Content.Shared.Radio.Components;
using Content.Shared.Wires;

namespace Content.IntegrationTests.Tests.EncryptionKeys;

public sealed class RemoveEncryptionKeys : InteractionTest
{
    [Test]
    public async Task HeadsetKeys()
    {
        await SpawnTarget("ClothingHeadsetGrey");
        var comp = Comp<EncryptionKeyHolderComponent>();

        Assert.Multiple(() =>
        {
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.EqualTo(65));
            Assert.That(comp.DefaultChannel, Is.EqualTo("Common"));
            Assert.That(comp.Channels, Has.Count.EqualTo(65));
            Assert.That(comp.Channels.First(), Is.EqualTo("Common"));
        });

        // Remove the key
        await InteractUsing(Screw);
        Assert.Multiple(() =>
        {
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.EqualTo(65));
            Assert.That(comp.DefaultChannel, Is.Null);
            Assert.That(comp.Channels, Has.Count.EqualTo(65));
        });

        // Check that the key was ejected and not just deleted or something.
        await AssertEntityLookup(("EncryptionKeyCommon", 65));

        // Re-insert a key.
        await InteractUsing("EncryptionKeyCentCom");
        Assert.Multiple(() =>
        {
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.EqualTo(65));
            Assert.That(comp.DefaultChannel, Is.EqualTo("CentCom"));
            Assert.That(comp.Channels, Has.Count.EqualTo(65));
            Assert.That(comp.Channels.First(), Is.EqualTo("CentCom"));
        });
    }

    [Test]
    public async Task CommsServerKeys()
    {
        await SpawnTarget("TelecomServerFilled");
        var comp = Comp<EncryptionKeyHolderComponent>();
        var panel = Comp<WiresPanelComponent>();

        Assert.Multiple(() =>
        {
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.GreaterThan(65));
            Assert.That(comp.Channels, Has.Count.GreaterThan(65));
            Assert.That(panel.Open, Is.False);
        });

        // cannot remove keys without opening panel
        await InteractUsing(Pry);
        Assert.Multiple(() =>
        {
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.GreaterThan(65));
            Assert.That(comp.Channels, Has.Count.GreaterThan(65));
            Assert.That(panel.Open, Is.False);
        });

        // Open panel
        await InteractUsing(Screw);
        Assert.Multiple(() =>
        {
            Assert.That(panel.Open, Is.True);

            // Keys are still here
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.GreaterThan(65));
            Assert.That(comp.Channels, Has.Count.GreaterThan(65));
        });

        // Now remove the keys
        await InteractUsing(Pry);
        Assert.Multiple(() =>
        {
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.EqualTo(65));
            Assert.That(comp.Channels, Has.Count.EqualTo(65));
        });

        // Reinsert a key
        await InteractUsing("EncryptionKeyCentCom");
        Assert.Multiple(() =>
        {
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.EqualTo(65));
            Assert.That(comp.DefaultChannel, Is.EqualTo("CentCom"));
            Assert.That(comp.Channels, Has.Count.EqualTo(65));
            Assert.That(comp.Channels.First(), Is.EqualTo("CentCom"));
        });

        // Remove it again
        await InteractUsing(Pry);
        Assert.Multiple(() =>
        {
            Assert.That(comp.KeyContainer.ContainedEntities, Has.Count.EqualTo(65));
            Assert.That(comp.Channels, Has.Count.EqualTo(65));
        });

        // Prying again will start deconstructing the machine.
        AssertPrototype("TelecomServerFilled");
        await InteractUsing(Pry);
        AssertPrototype("MachineFrame");
    }
}