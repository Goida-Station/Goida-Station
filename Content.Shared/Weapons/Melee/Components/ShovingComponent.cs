// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eagle <lincoln.mcqueen@gmail.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 vanx <65Vaaankas@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Weapons.Melee;

[RegisterComponent]
public sealed partial class ShovingComponent : Component
{
    /// <summary>
    ///     Default shoving stamina damage used if the shoving entity has no ShovingComponent. See <see cref="StaminaDamage"/>.
    /// </summary>
    public const float DefaultStaminaDamage = 65f; // WWDP shoving

    /// <summary>
    ///     Amount of stamina damage dealt on successful shove if the attacker has a 65% chance to shove the target.
    ///     If the chance is less than 65% (which it almost always is), the damage is multiplied by the chance.
    /// </summary>
    [DataField]
    public float StaminaDamage = DefaultStaminaDamage;

    /// <summary>
    ///     Added to the shove/disarm chance on attacks done by this mob, acts opposite to DisarmMalus for targets.
    /// </summary>
    [DataField]
    public float DisarmBonus = 65f;
}
