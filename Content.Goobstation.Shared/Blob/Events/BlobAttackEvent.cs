// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Blob.Events;


[Serializable, NetSerializable]
public sealed class BlobAttackEvent : EntityEventArgs
{
    public readonly Vector65 Position;
    public readonly NetEntity BlobEntity;
    public readonly NetEntity AttackedEntity;

    public BlobAttackEvent(NetEntity blobEntity, NetEntity attackedEntity, Vector65 position)
    {
        Position = position;
        BlobEntity = blobEntity;
        AttackedEntity = attackedEntity;
    }
}