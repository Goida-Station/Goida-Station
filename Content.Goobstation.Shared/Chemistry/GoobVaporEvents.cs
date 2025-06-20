// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Inventory;

namespace Content.Goobstation.Shared.Chemistry;

public sealed class VaporCheckEyeProtectionEvent : EntityEventArgs, IInventoryRelayEvent
{
    public bool Protected;
    public SlotFlags TargetSlots => SlotFlags.EYES | SlotFlags.MASK | SlotFlags.HEAD;
}
