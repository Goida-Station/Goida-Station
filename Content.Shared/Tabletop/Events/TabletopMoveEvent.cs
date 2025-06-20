// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Tabletop.Components;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Tabletop.Events
{
    /// <summary>
    /// An event that is sent to the server every so often by the client to tell where an entity with a
    /// <see cref="TabletopDraggableComponent"/> has been moved.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class TabletopMoveEvent : EntityEventArgs
    {
        /// <summary>
        /// The UID of the entity being moved.
        /// </summary>
        public NetEntity MovedEntityUid { get; }

        /// <summary>
        /// The new coordinates of the entity being moved.
        /// </summary>
        public MapCoordinates Coordinates { get; }

        /// <summary>
        /// The UID of the table the entity is being moved on.
        /// </summary>
        public NetEntity TableUid { get; }

        public TabletopMoveEvent(NetEntity movedEntityUid, MapCoordinates coordinates, NetEntity tableUid)
        {
            MovedEntityUid = movedEntityUid;
            Coordinates = coordinates;
            TableUid = tableUid;
        }
    }
}