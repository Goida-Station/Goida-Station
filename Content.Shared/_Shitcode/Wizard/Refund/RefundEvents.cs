// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Wizard.Refund;

[Serializable, NetSerializable]
public sealed class StoreRefundState(List<RefundListingData> listings, bool refundDisabled) : BoundUserInterfaceState
{
    public List<RefundListingData> Listings = listings;

    public bool RefundDisabled = refundDisabled;
}

[Serializable, NetSerializable]
public struct RefundListingData(NetEntity entity, string displayName)
{
    public NetEntity Entity = entity;

    public string DisplayName = displayName;
}

[Serializable, NetSerializable]
public sealed class StoreRefundListingMessage(NetEntity listingEntity) : BoundUserInterfaceMessage
{
    public NetEntity ListingEntity = listingEntity;
}

[Serializable, NetSerializable]
public sealed class StoreRefundAllListingsMessage : BoundUserInterfaceMessage;

[Serializable, NetSerializable]
public enum RefundUiKey : byte
{
    Key
}