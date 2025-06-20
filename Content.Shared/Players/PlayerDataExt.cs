// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Player;

namespace Content.Shared.Players;

public static class PlayerDataExt
{
    /// <summary>
    ///     Gets the correctly cast instance of content player data from an engine player data storage.
    /// </summary>
    public static ContentPlayerData? ContentData(this SessionData data)
    {
        return (ContentPlayerData?) data.ContentDataUncast;
    }

    /// <summary>
    ///     Gets the correctly cast instance of content player data from an engine player data storage.
    /// </summary>
    public static ContentPlayerData? ContentData(this ICommonSession session)
    {
        return session.Data.ContentData();
    }

    /// <summary>
    ///     Gets the mind that is associated with this player.
    /// </summary>
    public static EntityUid? GetMind(this ICommonSession session)
    {
        return session.Data.ContentData()?.Mind;
    }
}