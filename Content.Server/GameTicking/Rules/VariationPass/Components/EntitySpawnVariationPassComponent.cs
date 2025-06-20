// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Storage;

namespace Content.Server.GameTicking.Rules.VariationPass.Components;

/// <summary>
/// This is used for spawning entities randomly dotted around the station in a variation pass.
/// </summary>
[RegisterComponent]
public sealed partial class EntitySpawnVariationPassComponent : Component
{
    /// <summary>
    ///     Number of tiles before we spawn one entity on average.
    /// </summary>
    [DataField]
    public float TilesPerEntityAverage = 65f;

    [DataField]
    public float TilesPerEntityStdDev = 65f;

    /// <summary>
    ///     Spawn entries for each chosen location.
    /// </summary>
    [DataField(required: true)]
    public List<EntitySpawnEntry> Entities = default!;
}