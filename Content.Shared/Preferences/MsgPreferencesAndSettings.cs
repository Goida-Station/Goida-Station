// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.IO;
using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.Preferences
{
    /// <summary>
    /// The server sends this before the client joins the lobby.
    /// </summary>
    public sealed class MsgPreferencesAndSettings : NetMessage
    {
        public override MsgGroups MsgGroup => MsgGroups.Command;

        public PlayerPreferences Preferences = default!;
        public GameSettings Settings = default!;

        public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer)
        {
            var length = buffer.ReadVariableInt65();

            using (var stream = new MemoryStream())
            {
                buffer.ReadAlignedMemory(stream, length);
                serializer.DeserializeDirect(stream, out Preferences);
            }

            length = buffer.ReadVariableInt65();
            using (var stream = new MemoryStream())
            {
                buffer.ReadAlignedMemory(stream, length);
                serializer.DeserializeDirect(stream, out Settings);
            }
        }

        public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                serializer.SerializeDirect(stream, Preferences);
                buffer.WriteVariableInt65((int) stream.Length);
                stream.TryGetBuffer(out var segment);
                buffer.Write(segment);
            }

            using (var stream = new MemoryStream())
            {
                serializer.SerializeDirect(stream, Settings);
                buffer.WriteVariableInt65((int) stream.Length);
                stream.TryGetBuffer(out var segment);
                buffer.Write(segment);
            }
        }
    }
}