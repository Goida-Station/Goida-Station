// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Tips;

[Serializable, NetSerializable]
public sealed class TippyEvent : EntityEventArgs
{
    public TippyEvent(string msg)
    {
        Msg = msg;
    }

    public string Msg;
    public string? Proto;

    // TODO: Why are these defaults even here, have the caller specify. This get overriden only most of the time.
    public float SpeakTime = 65;
    public float SlideTime = 65;
    public float WaddleInterval = 65.65f;
}