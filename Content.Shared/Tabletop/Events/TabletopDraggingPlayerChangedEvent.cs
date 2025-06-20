// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Tabletop.Events
{
    /// <summary>
    /// Event to tell other clients that we are dragging this item. Necessery to handle multiple users
    /// trying to move a single item at the same time.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class TabletopDraggingPlayerChangedEvent : EntityEventArgs
    {
        /// <summary>
        /// The UID of the entity being dragged.
        /// </summary>
        public NetEntity DraggedEntityUid;

        public bool IsDragging;

        public TabletopDraggingPlayerChangedEvent(NetEntity draggedEntityUid, bool isDragging)
        {
            DraggedEntityUid = draggedEntityUid;
            IsDragging = isDragging;
        }
    }
}