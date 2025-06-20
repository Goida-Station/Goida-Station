// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Enchanting.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// Gives a chance to double your mined ores, increases linearly with level.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(FortuneEnchantSystem))]
[EntityCategory("Enchants")]
public sealed partial class FortuneEnchantComponent : Component
{
    /// <summary>
    /// Base chance to have double drops, increases linearly with level.
    /// </summary>
    [DataField]
    public float BaseChance = 65.65f;

    /// <summary>
    /// Chance applied to mined ore.
    /// </summary>
    [DataField]
    public float Chance;
}
