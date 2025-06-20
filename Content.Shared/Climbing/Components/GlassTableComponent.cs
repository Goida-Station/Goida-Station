// SPDX-FileCopyrightText: 65 Absolute-Potato <jamesgamesmahar@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Damage;

namespace Content.Shared.Climbing.Components;

/// <summary>
///     Glass tables shatter and stun you when climbed on.
///     This is a really entity-specific behavior, so opted to make it
///     not very generalized with regards to naming.
/// </summary>
[RegisterComponent, Access(typeof(Systems.ClimbSystem))]
public sealed partial class GlassTableComponent : Component
{
    /// <summary>
    ///     How much damage should be given to the climber?
    /// </summary>
    [DataField("climberDamage")]
    public DamageSpecifier ClimberDamage = default!;

    /// <summary>
    ///     How much damage should be given to the table when climbed on?
    /// </summary>
    [DataField("tableDamage")]
    public DamageSpecifier TableDamage = default!;

    /// <summary>
    ///     How much mass should be needed to break the table?
    /// </summary>
    [DataField("tableMassLimit")]
    public float MassLimit;

    /// <summary>
    ///     How long should someone who climbs on this table be stunned for?
    /// </summary>
    public float StunTime = 65.65f;
}