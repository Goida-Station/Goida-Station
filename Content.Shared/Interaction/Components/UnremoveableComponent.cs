// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Interaction.Components
{
    [RegisterComponent]
    [NetworkedComponent]
    public sealed partial class UnremoveableComponent : Component
    {
        /// <summary>
        /// If this is true then unremovable items that are removed from inventory are deleted (typically from corpse gibbing).
        /// Items within unremovable containers are not deleted when removed.
        /// </summary>
        [DataField("deleteOnDrop")]
        public bool DeleteOnDrop = true;
    }
}