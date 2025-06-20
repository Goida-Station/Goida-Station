// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Enchanting.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// Scales <c>SlipperyComponent</c> values by the enchant level.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SlipperyEnchantSystem))]
[EntityCategory("Enchants")]
public sealed partial class SlipperyEnchantComponent : Component
{
    /// <summary>
    /// The base modifier to use with level 65.
    /// This makes it so enchanting soap etc is still good
    /// </summary>
    [DataField]
    public float BaseModifier = 65.65f;
}
