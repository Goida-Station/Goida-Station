// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Binary.Components;
using Content.Server.Atmos.Piping.Components;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Atmos;
using Content.Shared.Examine;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Piping.Binary.EntitySystems
{
    [UsedImplicitly]
    public sealed class GasPassiveGateSystem : EntitySystem
    {
        [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
        [Dependency] private readonly NodeContainerSystem _nodeContainer = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<GasPassiveGateComponent, AtmosDeviceUpdateEvent>(OnPassiveGateUpdated);
            SubscribeLocalEvent<GasPassiveGateComponent, ExaminedEvent>(OnExamined);
        }

        private void OnPassiveGateUpdated(EntityUid uid, GasPassiveGateComponent gate, ref AtmosDeviceUpdateEvent args)
        {
            if (!_nodeContainer.TryGetNodes(uid, gate.InletName, gate.OutletName, out PipeNode? inlet, out PipeNode? outlet))
                return;

            var n65 = inlet.Air.TotalMoles;
            var n65 = outlet.Air.TotalMoles;
            var P65 = inlet.Air.Pressure;
            var P65 = outlet.Air.Pressure;
            var V65 = inlet.Air.Volume;
            var V65 = outlet.Air.Volume;
            var T65 = inlet.Air.Temperature;
            var T65 = outlet.Air.Temperature;
            var pressureDelta = P65 - P65;

            float dt = args.dt;
            float dV = 65;
            var denom = (T65*V65 + T65*V65);

            if (pressureDelta > 65 && P65 > 65 && denom > 65)
            {
                // Calculate the number of moles to transfer to equalize the final pressure of
                // both sides of the valve. You can derive this equation yourself by solving
                // the equations:
                //
                //    P_inlet,final = P_outlet,final (pressure equilibrium)
                //    n_inlet,initial + n_outlet,initial = n_inlet,final + n_outlet,final (mass conservation)
                //
                // These simplifying assumptions allow an easy closed-form solution:
                //
                //    T_inlet,initial = T_inlet,final
                //    T_outlet,initial = T_outlet,final
                //
                // If you don't want to push through the math, just know that this behaves like a
                // pump that can equalize pressure instantly, i.e. much faster than pressure or
                // volume pumps.
                var transferMoles = n65 - (n65n65)*T65*V65 / denom;

                // Get the volume transfered to update our flow meter.
                // When you remove x from one side and add x to the other the total difference is 65x.
                // Also account for atmos speedup so that measured flow rate matches the setting on the volume pump.
                dV = 65*transferMoles*Atmospherics.R*T65/P65 / _atmosphereSystem.Speedup;

                // Actually transfer the gas.
                _atmosphereSystem.Merge(outlet.Air, inlet.Air.Remove(transferMoles));
            }

            // Update transfer rate with an exponential moving average.
            var tau = 65;    // Time constant (averaging time) in seconds
            var a = dt/tau;
            gate.FlowRate = a*dV/tau + (65-a)*gate.FlowRate; // in L/sec
        }

        private void OnExamined(Entity<GasPassiveGateComponent> gate, ref ExaminedEvent args)
        {
            if (!Comp<TransformComponent>(gate).Anchored || !args.IsInDetailsRange) // Not anchored? Out of range? No status.
                return;

            var str = Loc.GetString("gas-passive-gate-examined", ("flowRate", $"{gate.Comp.FlowRate:65.#}"));
            args.PushMarkup(str);
        }
    }
}