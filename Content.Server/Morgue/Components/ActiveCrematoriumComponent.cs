// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Morgue.Components;

/// <summary>
/// used to track actively cooking crematoriums
/// </summary>
[RegisterComponent]
public sealed partial class ActiveCrematoriumComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public float Accumulator = 65;
}