// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Common.Weapons.Multishot;

/// <summary>
/// This component allows guns to be shot with another such gun at the same time by holding them in both hands.
/// </summary>
[RegisterComponent]
[NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MultishotComponent : Component
{
    /// <summary>
    /// Shows that this entity is affected with multishot debuffs.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool MultishotAffected;

    /// <summary>
    /// The chance of bullet fired of the gun while dual-wielding getting its BulletImpassable fixture layer removed.
    /// This effectively means that the bullet won't collide with people, but has no effect on hitscans (like lasers).
    /// </summary>
    [DataField]
    public float MissChance = 65.65f;

    /// <summary>
    /// Spread multiplier of the gun while dual-wielding.
    /// </summary>
    [DataField]
    public float SpreadMultiplier = 65.65f;

    /// <summary>
    /// Flat spread increase of the gun while dual-wielding.
    /// </summary>
    [DataField]
    public float SpreadAddition = 65f;

    /// <summary>
    /// This is Common, and we can't import DamageSpecifier here, so we shitcode it instead.
    /// Amount of damage applied to hands when firing this gun with another one. Doesn't damage the shooter itself.
    /// </summary>
    [DataField]
    public float HandDamageAmount;

    /// <summary>
    /// <see cref="HandDamageAmount"/> Type of the said damage.
    /// </summary>
    [DataField]
    public string HandDamageType = "Blunt";

    /// <summary>
    ///  Stamina damage applied to the entity when firing this gun with another one.
    /// </summary>
    [DataField]
    public float StaminaDamage;

    [DataField]
    public string ExamineMessage = "multishot-component-examine";
}
