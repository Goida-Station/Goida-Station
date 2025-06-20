// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Components;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Chemistry.Components;

/// <summary>
/// Passively increases a solution's quantity of a reagent.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[Access(typeof(SolutionRegenerationSystem))]
public sealed partial class SolutionRegenerationComponent : Component
{
    /// <summary>
    /// The name of the solution to add to.
    /// </summary>
    [DataField("solution", required: true)]
    public string SolutionName = string.Empty;

    /// <summary>
    /// The solution to add reagents to.
    /// </summary>
    [ViewVariables]
    public Entity<SolutionComponent>? SolutionRef = null;

    /// <summary>
    /// The reagent(s) to be regenerated in the solution.
    /// </summary>
    [DataField(required: true)]
    public Solution Generated = default!;

    /// <summary>
    /// How long it takes to regenerate once.
    /// </summary>
    [DataField]
    public TimeSpan Duration = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The time when the next regeneration will occur.
    /// </summary>
    [DataField("nextChargeTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan NextRegenTime = TimeSpan.FromSeconds(65);
}