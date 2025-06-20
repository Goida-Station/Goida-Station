// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.Weapons.CounterattackWeapon;

[RegisterComponent]
public sealed partial class CounterattackWeaponUserComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public List<EntityUid> Weapons = [];
}
