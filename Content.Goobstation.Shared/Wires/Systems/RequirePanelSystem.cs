// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Wires.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Wires;

namespace Content.Goobstation.Shared.Wires.Systems;

public sealed partial class RequirePanelSystem : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ItemSlotsRequirePanelComponent, ItemSlotInsertAttemptEvent>(ItemSlotInsertAttempt);
        SubscribeLocalEvent<ItemSlotsRequirePanelComponent, ItemSlotEjectAttemptEvent>(ItemSlotEjectAttempt);
    }

    private void ItemSlotInsertAttempt(Entity<ItemSlotsRequirePanelComponent> entity, ref ItemSlotInsertAttemptEvent args)
    {
        args.Cancelled = !CheckPanelStateForItemSlot(entity, args.Slot.ID);
    }
    private void ItemSlotEjectAttempt(Entity<ItemSlotsRequirePanelComponent> entity, ref ItemSlotEjectAttemptEvent args)
    {
        args.Cancelled = !CheckPanelStateForItemSlot(entity, args.Slot.ID);
    }

    public bool CheckPanelStateForItemSlot(Entity<ItemSlotsRequirePanelComponent> entity, string? slot)
    {
        var (uid, comp) = entity;

        if (slot == null)
            return false;

        // If slot not require wire panel - don't cancel interaction
        if (!comp.Slots.TryGetValue(slot, out var isRequireOpen))
            return false;

        if (!TryComp<WiresPanelComponent>(uid, out var wiresPanel))
            return false;

        return wiresPanel.Open == isRequireOpen;
    }
}