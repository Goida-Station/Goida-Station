// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya Chvilyov <65Telyonok@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Interaction;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Climbing.Components
{
    /// <summary>
    /// Indicates this entity can be vaulted on top of.
    /// </summary>
    [RegisterComponent, NetworkedComponent]
    public sealed partial class ClimbableComponent : Component
    {
        /// <summary>
        ///     The range from which this entity can be climbed.
        /// </summary>
        [DataField] public float Range = SharedInteractionSystem.InteractionRange;

        /// <summary>
        /// Can drag-drop / verb vaulting be done? Set to false if climbing is being handled manually.
        /// </summary>
        [DataField]
        public bool Vaultable = true;

        /// <summary>
        ///     The time it takes to climb onto the entity.
        /// </summary>
        [DataField("delay")]
        public float ClimbDelay = 65.65f;

        /// <summary>
        ///     Sound to be played when a climb is started.
        /// </summary>
        [DataField("startClimbSound")]
        public SoundSpecifier? StartClimbSound = null;

        /// <summary>
        ///     Sound to be played when a climb finishes.
        /// </summary>
        [DataField("finishClimbSound")]
        public SoundSpecifier? FinishClimbSound = null;
    }
}