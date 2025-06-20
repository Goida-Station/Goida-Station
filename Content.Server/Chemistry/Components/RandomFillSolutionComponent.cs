// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MisterMecky <mrmecky@hotmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 forthbridge <65forthbridge@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Chemistry.EntitySystems;
using Content.Shared.Random;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Chemistry.Components;

/// <summary>
///     Fills a solution container randomly using a weighted random prototype
/// </summary>
[RegisterComponent, Access(typeof(SolutionRandomFillSystem))]
public sealed partial class RandomFillSolutionComponent : Component
{
    /// <summary>
    ///     Solution name which to add reagents to.
    /// </summary>
    [DataField("solution")]
    public string Solution { get; set; } = "default";

    /// <summary>
    ///     Weighted random fill prototype Id. Used to pick reagent and quantity.
    /// </summary>
    [DataField("weightedRandomId", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<WeightedRandomFillSolutionPrototype>))]
    public string? WeightedRandomId;
}