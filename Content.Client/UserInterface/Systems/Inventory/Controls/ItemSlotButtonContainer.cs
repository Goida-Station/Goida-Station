// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Systems.Inventory.Controls;

public sealed class ItemSlotButtonContainer : ItemSlotUIContainer<SlotControl>
{
    private readonly InventoryUIController _inventoryController;
    private string _slotGroup = "";

    public string SlotGroup
    {
        get => _slotGroup;
        set
        {
            _inventoryController.RemoveSlotGroup(SlotGroup);
            _slotGroup = value;
            _inventoryController.RegisterSlotGroupContainer(this);
        }
    }

    public ItemSlotButtonContainer()
    {
        _inventoryController = UserInterfaceManager.GetUIController<InventoryUIController>();
    }
}