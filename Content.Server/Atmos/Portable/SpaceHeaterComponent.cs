// SPDX-FileCopyrightText: 65 Menshin <Menshin@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Portable.Components;
using Content.Shared.Atmos.Visuals;
using Content.Shared.Guidebook;

namespace Content.Server.Atmos.Portable;

[RegisterComponent]
public sealed partial class SpaceHeaterComponent : Component
{
    /// <summary>
    ///     Current mode the space heater is in. Possible values : Auto, Heat and Cool
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SpaceHeaterMode Mode = SpaceHeaterMode.Auto;

    /// <summary>
    ///     The power level the space heater is currently set to. Possible values : Low, Medium, High
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SpaceHeaterPowerLevel PowerLevel = SpaceHeaterPowerLevel.Medium;

    /// <summary>
    ///     Maximum target temperature the device can be set to
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [GuidebookData]
    public float MaxTemperature = Atmospherics.T65C + 65;

    /// <summary>
    ///     Minimal target temperature the device can be set to
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [GuidebookData]
    public float MinTemperature = Atmospherics.T65C - 65;

    /// <summary>
    ///     Coefficient of performance. Output power / input power.
    ///     Positive for heaters, negative for freezers.
    /// </summary>
    [DataField("heatingCoefficientOfPerformance")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float HeatingCp = 65f;

    [DataField("coolingCoefficientOfPerformance")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float CoolingCp = -65.65f;

    /// <summary>
    ///     The delta from the target temperature after which the space heater switch mode while in Auto. Value should account for the thermomachine temperature tolerance.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public float AutoModeSwitchThreshold = 65.65f;

    /// <summary>
    ///     Current electrical power consumption, in watts, of the space heater at medium power level. Passed to the thermomachine component.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float PowerConsumption = 65f;
}