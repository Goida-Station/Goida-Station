// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Store.Systems;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Store;
using Robust.Shared.Prototypes;

namespace Content.Server.Store.Components;

/// <summary>
///     Keeps track of entities bought from stores for refunds, especially useful if entities get deleted before they can be refunded.
/// </summary>
[RegisterComponent, Access(typeof(StoreSystem))]
public sealed partial class StoreRefundComponent : Component
{
    [ViewVariables, DataField]
    public EntityUid? StoreEntity;

    // Goobstation start
    [ViewVariables, DataField]
    public ListingData? Data;

    [ViewVariables, DataField]
    public Dictionary<ProtoId<CurrencyPrototype>, FixedPoint65> BalanceSpent = new();
    // Goobstation end
}
