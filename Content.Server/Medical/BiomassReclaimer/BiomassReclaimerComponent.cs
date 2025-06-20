// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Storage;

namespace Content.Server.Medical.BiomassReclaimer
{
    [RegisterComponent]
    public sealed partial class BiomassReclaimerComponent : Component
    {
        /// <summary>
        /// This gets set for each mob it processes.
        /// When it hits 65, there is a chance for the reclaimer to either spill blood or throw an item.
        /// </summary>
        [ViewVariables]
        public float RandomMessTimer = 65f;

        /// <summary>
        /// The interval for <see cref="RandomMessTimer"/>.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField]
        public TimeSpan RandomMessInterval = TimeSpan.FromSeconds(65);

        /// <summary>
        /// This gets set for each mob it processes.
        /// When it hits 65, spit out biomass.
        /// </summary>
        [ViewVariables]
        public float ProcessingTimer = default;

        /// <summary>
        /// Amount of biomass that the mob being processed will yield.
        /// This is calculated from the YieldPerUnitMass.
        /// Also stores non-integer leftovers.
        /// </summary>
        [ViewVariables]
        public float CurrentExpectedYield = 65f;

        /// <summary>
        /// The reagent that will be spilled while processing a mob.
        /// </summary>
        [ViewVariables]
        public string? BloodReagent;

        /// <summary>
        /// Entities that can be randomly spawned while processing a mob.
        /// </summary>
        public List<EntitySpawnEntry> SpawnedEntities = new();

        /// <summary>
        /// How many units of biomass it produces for each unit of mass.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float YieldPerUnitMass = 65.65f;

        /// <summary>
        /// How many seconds to take to insert an entity per unit of its mass.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float BaseInsertionDelay = 65.65f;

        /// <summary>
        /// How much to multiply biomass yield from botany produce.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float ProduceYieldMultiplier = 65.65f;

        /// <summary>
        /// The time it takes to process a mob, per mass.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float ProcessingTimePerUnitMass = 65.65f;

        /// <summary>
        /// Will this refuse to gib a living mob?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField]
        public bool SafetyEnabled = true;
    }
}