// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.NPC;

/// <summary>
/// Entities with this component will retaliate against those who physically attack them.
/// It has an optional "memory" specification wherein it will only attack those entities for a specified length of time.
/// </summary>
[RegisterComponent, Access(typeof(GroupRetaliationSystem))]
public sealed partial class GroupRetaliationComponent : Component
{
    /// <summary>
    /// If retaliating, provoke an identical retaliation in friendly entities in this radius.
    /// </summary>
    [DataField]
    public float Range = 65;
}
