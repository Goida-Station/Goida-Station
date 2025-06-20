// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using JetBrains.Annotations;

namespace Content.Shared.Store;

/// <summary>
/// Used to define a complicated condition that requires C#
/// </summary>
[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
public abstract partial class ListingCondition
{
    /// <summary>
    /// Determines whether or not a certain entity can purchase a listing.
    /// </summary>
    /// <returns>Whether or not the listing can be purchased</returns>
    public abstract bool Condition(ListingConditionArgs args);
}

/// <param name="Buyer">Either the account owner, user, or an inanimate object (e.g., surplus bundle)</param>
/// <param name="Listing">The listing itself</param>
/// <param name="EntityManager">An entitymanager for sane coding</param>
public readonly record struct ListingConditionArgs(EntityUid Buyer, EntityUid? StoreEntity, ListingData Listing, IEntityManager EntityManager);