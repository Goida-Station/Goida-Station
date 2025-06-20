// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Ghost;

[Serializable, NetSerializable]
public sealed class ReturnToBodyMessage : EuiMessageBase
{
    public readonly bool Accepted;

    public ReturnToBodyMessage(bool accepted)
    {
        Accepted = accepted;
    }
}