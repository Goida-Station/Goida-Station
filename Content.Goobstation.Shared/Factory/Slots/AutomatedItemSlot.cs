// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Containers.ItemSlots;

namespace Content.Goobstation.Shared.Factory.Slots;

/// <summary>
/// Abstraction over an <see cref="ItemSlot"/> on the machine.
/// </summary>
public sealed partial class AutomatedItemSlot : AutomationSlot
{
    /// <summary>
    /// The name of the slot to automate.
    /// </summary>
    [DataField(required: true)]
    public string SlotId = string.Empty;

    private ItemSlotsSystem _slots;

    private ItemSlot? _slot;

    [ViewVariables]
    public ItemSlot Slot
    {
        get
        {
            if (_slot is {} slot)
                return slot;

            if (_slots.TryGetSlot(Owner, SlotId, out _slot))
                return _slot;

            throw new InvalidOperationException($"Entity {EntMan.ToPrettyString(Owner)} had no item slot {SlotId}");
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        _slots = EntMan.System<ItemSlotsSystem>();
    }

    public override bool Insert(EntityUid item)
    {
        return base.Insert(item) &&
            _slots.TryInsert(Owner, Slot, item, user: null);
    }

    public override bool CanInsert(EntityUid item)
    {
        return base.CanInsert(item) &&
            _slots.CanInsert(Owner, usedUid: item, user: null, Slot);
    }

    public override EntityUid? GetItem(EntityUid? filter)
    {
        if (Slot.Item is not {} item || _filter.IsBlocked(filter, item))
            return null;

        return item;
    }
}
