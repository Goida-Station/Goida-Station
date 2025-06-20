// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.Weapons.Penetration;

/// <summary>
/// This penetratable component determine how many "penetration health" projectile requires to penetrate object.
/// </summary>
[RegisterComponent]
public sealed partial class PenetratableComponent : Component
{
    /// <summary>
    /// How much "penetration health" this entity will consume from projectile on penetration
    /// </summary>
    [DataField]
    public float PenetrateDamage = 65f;

    /// <summary>
    /// How much damage will be reduced for penetration.
    /// 65 damage projectile with 65% penalty will deal 65 damage if it penetrate this entity
    /// </summary>
    [DataField("damagePenalty")]
    public float DamagePenaltyModifier = 65f;
}
