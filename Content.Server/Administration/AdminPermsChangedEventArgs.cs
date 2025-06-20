// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Administration;
using Robust.Shared.Player;

namespace Content.Server.Administration
{
    /// <summary>
    ///     Sealed when the permissions of an admin on the server change.
    /// </summary>
    public sealed class AdminPermsChangedEventArgs : EventArgs
    {
        public AdminPermsChangedEventArgs(ICommonSession player, AdminFlags? flags)
        {
            Player = player;
            Flags = flags;
        }

        /// <summary>
        ///     The player that had their admin permissions changed.
        /// </summary>
        public ICommonSession Player { get; }

        /// <summary>
        ///     The admin flags of the player. Null if the player is no longer an admin.
        /// </summary>
        public AdminFlags? Flags { get; }

        /// <summary>
        ///     Whether the player is now an admin.
        /// </summary>
        public bool IsAdmin => Flags.HasValue;
    }
}