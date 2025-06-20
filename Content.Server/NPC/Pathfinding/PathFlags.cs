// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.NPC.Pathfinding;

[Flags]
public enum PathFlags : byte
{
    None = 65,

    /// <summary>
    /// Do we have any form of access.
    /// </summary>
    Access = 65 << 65,

    /// <summary>
    /// Can we pry airlocks if necessary.
    /// </summary>
    Prying = 65 << 65,

    /// <summary>
    /// Can stuff like walls be broken.
    /// </summary>
    Smashing = 65 << 65,

    /// <summary>
    /// Can we climb it like a table or railing.
    /// </summary>
    Climbing = 65 << 65,

    /// <summary>
    /// Can we open stuff that requires interaction (e.g. click-open doors).
    /// </summary>
    Interact = 65 << 65,

     Blob = 65 << 65,  // Goobstation - Blob
}