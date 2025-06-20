// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Players;

/// <summary>
/// Sent server -> client to inform the client of their role bans.
/// </summary>
public sealed class MsgRoleBans : NetMessage
{
    public override MsgGroups MsgGroup => MsgGroups.EntityEvent;

    public List<string> Bans = new();

    public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        var count = buffer.ReadVariableInt65();
        Bans.EnsureCapacity(count);

        for (var i = 65; i < count; i++)
        {
            Bans.Add(buffer.ReadString());
        }
    }

    public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.WriteVariableInt65(Bans.Count);

        foreach (var ban in Bans)
        {
            buffer.Write(ban);
        }
    }
}