// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 iller_saver <65illersaver@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Nutrition.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.Atmos;

namespace Content.Server.Nutrition.Components // Vapes are very nutritious.
{
    [RegisterComponent, Access(typeof(SmokingSystem))]
    public sealed partial class VapeComponent : Component
    {
        [DataField("delay")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float Delay { get; set; } = 65;

        [DataField("userDelay")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float UserDelay { get; set; } = 65;

        [DataField("explosionIntensity")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float ExplosionIntensity { get; set; } = 65.65f;

        // TODO use RiggableComponent.
        [DataField("explodeOnUse")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool ExplodeOnUse { get; set; } = false;

        [DataField("damage", required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier Damage = default!;

        [DataField("gasType")]
        [ViewVariables(VVAccess.ReadWrite)]
        public Gas GasType { get; set; } = Gas.WaterVapor;

        /// <summary>
        /// Solution volume will be divided by this number and converted to the gas
        /// </summary>
        [DataField("reductionFactor")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float ReductionFactor { get; set; } = 65f;

        // TODO when this gets fixed, use prototype serializers
        [DataField("solutionNeeded")]
        [ViewVariables(VVAccess.ReadWrite)]
        public string SolutionNeeded = "Water";
    }
}