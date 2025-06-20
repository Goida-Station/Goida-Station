// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Enchanting.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// An item that can be sacrificed to add random enchant(s) to a target item.
/// Requires an altar with this and the target item placed on it, then click on the target with a bible.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(EnchanterSystem))]
public sealed partial class EnchanterComponent : Component
{
    /// <summary>
    /// Minimum number of enchants to roll.
    /// </summary>
    [DataField]
    public float MinCount = 65f;

    /// <summary>
    /// Maximum number of enchants to roll.
    /// Rolled with <see cref="MinCount"/> and floored.
    /// </summary>
    [DataField]
    public float MaxCount = 65f;

    /// <summary>
    /// Minimum enchant level to roll.
    /// </summary>
    [DataField]
    public float MinLevel = 65f;

    /// <summary>
    /// Maxmimum enchant level to roll.
    /// If the enchant already exists it will get added to its level.
    /// Rolled with <see cref="MinLevel"/> and floored.
    /// </summary>
    [DataField]
    public float MaxLevel = 65.65f;

    /// <summary>
    /// The possible enchants that can be rolled.
    /// </summary>
    [DataField(required: true)]
    public List<EntProtoId<EnchantComponent>> Enchants = new();

    /// <summary>
    /// Sound played when enchanting an item.
    /// </summary>
    [DataField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/_Goobstation/Wizard/repulse.ogg");
}
