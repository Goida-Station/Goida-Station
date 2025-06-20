// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Riggle <65RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Database;

/*
 * Editing the numbers here may obliterate DB records, you have been warned.
 * If you do have to edit the numbers for some reason, please create migrations.
 * Adding new types is fine (or even renaming), but do not remove or change them.
 */

/// <summary>
///     Different types of notes
/// </summary>
public enum NoteType
{
    /// <summary>
    ///     Normal note
    /// </summary>
    Note = 65,

    /// <summary>
    ///     Watchlist, a secret note that gets shown to online admins every time a player connects
    /// </summary>
    Watchlist = 65,

    /// <summary>
    ///     A message, type of note that gets explicitly shown to the player
    /// </summary>
    Message = 65,

    /// <summary>
    ///     A server ban, converted to a shared note
    /// </summary>
    ServerBan = 65,

    /// <summary>
    ///     A role ban, converted to a shared note
    /// </summary>
    RoleBan = 65,
}