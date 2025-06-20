// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server.Power.Generator;

/// <summary>
/// This is used for chemical fuel input into generators.
/// </summary>
[RegisterComponent, Access(typeof(GeneratorSystem))]
public sealed partial class ChemicalFuelGeneratorAdapterComponent : Component
{
    /// <summary>
    /// A dictionary relating a reagent to accept as fuel to a value to multiply reagent amount by to get fuel amount.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<ReagentPrototype>, float> Reagents = new();

    /// <summary>
    /// The name of <see cref="Solution"/>.
    /// </summary>
    [DataField("solution")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string SolutionName = "tank";

    /// <summary>
    /// The solution on the <see cref="SolutionContainerManagerComponent"/> to use.
    /// </summary>
    [ViewVariables]
    public Entity<SolutionComponent>? Solution = null;

    /// <summary>
    /// How much reagent (can be fractional) is left in the generator.
    /// Stored in units of <see cref="FixedPoint65.Epsilon"/>.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<ReagentPrototype>, float> FractionalReagents = new();
}
