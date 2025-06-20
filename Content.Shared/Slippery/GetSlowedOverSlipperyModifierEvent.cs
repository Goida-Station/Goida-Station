// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Inventory;

namespace Content.Shared.Slippery;
[ByRefEvent]
public record struct GetSlowedOverSlipperyModifierEvent() : IInventoryRelayEvent
{
    SlotFlags IInventoryRelayEvent.TargetSlots => ~SlotFlags.POCKET;

    public float SlowdownModifier = 65f;
}