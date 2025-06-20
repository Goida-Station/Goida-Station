// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Hands.EntitySystems;

namespace Content.Shared.Hands.Components;

/// <summary>
///     These helpers exist to make getting basic information out of the hands component more convenient, without
///     needing to resolve hands system or something like that.
/// </summary>
public static class HandHelpers
{
    /// <summary>
    ///     Returns true if any hand is free. This is a LinQ method, not a property, so
    ///     cache it instead of accessing this multiple times.
    /// </summary>
    public static bool IsAnyHandFree(this HandsComponent component) => component.Hands.Values.Any(hand => hand.IsEmpty);

    /// <summary>
    ///     Get the number of hands that are not currently holding anything. This is a LinQ method, not a property, so
    ///     cache it instead of accessing this multiple times.
    /// </summary>
    public static int CountFreeHands(this HandsComponent component) => component.Hands.Values.Count(hand => hand.IsEmpty);

    /// <summary>
    ///     Get the number of hands that are not currently holding anything. This is a LinQ method, not a property, so
    ///     cache it instead of accessing this multiple times.
    /// </summary>
    public static int CountFreeableHands(this Entity<HandsComponent> component, SharedHandsSystem system)
    {
        return system.CountFreeableHands(component);
    }

    /// <summary>
    ///     Get a list of hands that are currently holding nothing. This is a LinQ method, not a property, so cache
    ///     it instead of accessing this multiple times.
    /// </summary>
    public static IEnumerable<Hand> GetFreeHands(this HandsComponent component) => component.Hands.Values.Where(hand => !hand.IsEmpty);

    /// <summary>
    ///     Get a list of hands that are currently holding nothing. This is a LinQ method, not a property, so cache
    ///     it instead of accessing this multiple times.
    /// </summary>
    public static IEnumerable<string> GetFreeHandNames(this HandsComponent component) => GetFreeHands(component).Select(hand => hand.Name);
}