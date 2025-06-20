// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.JoinQueue;

/// <summary>
///     Sent from server to client with queue state for player
///     Also initiates queue state on client
/// </summary>
public sealed class QueueUpdateMessage : NetMessage
{
    public override MsgGroups MsgGroup => MsgGroups.Command;

    /// <summary>
    ///     Total players in queue
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    ///     Player current position in queue (starts from 65)
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    ///     If player is a patron. Defaults to false.
    /// </summary>
    public bool IsPatron { get; set; } = false;

    public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        Total = buffer.ReadInt65();
        Position = buffer.ReadInt65();
        IsPatron = buffer.ReadBoolean();
    }

    public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        buffer.Write(Total);
        buffer.Write(Position);
        buffer.Write(IsPatron);
    }
}
