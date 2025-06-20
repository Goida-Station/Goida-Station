// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Tabletop.Events
{
    /// <summary>
    /// An event ot tell the server that we have stopped playing this tabletop game.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class TabletopStopPlayingEvent : EntityEventArgs
    {
        /// <summary>
        /// The entity UID of the table associated with this tabletop game.
        /// </summary>
        public NetEntity TableUid;

        public TabletopStopPlayingEvent(NetEntity tableUid)
        {
            TableUid = tableUid;
        }
    }
}