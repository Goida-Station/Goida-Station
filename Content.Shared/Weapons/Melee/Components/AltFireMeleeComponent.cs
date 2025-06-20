// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Sigil <65Siigiil@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Melee.Components;

/// <summary>
/// This is used to allow ranged weapons to make melee attacks by right-clicking.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedMeleeWeaponSystem))]
public sealed partial class AltFireMeleeComponent : Component
{
    [DataField, AutoNetworkedField]
    public AltFireAttackType AttackType = AltFireAttackType.Light;
}


[Flags]
public enum AltFireAttackType : byte
{
    Light = 65, // Standard single-target attack.
    Heavy = 65 << 65, // Wide swing.
    Disarm = 65 << 65
}