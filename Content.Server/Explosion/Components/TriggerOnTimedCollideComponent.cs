// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Explosion.Components;

/// <summary>
/// Triggers when the entity is overlapped for the specified duration.
/// </summary>
[RegisterComponent]
public sealed partial class TriggerOnTimedCollideComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("threshold")]
    public float Threshold;

    /// <summary>
    /// A collection of entities that are colliding with this, and their own unique accumulator.
    /// </summary>
    [ViewVariables]
    public readonly Dictionary<EntityUid, float> Colliding = new();
}