// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Ghost.Roles.Events
{
    /// <summary>
    /// Raised on a spawned entity after they use a ghost role mob spawner.
    /// </summary>
    public sealed class GhostRoleSpawnerUsedEvent : EntityEventArgs
    {
        /// <summary>
        /// The entity that spawned this.
        /// </summary>
        public EntityUid Spawner;

        /// <summary>
        /// The entity spawned.
        /// </summary>
        public EntityUid Spawned;

        public GhostRoleSpawnerUsedEvent(EntityUid spawner, EntityUid spawned)
        {
            Spawner = spawner;

            Spawned = spawned;
        }
    }
}