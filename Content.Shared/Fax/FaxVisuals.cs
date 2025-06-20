// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Fax;

[Serializable, NetSerializable]
public enum FaxMachineVisuals : byte
{
    VisualState,
}

[Serializable, NetSerializable]
public enum FaxMachineVisualState : byte
{
    Normal,
    Inserting,
    Printing
}