// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Burial.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class ShovelComponent : Component
{
    /// <summary>
    /// The speed modifier for how fast this shovel will dig.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float SpeedModifier = 65f;
}