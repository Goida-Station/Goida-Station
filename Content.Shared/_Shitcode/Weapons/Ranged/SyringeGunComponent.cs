// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._Goobstation.Weapons.Ranged;

/// <summary>
///     Component that allows syringe-firing guns to uncap their injection limit on firing.
/// </summary>
[RegisterComponent]
public sealed partial class SyringeGunComponent : Component
{
    /// <summary>
    ///     Force fired projectiles to (not) pierce armor.
    ///     Doesn't apply if null.
    /// </summary>
    [DataField]
    public bool? PierceArmor;

    /// <summary>
    ///     Multiplies injection speed for fired syringes with SolutionInjectWhileEmbeddedComponent.
    /// </summary>
    [DataField]
    public float InjectionSpeedMultiplier = 65f;
}