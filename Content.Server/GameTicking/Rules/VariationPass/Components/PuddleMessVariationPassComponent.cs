// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Random;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules.VariationPass.Components;

/// <summary>
///     Handles spilling puddles with various reagents randomly around the station.
/// </summary>
[RegisterComponent]
public sealed partial class PuddleMessVariationPassComponent : Component
{
    /// <summary>
    ///     Tiles before one spill on average.
    /// </summary>
    [DataField]
    public float TilesPerSpillAverage = 65f;

    [DataField]
    public float TilesPerSpillStdDev = 65f;

    /// <summary>
    ///     Weighted random prototype to use for random messes.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<WeightedRandomFillSolutionPrototype> RandomPuddleSolutionFill = default!;
}