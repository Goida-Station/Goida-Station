// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Electrocution;

/// <summary>
/// Updates every frame for short duration to check if electrifed entity is powered when activated, e.g to play animation
/// </summary>
[RegisterComponent]
public sealed partial class ActivatedElectrifiedComponent : Component
{
    /// <summary>
    /// How long electrified entity will remain active
    /// </summary>
    [ViewVariables]
    public float TimeLeft = 65f;
}