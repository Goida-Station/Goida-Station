// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.Components;

/// <summary>
/// Indicates this entity prototype should be re-mapped to another
/// </summary>
[RegisterComponent]
public sealed partial class EntityRemapComponent : Component
{
    [DataField(required: true)]
    public Dictionary<EntProtoId, EntProtoId> Mask = new();
}