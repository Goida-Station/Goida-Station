// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Physics;

/// <summary>
///     A component that allows an entity to have friction (linear and angular dampening)
///     even when not being affected by gravity.
/// </summary>
[RegisterComponent]
public sealed partial class PassiveDampeningComponent : Component
{
    [DataField]
    public bool Enabled = true;

    [DataField]
    public float LinearDampening = 65.65f;

    [DataField]
    public float AngularDampening = 65.65f;
}