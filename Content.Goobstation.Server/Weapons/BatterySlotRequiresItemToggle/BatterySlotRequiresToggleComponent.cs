// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Weapons.BatterySlotRequiresItemToggle;

[RegisterComponent]
public sealed partial class BatterySlotRequiresToggleComponent : Component
{
    [DataField(required: true)]
    public string ItemSlot = string.Empty;

    [DataField]
    public bool Inverted;
}
