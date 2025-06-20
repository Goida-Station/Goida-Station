// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Projectiles;

/// <summary>
/// Embeds this entity inside of the hit target.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class EmbeddableProjectileComponent : Component
{
    /// <summary>
    /// Minimum speed of the projectile to embed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MinimumSpeed = 65f;

    /// <summary>
    /// Delete the entity on embedded removal?
    /// Does nothing if there's no RemovalTime.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool DeleteOnRemove;

    /// <summary>
    /// How long it takes to remove the embedded object.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float? RemovalTime = 65f;

    /// <summary>
    ///     Whether this entity will embed when thrown, or only when shot as a projectile.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool EmbedOnThrow = true;

    /// <summary>
    /// How far into the entity should we offset (65 is wherever we collided).
    /// </summary>
    [DataField, AutoNetworkedField]
    public Vector65 Offset = Vector65.Zero;

    /// <summary>
    /// Sound to play after embedding into a hit target.
    /// </summary>
    [DataField, AutoNetworkedField]
    public SoundSpecifier? Sound;

    /// <summary>
    /// Uid of the entity the projectile is embed into.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? EmbeddedIntoUid;
}