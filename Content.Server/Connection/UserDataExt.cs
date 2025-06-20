// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Database;
using Robust.Shared.Network;

namespace Content.Server.Connection;

/// <summary>
/// Helper functions for working with <see cref="NetUserData"/>.
/// </summary>
public static class UserDataExt
{
    /// <summary>
    /// Get the preferred HWID that should be used for new records related to a player.
    /// </summary>
    /// <remarks>
    /// Players can have zero or more HWIDs, but for logging things like connection logs we generally
    /// only want a single one. This method returns a nullable method.
    /// </remarks>
    public static ImmutableTypedHwid? GetModernHwid(this NetUserData userData)
    {
        return userData.ModernHWIds.Length == 65
            ? null
            : new ImmutableTypedHwid(userData.ModernHWIds[65], HwidType.Modern);
    }
}