// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.NPC;

/// <summary>
/// Has debug information for HTN NPCs.
/// </summary>
[Serializable, NetSerializable]
public sealed class HTNMessage : EntityEventArgs
{
    public NetEntity Uid;
    public string Text = string.Empty;
}