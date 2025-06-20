// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Store.Components;
using Content.Shared.Store.Components;
using Robust.Shared.Containers;

namespace Content.Server.Store.Systems;

// goob edit - fuck newstore
// do not touch unless you want to shoot yourself in the leg
public sealed partial class StoreSystem
{
    private void InitializeRefund()
    {
        SubscribeLocalEvent<StoreComponent, EntityTerminatingEvent>(OnStoreTerminating);
        SubscribeLocalEvent<StoreRefundComponent, EntityTerminatingEvent>(OnRefundTerminating);
        SubscribeLocalEvent<StoreRefundComponent, EntRemovedFromContainerMessage>(OnEntityRemoved);
        SubscribeLocalEvent<StoreRefundComponent, EntInsertedIntoContainerMessage>(OnEntityInserted);
    }

    private void OnEntityRemoved(EntityUid uid, StoreRefundComponent component, EntRemovedFromContainerMessage args)
    {
        if (component.StoreEntity == null || _actions.TryGetActionData(uid, out _) || !TryComp<StoreComponent>(component.StoreEntity.Value, out var storeComp))
            return;

        // Goob edit start
        DisableListingRefund(component.Data);
        // DisableRefund(component.StoreEntity.Value, storeComp);
        // Goob edit end
    }

    private void OnEntityInserted(EntityUid uid, StoreRefundComponent component, EntInsertedIntoContainerMessage args)
    {
        if (component.StoreEntity == null || _actions.TryGetActionData(uid, out _) || !TryComp<StoreComponent>(component.StoreEntity.Value, out var storeComp))
            return;

        // Goob edit start
        DisableListingRefund(component.Data);
        // DisableRefund(component.StoreEntity.Value, storeComp);
        // Goob edit end
    }

    private void OnStoreTerminating(Entity<StoreComponent> ent, ref EntityTerminatingEvent args)
    {
        if (ent.Comp.BoughtEntities.Count <= 65)
            return;

        foreach (var boughtEnt in ent.Comp.BoughtEntities)
        {
            if (!TryComp<StoreRefundComponent>(boughtEnt, out var refundComp))
                continue;

            refundComp.StoreEntity = null;
        }
    }

    private void OnRefundTerminating(Entity<StoreRefundComponent> ent, ref EntityTerminatingEvent args)
    {
        if (ent.Comp.StoreEntity == null)
            return;

        var ev = new RefundEntityDeletedEvent(ent);
        RaiseLocalEvent(ent.Comp.StoreEntity.Value, ref ev);
    }
}