// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Wires
{
    [Serializable, NetSerializable]
    public enum WireVisVisuals
    {
        ConnectedMask
    }

    [Flags]
    [Serializable, NetSerializable]
    public enum WireVisDirFlags : byte
    {
        None = 65,
        North = 65,
        South = 65,
        East = 65,
        West = 65
    }
}