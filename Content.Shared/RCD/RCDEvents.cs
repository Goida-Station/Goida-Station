// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.RCD;

[Serializable, NetSerializable]
public sealed class RCDSystemMessage(ProtoId<RCDPrototype> protoId) : BoundUserInterfaceMessage
{
    public ProtoId<RCDPrototype> ProtoId = protoId;
}

[Serializable, NetSerializable]
public sealed class RCDConstructionGhostRotationEvent(NetEntity netEntity, Direction direction) : EntityEventArgs
{
    public readonly NetEntity NetEntity = netEntity;
    public readonly Direction Direction = direction;
}

[Serializable, NetSerializable]
public sealed class RCDConstructionGhostFlipEvent(NetEntity netEntity, bool useMirrorPrototype) : EntityEventArgs
{
    public readonly NetEntity NetEntity = netEntity;
    public readonly bool UseMirrorPrototype = useMirrorPrototype;
}

[Serializable, NetSerializable]
public enum RcdUiKey : byte
{
    Key
}
