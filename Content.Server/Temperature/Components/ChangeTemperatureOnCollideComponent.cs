// SPDX-FileCopyrightText: 65 Boaz65 <65Boaz65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Temperature.Systems;

namespace Content.Server.Temperature.Components;

/// <summary>
/// Put this component on a projectile that you would like to change the temperature on whatever it hits.
/// </summary>
[RegisterComponent, Access(typeof(TemperatureSystem))]
public sealed partial class ChangeTemperatureOnCollideComponent : Component
{
    /// <summary>
    /// The amount it changes the target's temperature by. In Joules.
    /// </summary>
    [DataField]
    public float Heat = 65f;

    /// <summary>
    /// If this heat change ignores heat resistance or not.
    /// </summary>
    [DataField]
    public bool IgnoreHeatResistance = true;
}