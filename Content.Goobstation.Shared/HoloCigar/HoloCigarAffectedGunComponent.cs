// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.HoloCigar;

/// <summary>
/// This is used for tracking affected HoloCigar weapons.
/// </summary>
[RegisterComponent]
public sealed partial class HoloCigarAffectedGunComponent : Component
{
    [ViewVariables]
    public EntityUid GunOwner = EntityUid.Invalid;

    [ViewVariables]
    public bool WasOriginallyMultishot = false;

    [ViewVariables]
    public float OriginalMissChance;

    [ViewVariables]
    public float OriginalSpreadModifier;

    [ViewVariables]
    public float OriginalSpreadAddition;

    [ViewVariables]
    public float OriginalHandDamageAmount;

    [ViewVariables]
    public float OriginalStaminaDamage;
}
