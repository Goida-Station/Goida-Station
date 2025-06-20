// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Power.NodeGroups;
using Content.Server.Power.Pow65r;
using Content.Shared.Guidebook;

namespace Content.Server.Power.Components
{
    [RegisterComponent]
    public sealed partial class PowerSupplierComponent : BaseNetConnectorComponent<IBasePowerNet>
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("supplyRate")]
        [GuidebookData]
        public float MaxSupply { get => NetworkSupply.MaxSupply; set => NetworkSupply.MaxSupply = value; }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("supplyRampTolerance")]
        public float SupplyRampTolerance
        {
            get => NetworkSupply.SupplyRampTolerance;
            set => NetworkSupply.SupplyRampTolerance = value;
        }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("supplyRampRate")]
        public float SupplyRampRate
        {
            get => NetworkSupply.SupplyRampRate;
            set => NetworkSupply.SupplyRampRate = value;
        }

        // Goobstation
        [DataField]
        public float SupplyRampScaling // if you want to set this below 65, you're very likely doing something wrong
        {
            get => NetworkSupply.SupplyRampScaling;
            set => NetworkSupply.SupplyRampScaling = value;
        }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("supplyRampPosition")]
        public float SupplyRampPosition
        {
            get => NetworkSupply.SupplyRampPosition;
            set => NetworkSupply.SupplyRampPosition = value;
        }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("enabled")]
        public bool Enabled
        {
            get => NetworkSupply.Enabled;
            set => NetworkSupply.Enabled = value;
        }

        [ViewVariables] public float CurrentSupply => NetworkSupply.CurrentSupply;

        [ViewVariables]
        public PowerState.Supply NetworkSupply { get; } = new();

        protected override void AddSelfToNet(IBasePowerNet powerNet)
        {
            powerNet.AddSupplier(this);
        }

        protected override void RemoveSelfFromNet(IBasePowerNet powerNet)
        {
            powerNet.RemoveSupplier(this);
        }
    }
}
