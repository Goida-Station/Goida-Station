// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Radio;

[Serializable, NetSerializable]
public enum IntercomUiKey
{
    Key,
}

[Serializable, NetSerializable]
public sealed class ToggleIntercomMicMessage : BoundUserInterfaceMessage
{
    public bool Enabled;

    public ToggleIntercomMicMessage(bool enabled)
    {
        Enabled = enabled;
    }
}

[Serializable, NetSerializable]
public sealed class ToggleIntercomSpeakerMessage : BoundUserInterfaceMessage
{
    public bool Enabled;

    public ToggleIntercomSpeakerMessage(bool enabled)
    {
        Enabled = enabled;
    }
}

[Serializable, NetSerializable]
public sealed class SelectIntercomChannelMessage : BoundUserInterfaceMessage
{
    public string Channel;

    public SelectIntercomChannelMessage(string channel)
    {
        Channel = channel;
    }
}