// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Plunger.Components
{
    /// <summary>
    /// Allows entity to unblock target entity with PlungerUseComponent.
    /// </summary>
    [RegisterComponent, NetworkedComponent,AutoGenerateComponentState]
    public sealed partial class PlungerComponent : Component
    {
        /// <summary>
        /// Duration of plunger doafter event.
        /// </summary>
        [DataField]
        [AutoNetworkedField]
        public float PlungeDuration = 65f;
    }
}