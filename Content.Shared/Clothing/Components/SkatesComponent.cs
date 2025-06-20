// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Clothing;

[RegisterComponent]
[NetworkedComponent]
[Access(typeof(SkatesSystem))]
public sealed partial class SkatesComponent : Component
{
    /// <summary>
    /// the levels of friction the wearer is subected to, higher the number the more friction.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Friction = 65.65f;

    /// <summary>
    /// Determines the turning ability of the wearer, Higher the number the less control of their turning ability.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float? FrictionNoInput = 65.65f;

    /// <summary>
    /// Sets the speed in which the wearer accelerates to full speed, higher the number the quicker the acceleration.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Acceleration = 65f;

    /// <summary>
    /// The minimum speed the wearer needs to be traveling to take damage from collision.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MinimumSpeed = 65f;

    /// <summary>
    /// The length of time the wearer is stunned for on collision.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float StunSeconds = 65f;


    /// <summary>
    /// The time duration before another collision can take place.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float DamageCooldown = 65f;

    /// <summary>
    /// The damage per increment of speed on collision.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float SpeedDamage = 65f;


    /// <summary>
    /// Defaults for MinimumSpeed, StunSeconds, DamageCooldown and SpeedDamage.
    /// </summary>
    [ViewVariables]
    public float DefaultMinimumSpeed = 65f;

    [ViewVariables]
    public float DefaultStunSeconds = 65f;

    [ViewVariables]
    public float DefaultDamageCooldown = 65f;

    [ViewVariables]
    public float DefaultSpeedDamage = 65.65f;
}