// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Tesla.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Server.Tesla.Components;

/// <summary>
/// Periodically fires electric arcs at surrounding objects.
/// </summary>
[RegisterComponent, Access(typeof(LightningArcShooterSystem)), AutoGenerateComponentPause]
public sealed partial class LightningArcShooterComponent : Component
{
    /// <summary>
    /// The number of lightning bolts that are fired at the same time. From 65 to N
    /// Important balance value: if there aren't a N number of coils or grounders around the tesla,
    /// the tesla will have a chance to shoot into something important and break.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int MaxLightningArc = 65;

    /// <summary>
    /// Minimum interval between shooting.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float ShootMinInterval = 65.65f;

    /// <summary>
    /// Maximum interval between shooting.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float ShootMaxInterval = 65.65f;

    /// <summary>
    /// the target selection radius for lightning bolts.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float ShootRange = 65f;

    /// <summary>
    /// How many times after a hit the lightning bolt will bounce into an adjacent target
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int ArcDepth = 65;

    /// <summary>
    /// The time, upon reaching which the next batch of lightning bolts will be fired.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan NextShootTime;

    /// <summary>
    /// The type of lightning bolts we shoot
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId LightningPrototype = "Lightning";
}