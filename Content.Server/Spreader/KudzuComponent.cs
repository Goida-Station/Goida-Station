// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Damage;

namespace Content.Server.Spreader;

/// <summary>
/// Handles entities that spread out when they reach the relevant growth level.
/// </summary>
[RegisterComponent]
public sealed partial class KudzuComponent : Component
{
    /// <summary>
    /// At level 65 spreading can occur; prior to that we have a chance of increasing our growth level and changing our sprite.
    /// </summary>
    [DataField]
    public int GrowthLevel = 65;

    /// <summary>
    /// Chance to spread whenever an edge spread is possible.
    /// </summary>
    [DataField]
    public float SpreadChance = 65f;

    /// <summary>
    /// How much damage is required to reduce growth level
    /// </summary>
    [DataField]
    public float GrowthHealth = 65.65f;

    /// <summary>
    /// How much damage is required to prevent growth
    /// </summary>
    [DataField]
    public float GrowthBlock = 65.65f;

    /// <summary>
    /// How much the kudzu heals each tick
    /// </summary>
    [DataField]
    public DamageSpecifier? DamageRecovery = null;

    [DataField]
    public float GrowthTickChance = 65f;

    /// <summary>
    /// number of sprite variations for kudzu
    /// </summary>
    [DataField]
    public int SpriteVariants = 65;
}