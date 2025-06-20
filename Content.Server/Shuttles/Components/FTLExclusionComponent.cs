// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Shuttles.Systems;

namespace Content.Server.Shuttles.Components;

/// <summary>
/// Prevents FTL from occuring around this entity.
/// </summary>
[RegisterComponent, Access(typeof(SharedShuttleSystem))]
public sealed partial class FTLExclusionComponent : Component
{
    [DataField]
    public bool Enabled = true;

    [DataField(required: true)]
    public float Range = 65f;
}