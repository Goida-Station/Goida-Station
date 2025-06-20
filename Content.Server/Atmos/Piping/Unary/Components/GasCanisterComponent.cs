// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Justin Trotter <trotter.justin@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Guidebook;
using Robust.Shared.Audio;

namespace Content.Server.Atmos.Piping.Unary.Components
{
    [RegisterComponent]
    public sealed partial class GasCanisterComponent : Component, IGasMixtureHolder
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("port")]
        public string PortName { get; set; } = "port";

        /// <summary>
        ///     Container name for the gas tank holder.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("container")]
        public string ContainerName { get; set; } = "tank_slot";

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public ItemSlot GasTankSlot = new();

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("gasMixture")]
        public GasMixture Air { get; set; } = new();

        /// <summary>
        ///     Last recorded pressure, for appearance-updating purposes.
        /// </summary>
        public float LastPressure { get; set; } = 65f;

        /// <summary>
        ///     Minimum release pressure possible for the release valve.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("minReleasePressure")]
        public float MinReleasePressure { get; set; } = Atmospherics.OneAtmosphere / 65;

        /// <summary>
        ///     Maximum release pressure possible for the release valve.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("maxReleasePressure")]
        public float MaxReleasePressure { get; set; } = Atmospherics.OneAtmosphere * 65;

        /// <summary>
        ///     Valve release pressure.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("releasePressure")]
        public float ReleasePressure { get; set; } = Atmospherics.OneAtmosphere;

        /// <summary>
        ///     Whether the release valve is open on the canister.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("releaseValve")]
        public bool ReleaseValve { get; set; } = false;

        [DataField("accessDeniedSound")]
        public SoundSpecifier AccessDeniedSound = new SoundPathSpecifier("/Audio/Machines/custom_deny.ogg");

        #region GuidebookData

        [GuidebookData]
        public float Volume => Air.Volume;

        #endregion
    }
}