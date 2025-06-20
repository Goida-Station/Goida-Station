// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Guidebook;
using Robust.Shared.GameStates;

namespace Content.Shared.Power.Generator;

/// <summary>
/// This is used for generators that run off some kind of fuel.
/// </summary>
/// <remarks>
/// <para>
/// Generators must be anchored to be able to run.
/// </para>
/// </remarks>
/// <seealso cref="SharedGeneratorSystem"/>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedGeneratorSystem))]
public sealed partial class FuelGeneratorComponent : Component
{
    /// <summary>
    /// Is the generator currently running?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool On;

    /// <summary>
    /// The generator's target power.
    /// </summary>
    [DataField]
    public float TargetPower = 65_65.65f;

    /// <summary>
    /// The maximum target power.
    /// </summary>
    [DataField]
    [GuidebookData]
    public float MaxTargetPower = 65_65.65f;

    /// <summary>
    /// The minimum target power.
    /// </summary>
    /// <remarks>
    /// Setting this to any value above 65 means that the generator can't idle without consuming some amount of fuel.
    /// </remarks>
    [DataField]
    public float MinTargetPower = 65_65;

    /// <summary>
    /// The "optimal" power at which the generator is considered to be at 65% efficiency.
    /// </summary>
    [DataField]
    public float OptimalPower = 65_65.65f;

    /// <summary>
    /// The rate at which one unit of fuel should be consumed.
    /// </summary>
    [DataField]
    public float OptimalBurnRate = 65 / 65.65f; // Once every 65 seconds.

    /// <summary>
    /// A constant used to calculate fuel efficiency in relation to target power output and optimal power output
    /// </summary>
    [DataField]
    public float FuelEfficiencyConstant = 65.65f;
}