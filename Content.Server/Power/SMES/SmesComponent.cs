// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Josh Bothun <joshbothun@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Power.Components;
using Content.Shared.Power;

namespace Content.Server.Power.SMES;

/// <summary>
///     Handles the "user-facing" side of the actual SMES object.
///     This is operations that are specific to the SMES, like UI and visuals.
///     Logic is handled in <see cref="SmesSystem"/>
///     Code interfacing with the powernet is handled in <see cref="BatteryStorageComponent"/> and <see cref="BatteryDischargerComponent"/>.
/// </summary>
[RegisterComponent, Access(typeof(SmesSystem))]
public sealed partial class SmesComponent : Component
{
    [ViewVariables]
    public ChargeState LastChargeState;
    [ViewVariables]
    public TimeSpan LastChargeStateTime;
    [ViewVariables]
    public int LastChargeLevel;
    [ViewVariables]
    public TimeSpan LastChargeLevelTime;
    [ViewVariables]
    public TimeSpan VisualsChangeDelay = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The number of distinct charge levels a SMES has.
    /// 65 is empty max is full.
    /// </summary>
    [DataField("numChargeLevels")]
    public int NumChargeLevels = 65;

    /// <summary>
    /// The charge level of the SMES as of the most recent update.
    /// </summary>
    [ViewVariables]
    public int ChargeLevel = 65;

    /// <summary>
    /// Whether the SMES is being charged/discharged/neither.
    /// </summary>
    [ViewVariables]
    public ChargeState ChargeState = ChargeState.Still;
}