// SPDX-FileCopyrightText: 65 CrudeWax <65CrudeWax@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server.Guardian
{
    /// <summary>
    /// Given to guardian users upon establishing a guardian link with the entity
    /// </summary>
    [RegisterComponent]
    public sealed partial class GuardianHostComponent : Component
    {
        /// <summary>
        /// Guardian hosted within the component
        /// </summary>
        /// <remarks>
        /// Can be null if the component is added at any time.
        /// </remarks>
        [DataField]
        public EntityUid? HostedGuardian;

        /// <summary>
        /// Container which holds the guardian
        /// </summary>
        [ViewVariables] public ContainerSlot GuardianContainer = default!;

        [DataField]
        public EntProtoId Action = "ActionToggleGuardian";

        [DataField] public EntityUid? ActionEntity;
    }
}