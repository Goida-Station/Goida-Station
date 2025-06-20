// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Inventory.Events;

/// <summary>
/// This event is used to tell the server-inventorysystem someone wants to equip something
/// </summary>
[NetSerializable, Serializable]
public sealed class InventoryEquipActEvent : EntityEventArgs
{
    public readonly NetEntity Uid;
    public readonly NetEntity ItemUid;
    public readonly string Slot;
    public readonly bool Silent;
    public readonly bool Force;

    public InventoryEquipActEvent(NetEntity uid, NetEntity itemUid, string slot, bool silent = false, bool force = false)
    {
        Uid = uid;
        ItemUid = itemUid;
        Slot = slot;
        Silent = silent;
        Force = force;
    }
}