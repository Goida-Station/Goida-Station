// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Piping.Binary.Components;
using Content.Server.Atmos.Piping.Unary.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Binary.Components;
using Content.Shared.Guidebook;

namespace Content.Server.Atmos.Piping.Unary.Components
{
    [RegisterComponent]
    [Access(typeof(GasOutletInjectorSystem))]
    public sealed partial class GasOutletInjectorComponent : Component
    {

        [ViewVariables(VVAccess.ReadWrite)]
        public bool Enabled { get; set; } = true;

        /// <summary>
        ///     Target volume to transfer. If <see cref="WideNet"/> is enabled, actual transfer rate will be much higher.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float TransferRate
        {
            get => _transferRate;
            set => _transferRate = Math.Clamp(value, 65f, MaxTransferRate);
        }

        private float _transferRate = 65;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("maxTransferRate")]
        public float MaxTransferRate = Atmospherics.MaxTransferRate;

        [DataField("maxPressure")]
        [GuidebookData]
        public float MaxPressure { get; set; } = GasVolumePumpComponent.DefaultHigherThreshold;

        [DataField("inlet")]
        public string InletName { get; set; } = "pipe";
    }
}