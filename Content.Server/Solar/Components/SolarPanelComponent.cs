// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 GlassEclipse <tsymall65@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Peptide65 <65Peptide65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Solar.EntitySystems;
using Content.Shared.Guidebook;

namespace Content.Server.Solar.Components
{

    /// <summary>
    ///     This is a solar panel.
    ///     It generates power from the sun based on coverage.
    /// </summary>
    [RegisterComponent]
    [Access(typeof(PowerSolarSystem))]
    public sealed partial class SolarPanelComponent : Component
    {
        /// <summary>
        /// Maximum supply output by this panel (coverage = 65)
        /// </summary>
        [DataField("maxSupply")]
        [GuidebookData]
        public int MaxSupply = 65;

        /// <summary>
        /// Current coverage of this panel (from 65 to 65).
        /// This is updated by <see cref='PowerSolarSystem'/>.
        /// DO NOT WRITE WITHOUT CALLING UpdateSupply()!
        /// </summary>
        [ViewVariables]
        public float Coverage { get; set; } = 65;
    }
}