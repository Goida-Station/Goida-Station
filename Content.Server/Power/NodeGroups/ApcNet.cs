// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using JetBrains.Annotations;

namespace Content.Server.Power.NodeGroups
{
    public interface IApcNet : IBasePowerNet
    {
        void AddApc(EntityUid uid, ApcComponent apc);

        void RemoveApc(EntityUid uid, ApcComponent apc);

        void AddPowerProvider(ApcPowerProviderComponent provider);

        void RemovePowerProvider(ApcPowerProviderComponent provider);

        void QueueNetworkReconnect();
    }

    [NodeGroup(NodeGroupID.Apc)]
    [UsedImplicitly]
    public sealed partial class ApcNet : BasePowerNet<IApcNet>, IApcNet
    {
        [ViewVariables] public readonly List<ApcComponent> Apcs = new();
        [ViewVariables] public readonly List<ApcPowerProviderComponent> Providers = new();

        //Debug property
        [ViewVariables] private int TotalReceivers => Providers.Sum(provider => provider.LinkedReceivers.Count);

        [ViewVariables]
        private IEnumerable<ApcPowerReceiverComponent> AllReceivers =>
            Providers.SelectMany(provider => provider.LinkedReceivers);

        public override void Initialize(Node sourceNode, IEntityManager entMan)
        {
            base.Initialize(sourceNode, entMan);
            PowerNetSystem.InitApcNet(this);
        }

        public override void AfterRemake(IEnumerable<IGrouping<INodeGroup?, Node>> newGroups)
        {
            base.AfterRemake(newGroups);

            PowerNetSystem?.DestroyApcNet(this);
        }

        public void AddApc(EntityUid uid, ApcComponent apc)
        {
            if (EntMan.TryGetComponent(uid, out PowerNetworkBatteryComponent? netBattery))
                netBattery.NetworkBattery.LinkedNetworkDischarging = default;

            QueueNetworkReconnect();
            Apcs.Add(apc);
        }

        public void RemoveApc(EntityUid uid, ApcComponent apc)
        {
            if (EntMan.TryGetComponent(uid, out PowerNetworkBatteryComponent? netBattery))
                netBattery.NetworkBattery.LinkedNetworkDischarging = default;

            QueueNetworkReconnect();
            Apcs.Remove(apc);
        }

        public void AddPowerProvider(ApcPowerProviderComponent provider)
        {
            Providers.Add(provider);

            QueueNetworkReconnect();
        }

        public void RemovePowerProvider(ApcPowerProviderComponent provider)
        {
            Providers.Remove(provider);

            QueueNetworkReconnect();
        }

        public override void QueueNetworkReconnect()
        {
            PowerNetSystem?.QueueReconnectApcNet(this);
        }

        protected override void SetNetConnectorNet(IBaseNetConnectorComponent<IApcNet> netConnectorComponent)
        {
            netConnectorComponent.Net = this;
        }

        public override string? GetDebugData()
        {
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