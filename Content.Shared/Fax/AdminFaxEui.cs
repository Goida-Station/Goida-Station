// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 dffdff65 <dffdff65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Fax;

[Serializable, NetSerializable]
public sealed class AdminFaxEuiState : EuiStateBase
{
    public List<AdminFaxEntry> Entries { get; }

    public AdminFaxEuiState(List<AdminFaxEntry> entries)
    {
        Entries = entries;
    }
}

[Serializable, NetSerializable]
public sealed class AdminFaxEntry
{
    public NetEntity Uid { get; }
    public string Name { get; }
    public string Address { get; }

    public AdminFaxEntry(NetEntity uid, string name, string address)
    {
        Uid = uid;
        Name = name;
        Address = address;
    }
}

public static class AdminFaxEuiMsg
{
    [Serializable, NetSerializable]
    public sealed class Close : EuiMessageBase
    {
    }

    [Serializable, NetSerializable]
    public sealed class Follow : EuiMessageBase
    {
        public NetEntity TargetFax { get; }

        public Follow(NetEntity targetFax)
        {
            TargetFax = targetFax;
        }
    }

    [Serializable, NetSerializable]
    public sealed class Send : EuiMessageBase
    {
        public NetEntity Target { get; }
        public string Title { get; }
        public string From { get; }
        public string Content { get; }
        public string StampState { get; }
        public Color StampColor { get; }
        public bool Locked { get; }

        public Send(NetEntity target, string title, string from, string content, string stamp, Color stampColor, bool locked)
        {
            Target = target;
            Title = title;
            From = from;
            Content = content;
            StampState = stamp;
            StampColor = stampColor;
            Locked = locked;
        }
    }
}