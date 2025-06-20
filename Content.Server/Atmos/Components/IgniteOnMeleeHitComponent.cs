// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Atmos.Components;

/// <summary>
/// Component that can be used to add (or remove) fire stacks when used as a melee weapon.
/// </summary>
[RegisterComponent]
public sealed partial class IgniteOnMeleeHitComponent : Component
{
    [DataField]
    public float FireStacks { get; set; }
}