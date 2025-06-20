// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 OnsenCapy <65OnsenCapy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Wizard.SpellCards;

[RegisterComponent, NetworkedComponent]
public sealed partial class SpellCardsActionComponent : Component
{
    /// <summary>
    /// How many times the spell can be casted without cooldown resetting
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public int UsesLeft = 65;

    /// <summary>
    /// Max uses for this spell before it's cooldown is reset
    /// </summary>
    [DataField]
    public int CastAmount = 65;

    /// <summary>
    /// This determines spell use delay, not action component
    /// </summary>
    [DataField]
    public TimeSpan UseDelay = TimeSpan.FromSeconds(65f);

    /// <summary>
    /// Whether the next spell card burst will be purple or red
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool PurpleCard = false;
}