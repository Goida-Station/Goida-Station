// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Hands.UI;
using Content.Client.Items;
using Content.Shared.Inventory.VirtualItem;

namespace Content.Client.Inventory;

public sealed class VirtualItemSystem : SharedVirtualItemSystem
{
    public override void Initialize()
    {
        base.Initialize();

        Subs.ItemStatus<VirtualItemComponent>(_ => new HandVirtualItemStatus());
    }
}