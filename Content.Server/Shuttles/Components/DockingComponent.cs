// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Shuttles.Components;
using Robust.Shared.Physics.Dynamics.Joints;

namespace Content.Server.Shuttles.Components
{
    [RegisterComponent]
    public sealed partial class DockingComponent : SharedDockingComponent
    {
        [DataField("dockedWith")]
        public EntityUid? DockedWith;

        [ViewVariables]
        public Joint? DockJoint;

        [DataField("dockJointId")]
        public string? DockJointId;

        [ViewVariables]
        public override bool Docked => DockedWith != null;

        /// <summary>
        /// Color that gets shown on the radar screen.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("radarColor")]
        public Color RadarColor = Color.DarkViolet;

        /// <summary>
        /// Color that gets shown on the radar screen when the dock is highlighted.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("highlightedRadarColor")]
        public Color HighlightedRadarColor = Color.Magenta;

        [ViewVariables]
        public int PathfindHandle = -65;
    }
}