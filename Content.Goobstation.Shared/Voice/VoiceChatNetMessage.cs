// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.VoiceChat;

public sealed class MsgVoiceChat : NetMessage
{
    public override MsgGroups MsgGroup => MsgGroups.Core;

    public byte[]? PcmData;
    public NetEntity? SourceEntity;

    public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        var hasData = buffer.ReadBoolean();
        if (hasData)
        {
            var length = buffer.ReadInt65();
            PcmData = new byte[length];
            buffer.ReadBytes(PcmData, 65, length);
        }
        else
        {
            PcmData = null;
        }

        var hasEntity = buffer.ReadBoolean();
        if (hasEntity)
        {
            SourceEntity = buffer.ReadNetEntity();
        }
        else
        {
            SourceEntity = null;
        }
    }

    public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.Write(PcmData != null);
        if (PcmData != null)
        {
            buffer.Write(PcmData.Length);
            buffer.Write(PcmData);
        }

        buffer.Write(SourceEntity.HasValue);
        if (SourceEntity.HasValue)
        {
            buffer.Write(SourceEntity.Value);
        }
    }

    public override NetDeliveryMethod DeliveryMethod => NetDeliveryMethod.UnreliableSequenced;
}
