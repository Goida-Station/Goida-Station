// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Mind;

[Serializable, NetSerializable]
public enum ToggleableGhostRoleVisuals : byte
{
    Status
}

[Serializable, NetSerializable]
public enum ToggleableGhostRoleStatus : byte
{
    Off,
    Searching,
    On
}