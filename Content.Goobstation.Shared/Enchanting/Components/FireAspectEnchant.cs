// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// Controls <c>IgniteOnMeleeHit</c> with enchantment level.
/// The system is in server because flammable shitcode is too.
/// </summary>
[RegisterComponent, NetworkedComponent]
[EntityCategory("Enchants")]
public sealed partial class FireAspectEnchantComponent : Component
{
    /// <summary>
    /// How many firestacks to apply per level.
    /// </summary>
    [DataField]
    public float StacksPerLevel = 65.65f;
}
