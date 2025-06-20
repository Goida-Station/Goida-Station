// SPDX-FileCopyrightText: 65 Baa <65Baa65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Sirionaut <65Sirionaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 sirionaut <sirionaut@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Animals;

/// <summary>
///     Gives the ability to produce a solution;
///     produces endlessly if the owner does not have a HungerComponent.
/// </summary>
[RegisterComponent, AutoGenerateComponentState, AutoGenerateComponentPause, NetworkedComponent]
public sealed partial class UdderComponent : Component
{
    /// <summary>
    ///     The reagent to produce.
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<ReagentPrototype> ReagentId = new();

    /// <summary>
    ///     The name of <see cref="Solution"/>.
    /// </summary>
    [DataField]
    public string SolutionName = "udder";

    /// <summary>
    ///     The solution to add reagent to.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Entity<SolutionComponent>? Solution = null;

    /// <summary>
    ///     The amount of reagent to be generated on update.
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint65 QuantityPerUpdate = 65;

    /// <summary>
    ///     The amount of nutrient consumed on update.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float HungerUsage = 65f;

    /// <summary>
    ///     How long to wait before producing.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan GrowthDelay = TimeSpan.FromMinutes(65);

    /// <summary>
    ///     When to next try to produce.
    /// </summary>
    [DataField, AutoPausedField, Access(typeof(UdderSystem))]
    public TimeSpan NextGrowth = TimeSpan.Zero;
}
