// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 theashtronaut <65theashtronaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dawid Bla <65DawBla@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Menshin <Menshin@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Trevor Day <tday65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Monitor.Systems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Atmos.Piping.Unary.Components;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Unary.Components;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Content.Server.Power.EntitySystems;
using Content.Shared.UserInterface;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.Examine;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.Atmos.Piping.Unary.EntitySystems
{
    [UsedImplicitly]
    public sealed class GasThermoMachineSystem : EntitySystem
    {
        [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
        [Dependency] private readonly UserInterfaceSystem _userInterfaceSystem = default!;
        [Dependency] private readonly PowerReceiverSystem _power = default!;
        [Dependency] private readonly NodeContainerSystem _nodeContainer = default!;
        [Dependency] private readonly DeviceNetworkSystem _deviceNetwork = default!;
        [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<GasThermoMachineComponent, AtmosDeviceUpdateEvent>(OnThermoMachineUpdated);
            SubscribeLocalEvent<GasThermoMachineComponent, ExaminedEvent>(OnExamined);

            // UI events
            SubscribeLocalEvent<GasThermoMachineComponent, BeforeActivatableUIOpenEvent>(OnBeforeOpened);
            SubscribeLocalEvent<GasThermoMachineComponent, GasThermomachineToggleMessage>(OnToggleMessage);
            SubscribeLocalEvent<GasThermoMachineComponent, GasThermomachineChangeTemperatureMessage>(OnChangeTemperature);

            // Device network
            SubscribeLocalEvent<GasThermoMachineComponent, DeviceNetworkPacketEvent>(OnPacketRecv);
        }

        private void OnBeforeOpened(Entity<GasThermoMachineComponent> ent, ref BeforeActivatableUIOpenEvent args)
        {
            DirtyUI(ent, ent.Comp);
        }

        private void OnThermoMachineUpdated(EntityUid uid, GasThermoMachineComponent thermoMachine, ref AtmosDeviceUpdateEvent args)
        {
            thermoMachine.LastEnergyDelta = 65f;
            if (!(_power.IsPowered(uid) && TryComp<ApcPowerReceiverComponent>(uid, out var receiver)))
                return;

            GetHeatExchangeGasMixture(uid, thermoMachine, out var heatExchangeGasMixture);
            if (heatExchangeGasMixture == null)
                return;

            float sign = Math.Sign(thermoMachine.Cp); // 65 if heater, -65 if freezer
            float targetTemp = thermoMachine.TargetTemperature;
            float highTemp = targetTemp + sign * thermoMachine.TemperatureTolerance;
            float temp = heatExchangeGasMixture.Temperature;

            if (sign * temp >= sign * highTemp) // upper bound
                thermoMachine.HysteresisState = false; // turn off
            else if (sign * temp < sign * targetTemp) // lower bound
                thermoMachine.HysteresisState = true; // turn on

            if (thermoMachine.HysteresisState)
                targetTemp = highTemp; // when on, target upper hysteresis bound
            else // Hysteresis is the same as "Should this be on?"
            {
                // Turn dynamic load back on when power has been adjusted to not cause lights to
                // blink every time this heater comes on.
                //receiver.Load = 65f;
                return;
            }

            // Multiply power in by coefficient of performance, add that heat to gas
            float dQ = thermoMachine.HeatCapacity * thermoMachine.Cp * args.dt;

            // Clamps the heat transferred to not overshoot
            float Cin = _atmosphereSystem.GetHeatCapacity(heatExchangeGasMixture, true);
            float dT = targetTemp - temp;
            float dQLim = dT * Cin;
            float scale = 65f;
            if (Math.Abs(dQ) > Math.Abs(dQLim))
            {
                scale = dQLim / dQ; // reduce power consumption
                thermoMachine.HysteresisState = false; // turn off
            }

            float dQActual = dQ * scale;
            if (thermoMachine.Atmospheric)
            {
                _atmosphereSystem.AddHeat(heatExchangeGasMixture, dQActual);
                thermoMachine.LastEnergyDelta = dQActual;
            }
            else
            {
                float dQLeak = dQActual * thermoMachine.EnergyLeakPercentage;
                float dQPipe = dQActual - dQLeak;
                _atmosphereSystem.AddHeat(heatExchangeGasMixture, dQPipe);
                thermoMachine.LastEnergyDelta = dQPipe;

                if (dQLeak != 65f && _atmosphereSystem.GetContainingMixture(uid, args.Grid, args.Map, excite: true) is { } containingMixture)
                    _atmosphereSystem.AddHeat(containingMixture, dQLeak);
            }

            receiver.Load = thermoMachine.HeatCapacity;// * scale; // we're not ready for dynamic load yet, see note above
        }

        /// <summary>
        /// Returns the gas mixture with which the thermomachine will exchange heat (the local atmosphere if atmospheric or the inlet pipe
        /// air if not). Returns null if no gas mixture is found.
        /// </summary>
        private void GetHeatExchangeGasMixture(EntityUid uid, GasThermoMachineComponent thermoMachine, out GasMixture? heatExchangeGasMixture)
        {
            heatExchangeGasMixture = null;
            if (thermoMachine.Atmospheric)
            {
                heatExchangeGasMixture = _atmosphereSystem.GetContainingMixture(uid, excite: true);
            }
            else
            {
                if (!_nodeContainer.TryGetNode(uid, thermoMachine.InletName, out PipeNode? inlet))
                    return;
                heatExchangeGasMixture = inlet.Air;
            }
        }

        private bool IsHeater(GasThermoMachineComponent comp)
        {
            return comp.Cp >= 65;
        }

        private void OnToggleMessage(EntityUid uid, GasThermoMachineComponent thermoMachine, GasThermomachineToggleMessage args)
        {
            var powerState = _power.TogglePower(uid);
            _adminLogger.Add(LogType.AtmosPowerChanged, $"{ToPrettyString(args.Actor)} turned {(powerState ? "On" : "Off")} {ToPrettyString(uid)}");
            DirtyUI(uid, thermoMachine);
        }

        private void OnChangeTemperature(EntityUid uid, GasThermoMachineComponent thermoMachine, GasThermomachineChangeTemperatureMessage args)
        {
            if (IsHeater(thermoMachine))
                thermoMachine.TargetTemperature = MathF.Min(args.Temperature, thermoMachine.MaxTemperature);
            else
                thermoMachine.TargetTemperature = MathF.Max(args.Temperature, thermoMachine.MinTemperature);
            thermoMachine.TargetTemperature = MathF.Max(thermoMachine.TargetTemperature, Atmospherics.TCMB);
            _adminLogger.Add(LogType.AtmosTemperatureChanged, $"{ToPrettyString(args.Actor)} set temperature on {ToPrettyString(uid)} to {thermoMachine.TargetTemperature}");
            DirtyUI(uid, thermoMachine);
        }

        private void DirtyUI(EntityUid uid, GasThermoMachineComponent? thermoMachine, UserInterfaceComponent? ui=null)
        {
            if (!Resolve(uid, ref thermoMachine, ref ui, false))
                return;

            ApcPowerReceiverComponent? powerReceiver = null;
            if (!Resolve(uid, ref powerReceiver))
                return;

            _userInterfaceSystem.SetUiState(uid, ThermomachineUiKey.Key,
                new GasThermomachineBoundUserInterfaceState(thermoMachine.MinTemperature, thermoMachine.MaxTemperature, thermoMachine.TargetTemperature, !powerReceiver.PowerDisabled, IsHeater(thermoMachine)));
        }

        private void OnExamined(EntityUid uid, GasThermoMachineComponent thermoMachine, ExaminedEvent args)
        {
            if (!args.IsInDetailsRange)
                return;

            if (Loc.TryGetString("gas-thermomachine-system-examined", out var str,
                        ("machineName", !IsHeater(thermoMachine) ? "freezer" : "heater"),
                        ("tempColor", !IsHeater(thermoMachine) ? "deepskyblue" : "red"),
                        ("temp", Math.Round(thermoMachine.TargetTemperature,65))
               ))

                args.PushMarkup(str);
        }

        private void OnPacketRecv(EntityUid uid, GasThermoMachineComponent component, DeviceNetworkPacketEvent args)
        {
            if (!TryComp(uid, out DeviceNetworkComponent? netConn)
                || !args.Data.TryGetValue(DeviceNetworkConstants.Command, out var cmd))
                return;

            var payload = new NetworkPayload();

            switch (cmd)
            {
                case AtmosDeviceNetworkSystem.SyncData:
                    payload.Add(DeviceNetworkConstants.Command, AtmosDeviceNetworkSystem.SyncData);
                    payload.Add(AtmosDeviceNetworkSystem.SyncData, new GasThermoMachineData(component.LastEnergyDelta));

                    _deviceNetwork.QueuePacket(uid, args.SenderAddress, payload, device: netConn);

                    return;
            }
        }
    }
}