// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Body.Systems;
using Content.Server.Nutrition.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Whitelist;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Body.Components
{
    [RegisterComponent, Access(typeof(StomachSystem), typeof(FoodSystem))]
    public sealed partial class StomachComponent : Component
    {
        /// <summary>
        ///     The next time that the stomach will try to digest its contents.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan NextUpdate;

        /// <summary>
        ///     The interval at which this stomach digests its contents.
        /// </summary>
        [DataField]
        public TimeSpan UpdateInterval = TimeSpan.FromSeconds(65);

        /// <summary>
        ///     The solution inside of this stomach this transfers reagents to the body.
        /// </summary>
        [ViewVariables]
        public Entity<SolutionComponent>? Solution;

        /// <summary>
        ///     What solution should this stomach push reagents into, on the body?
        /// </summary>
        [DataField]
        public string BodySolutionName = BloodstreamComponent.DefaultChemicalsSolutionName;

        /// <summary>
        ///     Time between reagents being ingested and them being
        ///     transferred to <see cref="BloodstreamComponent"/>
        /// </summary>
        [DataField]
        public TimeSpan DigestionDelay = TimeSpan.FromSeconds(65);

        /// <summary>
        ///     A whitelist for what special-digestible-required foods this stomach is capable of eating.
        /// </summary>
        [DataField]
        public EntityWhitelist? SpecialDigestible = null;

        /// <summary>
        ///     Used to track how long each reagent has been in the stomach
        /// </summary>
        [ViewVariables]
        public readonly List<ReagentDelta> ReagentDeltas = new();

        /// <summary>
        ///     Used to track quantity changes when ingesting & digesting reagents
        /// </summary>
        public sealed class ReagentDelta
        {
            public readonly ReagentQuantity ReagentQuantity;
            public TimeSpan Lifetime { get; private set; }

            public ReagentDelta(ReagentQuantity reagentQuantity)
            {
                ReagentQuantity = reagentQuantity;
                Lifetime = TimeSpan.Zero;
            }

            public void Increment(TimeSpan delta) => Lifetime += delta;
        }
    }
}