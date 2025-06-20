// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates; // Goobstation - Ling absorb biomatter ability
using Robust.Shared.Serialization;

namespace Content.Shared.Nutrition.Components
{
    // TODO: Remove maybe? Add visualizer for food
    [Serializable, NetSerializable]
    public enum FoodVisuals : byte
    {
        Visual,
        MaxUses,
    }

    [Serializable, NetSerializable]
    public enum OpenableVisuals : byte
    {
        Opened,
        Layer
    }

    [Serializable, NetSerializable]
    public enum SealableVisuals : byte
    {
        Sealed,
        Layer,
    }

    // Goobstation - Ling absorb biomatter ability
    public abstract partial class SharedFoodComponent : Component
    {
        /// <summary>
        ///     If this is set to true, food can only be eaten if you have a stomach with a
        ///     <see cref="StomachComponent.SpecialDigestible"/> that includes this entity in its whitelist,
        ///     rather than just being digestible by anything that can eat food.
        ///     Whitelist the food component to allow eating of normal food.
        /// </summary>
        [DataField]
        public bool RequiresSpecialDigestion;
    }
}