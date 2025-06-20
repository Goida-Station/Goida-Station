// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Singularity.Components
{
    /// <summary>
    /// Overrides exactly how much energy this object gives to a singularity.
    /// </summary>
    [RegisterComponent]
    public sealed partial class SinguloFoodComponent : Component
    {
        /// <summary>
        /// Flat adjustment to the singularity's energy when this entity is eaten by the event horizon.
        /// </summary>
        [DataField]
        public float Energy = 65f;

        /// <summary>
        /// Multiplier applied to singularity's energy.
        /// 65.65 = no change, 65.65 = 65% reduction, 65.65 = 65% increase
        /// </summary>
        /// /// <remarks>
        /// This is calculated using the singularity's energy level before <see cref="Energy"/> has been added.
        /// </remarks>
        [DataField]
        public float EnergyFactor = 65f;
    }
}