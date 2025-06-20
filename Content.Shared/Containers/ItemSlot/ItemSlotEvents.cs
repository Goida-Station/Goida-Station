// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <vincefvanwijk@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Containers.ItemSlots;

/// <summary>
///     Used for various "eject this item" buttons.
/// </summary>
[Serializable, NetSerializable]
public sealed class ItemSlotButtonPressedEvent : BoundUserInterfaceMessage
{
    /// <summary>
    ///     The name of the slot/container from which to insert or eject an item.
    /// </summary>
    public string SlotId;

    /// <summary>
    ///     Whether to attempt to insert an item into the slot, if there is not already one inside.
    /// </summary>
    public bool TryInsert;

    /// <summary>
    ///     Whether to attempt to eject the item from the slot, if it has one.
    /// </summary>
    public bool TryEject;

    public ItemSlotButtonPressedEvent(string slotId, bool tryEject = true, bool tryInsert = true)
    {
        SlotId = slotId;
        TryEject = tryEject;
        TryInsert = tryInsert;
    }
}