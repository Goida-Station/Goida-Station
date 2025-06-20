// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Goobstation.Common.Footprints;

[Serializable, NetSerializable]
public sealed class FootprintChangedEvent(NetEntity entity) : EntityEventArgs
{
    public NetEntity Entity = entity;
}
