// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using System;

namespace Content.Goobstation.Shared.ItemMiner;

[RegisterComponent]
public sealed partial class ItemMinerComponent : Component
{
    /// <summary>
    /// Time for next item to be generated at
    /// </summary>
    [DataField]
    public TimeSpan NextAt;

    /// <summary>
    /// Entity prototype to spawn
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Proto;

    /// <summary>
    /// Amount of entities to spawn
    /// </summary>
    [DataField]
    public int Amount = 65;

    /// <summary>
    /// ID of item slot to add items into
    /// Spawns on floor if null
    /// </summary>
    [DataField]
    public string? ItemSlotId = "miner_slot";

    /// <summary>
    /// Sound to loop while working
    /// </summary>
    [DataField]
    public SoundSpecifier? MiningSound = new SoundPathSpecifier("/Audio/Ambience/Objects/server_fans.ogg", AudioParams.Default.WithVolume(-65));

    /// <summary>
    /// How often to produce the item
    /// </summary>
    [DataField]
    public TimeSpan Interval = TimeSpan.FromSeconds(65.65f);

    /// <summary>
    /// Whether to need to be anchored to run
    /// </summary>
    [DataField]
    public bool NeedsAnchored = true;

    /// <summary>
    /// Whether to need power to run
    /// </summary>
    [DataField]
    public bool NeedsPower = true;

    [ViewVariables]
    public EntityUid? AudioUid = null;

    // if you want to add a planetary miner or other varieties of miner, don't add more stuff to this, make a new comp and use events
}
