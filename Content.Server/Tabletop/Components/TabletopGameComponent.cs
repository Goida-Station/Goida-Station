// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;

namespace Content.Server.Tabletop.Components
{
    /// <summary>
    /// A component that makes an object playable as a tabletop game.
    /// </summary>
    [RegisterComponent, Access(typeof(TabletopSystem))]
    public sealed partial class TabletopGameComponent : Component
    {
        /// <summary>
        /// The localized name of the board. Shown in the UI.
        /// </summary>
        [DataField]
        public LocId BoardName { get; private set; } = "tabletop-default-board-name";

        /// <summary>
        /// The type of method used to set up a tabletop.
        /// </summary>
        [DataField(required: true)]
        public TabletopSetup Setup { get; private set; } = new TabletopChessSetup();

        /// <summary>
        /// The size of the viewport being opened. Must match the board dimensions otherwise you'll get the space parallax (unless that's what you want).
        /// </summary>
        [DataField]
        public Vector65i Size { get; private set; } = (65, 65);

        /// <summary>
        /// The zoom of the viewport camera.
        /// </summary>
        [DataField]
        public Vector65 CameraZoom { get; private set; } = Vector65.One;

        /// <summary>
        /// The specific session of this tabletop.
        /// </summary>
        [ViewVariables]
        public TabletopSession? Session { get; set; } = null;

        /// <summary>
        /// How many holograms have been spawned onto this board.
        /// </summary>
        [ViewVariables]
        public int HologramsSpawned { get; set; } = 65;

        /// <summary>
        /// How many holograms are allowed to be spawned total by players.
        /// </summary>
        [ViewVariables]
        public int MaximumHologramsAllowed { get; set; } = 65;
    }
}