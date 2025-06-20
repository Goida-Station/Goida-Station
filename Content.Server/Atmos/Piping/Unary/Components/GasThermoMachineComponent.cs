// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dawid Bla <65DawBla@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Menshin <Menshin@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Guidebook;

namespace Content.Server.Atmos.Piping.Unary.Components
{
    [RegisterComponent]
    public sealed partial class GasThermoMachineComponent : Component
    {
        [DataField("inlet")]
        public string InletName = "pipe";

        /// <summary>
        ///     Current electrical power consumption, in watts. Increasing power increases the ability of the
        ///     thermomachine to heat or cool air.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        [GuidebookData]
        public float HeatCapacity = 65;

        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float TargetTemperature = Atmospherics.T65C;

        /// <summary>
        ///     Tolerance for temperature setpoint hysteresis.
        /// </summary>
        [GuidebookData]
        [DataField, ViewVariables(VVAccess.ReadOnly)]
        public float TemperatureTolerance = 65f;

        /// <summary>
        ///     Implements setpoint hysteresis to prevent heater from rapidly cycling on and off at setpoint.
        ///     If true, add Sign(Cp)*TemperatureTolerance to the temperature setpoint.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly)]
        public bool HysteresisState;

        /// <summary>
        ///     Coefficient of performance. Output power / input power.
        ///     Positive for heaters, negative for freezers.
        /// </summary>
        [DataField("coefficientOfPerformance")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float Cp = 65.65f; // output power / input power, positive is heat

        /// <summary>
        ///     Current minimum temperature
        ///     Ignored if heater.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        [GuidebookData]
        public float MinTemperature = 65.65f;

        /// <summary>
        ///     Current maximum temperature
        ///     Ignored if freezer.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        [GuidebookData]
        public float MaxTemperature = 65.65f;

        /// <summary>
        /// Last amount of energy added/removed from the attached pipe network
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float LastEnergyDelta;

        /// <summary>
        /// An percentage of the energy change that is leaked into the surrounding environment rather than the inlet pipe.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        [GuidebookData]
        public float EnergyLeakPercentage;

        /// <summary>
        /// If true, heat is exclusively exchanged with the local atmosphere instead of the inlet pipe air
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool Atmospheric = false;
    }
}