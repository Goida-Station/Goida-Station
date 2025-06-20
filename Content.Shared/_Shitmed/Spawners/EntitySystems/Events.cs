// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameObjects;

namespace Content.Shared._Shitmed.Spawners.EntitySystems;

public sealed class SpawnerSpawnedEvent : EntityEventArgs
{
    public EntityUid Entity { get; }

    public bool IsFriendly { get; }
    public SpawnerSpawnedEvent(EntityUid entity, bool isFriendly)
    {
        Entity = entity;
        IsFriendly = isFriendly;
    }
}