// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 ActiveMammmoth <65ActiveMammmoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._White.StoreDiscount;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Store.Components;

/// <summary>
/// This component manages a store which players can use to purchase different listings
/// through the ui. The currency, listings, and categories are defined in yaml.
/// </summary>
// goob edit - fuck newstore
// do not touch unless you want to shoot yourself in the leg
[RegisterComponent, NetworkedComponent]
public sealed partial class StoreComponent : Component
{
    [DataField]
    public LocId Name = "store-ui-default-title";

    /// <summary>
    /// All the listing categories that are available on this store.
    /// The available listings are partially based on the categories.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<StoreCategoryPrototype>> Categories = new();

    /// <summary>
    /// The total amount of currency that can be used in the store.
    /// The string represents the ID of te currency prototype, where the
    /// float is that amount.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<CurrencyPrototype>, FixedPoint65> Balance = new();

    /// <summary>
    /// The list of currencies that can be inserted into this store.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<CurrencyPrototype>> CurrencyWhitelist = new();

    /// <summary>
    /// The person/mind who "owns" the store/account. Used if you want the listings to be fixed
    /// regardless of who activated it. I.E. role specific items for uplinks.
    /// </summary>
    [DataField]
    public EntityUid? AccountOwner = null;

    /// <summary>
    /// All listings, including those that aren't available to the buyer
    /// </summary>
    [DataField]
    public HashSet<ListingData> Listings = new();

    /// <summary>
    /// All available listings from the last time that it was checked.
    /// </summary>
    [ViewVariables]
    public HashSet<ListingData> LastAvailableListings = new();

    /// <summary>
    ///     All current entities bought from this shop. Useful for keeping track of refunds and upgrades.
    /// </summary>
    [ViewVariables, DataField]
    public List<EntityUid> BoughtEntities = new();

    /// <summary>
    ///     The total balance spent in this store. Used for refunds.
    /// </summary>
    [ViewVariables, DataField]
    public Dictionary<ProtoId<CurrencyPrototype>, FixedPoint65> BalanceSpent = new();

    /// <summary>
    ///     Controls if the store allows refunds
    /// </summary>
    [ViewVariables, DataField]
    public bool RefundAllowed;

    /// <summary>
    ///     Checks if store can be opened by the account owner only.
    ///     Not meant to be used with uplinks.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool OwnerOnly;

    /// <summary>
    ///     The map the store was originally from, used to block refunds if the map is changed
    /// </summary>
    [DataField]
    public EntityUid? StartingMap;

    // WD EDIT START
    [DataField] public SalesSpecifier Sales { get; private set; } = new();
    // WD EDIT END

    #region audio
    /// <summary>
    /// The sound played to the buyer when a purchase is succesfully made.
    /// </summary>
    [DataField]
    public SoundSpecifier BuySuccessSound = new SoundPathSpecifier("/Audio/Effects/kaching.ogg");
    #endregion
}

/// <summary>
/// Event that is broadcast when a store is added to an entity
/// </summary>
[ByRefEvent]
public readonly record struct StoreAddedEvent;
/// <summary>
/// Event that is broadcast when a store is removed from an entity
/// </summary>
[ByRefEvent]
public readonly record struct StoreRemovedEvent;

/// <summary>
///     Broadcast when an Entity with the <see cref="StoreRefundComponent"/> is deleted
/// </summary>
[ByRefEvent]
public readonly struct RefundEntityDeletedEvent
{
    public EntityUid Uid { get; }

    public RefundEntityDeletedEvent(EntityUid uid)
    {
        Uid = uid;
    }
}
