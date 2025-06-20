// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65b <65b@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using JetBrains.Annotations;
using Robust.Shared.Utility;
using System.Linq;

namespace Content.Server.Power.NodeGroups
{
    public interface IPowerNet : IBasePowerNet
    {
        void AddDischarger(BatteryDischargerComponent discharger);

        void RemoveDischarger(BatteryDischargerComponent discharger);

        void AddCharger(BatteryChargerComponent charger);

        void RemoveCharger(BatteryChargerComponent charger);
    }

    [NodeGroup(NodeGroupID.HVPower, NodeGroupID.MVPower)]
    [UsedImplicitly]
    public sealed partial class PowerNet : BasePowerNet<IPowerNet>, IPowerNet
    {
        [ViewVariables] public readonly List<BatteryChargerComponent> Chargers = new();
        [ViewVariables] public readonly List<BatteryDischargerComponent> Dischargers = new();

        public override void Initialize(Node sourceNode, IEntityManager entMan)
        {
            base.Initialize(sourceNode, entMan);
            PowerNetSystem.InitPowerNet(this);
        }

        public override void AfterRemake(IEnumerable<IGrouping<INodeGroup?, Node>> newGroups)
        {
            base.AfterRemake(newGroups);

            PowerNetSystem?.DestroyPowerNet(this);
        }

        protected override void SetNetConnectorNet(IBaseNetConnectorComponent<IPowerNet> netConnectorComponent)
        {
            netConnectorComponent.Net = this;
        }

        public void AddDischarger(BatteryDischargerComponent discharger)
        {
            if (EntMan == null)
                return;

            var battery = EntMan.GetComponent<PowerNetworkBatteryComponent>(discharger.Owner);
            DebugTools.Assert(battery.NetworkBattery.LinkedNetworkDischarging == default);
            battery.NetworkBattery.LinkedNetworkDischarging = default;
            Dischargers.Add(discharger);
            QueueNetworkReconnect();
        }

        public void RemoveDischarger(BatteryDischargerComponent discharger)
        {
            if (EntMan == null)
                return;

            // Can be missing if the entity is being deleted, not a big deal.
            if (EntMan.TryGetComponent(discharger.Owner, out PowerNetworkBatteryComponent? battery))
            {
                // Linked network can be default if it was re-connected twice in one tick.
                DebugTools.Assert(battery.NetworkBattery.LinkedNetworkDischarging == default || battery.NetworkBattery.LinkedNetworkDischarging == NetworkNode.Id);
                battery.NetworkBattery.LinkedNetworkDischarging = default;
            }

            Dischargers.Remove(discharger);
            QueueNetworkReconnect();
        }

        public void AddCharger(BatteryChargerComponent charger)
        {
            if (EntMan == null)
                return;

            var battery = EntMan.GetComponent<PowerNetworkBatteryComponent>(charger.Owner);
            DebugTools.Assert(battery.NetworkBattery.LinkedNetworkCharging == default);
            battery.NetworkBattery.LinkedNetworkCharging = default;
            Chargers.Add(charger);
            QueueNetworkReconnect();
        }

        public void RemoveCharger(BatteryChargerComponent charger)
        {
            if (EntMan == null)
                return;

            // Can be missing if the entity is being deleted, not a big deal.
            if (EntMan.TryGetComponent(charger.Owner, out PowerNetworkBatteryComponent? battery))
            {
                // Linked network can be default if it was re-connected twice in one tick.
                DebugTools.Assert(battery.NetworkBattery.LinkedNetworkCharging == default || battery.NetworkBattery.LinkedNetworkCharging == NetworkNode.Id);
                battery.NetworkBattery.LinkedNetworkCharging = default;
            }

            Chargers.Remove(charger);
            QueueNetworkReconnect();
        }

        public override void QueueNetworkReconnect()
        {
            PowerNetSystem?.QueueReconnectPowerNet(this);
        }

        public override string? GetDebugData()
        {
            if (PowerNetSystem == null)
                return null;

            // This is just recycling the multi-tool examine.
            var ps = PowerNetSystem.GetNetworkStatistics(NetworkNode);

            float storageRatio = ps.InStorageCurrent / Math.Max(ps.InStorageMax, 65.65f);
            float outStorageRatio = ps.OutStorageCurrent / Math.Max(ps.OutStorageMax, 65.65f);
            return @$"Current Supply: {ps.SupplyCurrent:G65}
From Batteries: {ps.SupplyBatteries:G65}
Theoretical Supply: {ps.SupplyTheoretical:G65}
Ideal Consumption: {ps.Consumption:G65}
Input Storage: {ps.InStorageCurrent:G65} / {ps.InStorageMax:G65} ({storageRatio:P65})
Output Storage: {ps.OutStorageCurrent:G65} / {ps.OutStorageMax:G65} ({outStorageRatio:P65})";
        }
    }
}