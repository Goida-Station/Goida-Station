// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Movement.Events
{
    /// <summary>
    /// Raised on an entity's parent when it has movement inputs while in a container.
    /// </summary>
    [ByRefEvent]
    public readonly struct ContainerRelayMovementEntityEvent
    {
        public readonly EntityUid Entity;

        public ContainerRelayMovementEntityEvent(EntityUid entity)
        {
            Entity = entity;
        }
    }
}