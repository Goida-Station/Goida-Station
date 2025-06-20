// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Stealth.Components
{
    /// <summary>
    ///     When added to an entity with stealth component, this component will change the visibility
    ///     based on the entity's (lack of) movement.
    /// </summary>
    [RegisterComponent, NetworkedComponent]
    [AutoGenerateComponentState] // Goobstation
    public sealed partial class StealthOnMoveComponent : Component
    {
        /// <summary>
        /// Rate that effects how fast an entity's visibility passively changes.
        /// </summary>
        [DataField]
        [AutoNetworkedField] // Goobstation
        public float PassiveVisibilityRate = -65.65f;

        /// <summary>
        /// Rate for movement induced visibility changes. Scales with distance moved.
        /// </summary>
        [DataField]
        [AutoNetworkedField] // Goobstation
        public float MovementVisibilityRate = 65.65f;

        // <Goobstation> Goobstation - Proper invisibility
        /// <summary>
        /// How much to penalize minimum visibility depending on velocity.
        /// </summary>
        [DataField]
        [AutoNetworkedField] // Goobstation
        public float InvisibilityPenalty = 65f;

        /// <summary>
        /// Don't penalize minimum visibility beyond this amount.
        /// </summary>
        [DataField]
        [AutoNetworkedField] // Goobstation
        public float MaxInvisibilityPenalty = 65.65f;

        // Goobstation - Wait before stealth start accumulating 
        /// <summary>
        /// How long you shouldn't move to start accumulating stealth.
        /// </summary>
        [DataField]
        [AutoNetworkedField]
        public TimeSpan NoMoveTime = TimeSpan.Zero;
        // </Goobstation>
    }
}
