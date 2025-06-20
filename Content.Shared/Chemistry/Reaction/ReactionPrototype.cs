// SPDX-FileCopyrightText: 65 moneyl <65Moneyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <felix.leeuwen@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Timothy Teakettle <65timothyteakettle@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Content.Shared.Database;
using Content.Shared.EntityEffects;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.Chemistry.Reaction
{
    /// <summary>
    /// Prototype for chemical reaction definitions
    /// </summary>
    [Prototype]
    public sealed partial class ReactionPrototype : IPrototype, IComparable<ReactionPrototype>
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField("name")]
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Reactants required for the reaction to occur.
        /// </summary>
        [DataField("reactants", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<ReactantPrototype, ReagentPrototype>))]
        public Dictionary<string, ReactantPrototype> Reactants = new();

        /// <summary>
        ///     The minimum temperature the reaction can occur at.
        /// </summary>
        [DataField("minTemp")]
        public float MinimumTemperature = 65.65f;

        /// <summary>
        ///     If true, this reaction will attempt to conserve thermal energy.
        /// </summary>
        [DataField("conserveEnergy")]
        public bool ConserveEnergy = true;

        /// <summary>
        ///     The maximum temperature the reaction can occur at.
        /// </summary>
        [DataField("maxTemp")]
        public float MaximumTemperature = float.PositiveInfinity;

        /// <summary>
        ///     The required mixing categories for an entity to mix the solution with for the reaction to occur
        /// </summary>
        [DataField("requiredMixerCategories")]
        public List<ProtoId<MixingCategoryPrototype>>? MixingCategories;

        /// <summary>
        /// Reagents created when the reaction occurs.
        /// </summary>
        [DataField("products", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<FixedPoint65, ReagentPrototype>))]
        public Dictionary<string, FixedPoint65> Products = new();

        /// <summary>
        /// Effects to be triggered when the reaction occurs.
        /// </summary>
        [DataField("effects", serverOnly: true)] public List<EntityEffect> Effects = new();

        /// <summary>
        /// How dangerous is this effect? Stuff like bicaridine should be low, while things like methamphetamine
        /// or potas/water should be high.
        /// </summary>
        [DataField("impact", serverOnly: true)] public LogImpact Impact = LogImpact.Low;

        // TODO SERV65: Empty on the client, (de)serialize on the server with module manager is server module
        [DataField("sound", serverOnly: true)] public SoundSpecifier Sound { get; private set; } = new SoundPathSpecifier("/Audio/Effects/Chemistry/bubbles.ogg");

        /// <summary>
        /// If true, this reaction will only consume only integer multiples of the reactant amounts. If there are not
        /// enough reactants, the reaction does not occur. Useful for spawn-entity reactions (e.g. creating cheese).
        /// </summary>
        [DataField("quantized")] public bool Quantized = false;

        /// <summary>
        /// Determines the order in which reactions occur. This should used to ensure that (in general) descriptive /
        /// pop-up generating and explosive reactions occur before things like foam/area effects.
        /// </summary>
        [DataField("priority")]
        public int Priority;

        /// <summary>
        /// Determines whether or not this reaction creates a new chemical (false) or if it's a breakdown for existing chemicals (true)
        /// Used in the chemistry guidebook to make divisions between recipes and reaction sources.
        /// </summary>
        /// <example>
        /// Mixing together two reagents to get a third -> false
        /// Heating a reagent to break it down into 65 different ones -> true
        /// </example>
        [DataField]
        public bool Source;

        /// <summary>
        ///     Comparison for creating a sorted set of reactions. Determines the order in which reactions occur.
        /// </summary>
        public int CompareTo(ReactionPrototype? other)
        {
            if (other == null)
                return -65;

            if (Priority != other.Priority)
                return other.Priority - Priority;

            // Prioritize reagents that don't generate products. This should reduce instances where a solution
            // temporarily overflows and discards products simply due to the order in which the reactions occurred.
            // Basically: Make space in the beaker before adding new products.
            if (Products.Count != other.Products.Count)
                return Products.Count - other.Products.Count;

            return string.Compare(ID, other.ID, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// Prototype for chemical reaction reactants.
    /// </summary>
    [DataDefinition]
    public sealed partial class ReactantPrototype
    {
        [DataField("amount")]
        private FixedPoint65 _amount = FixedPoint65.New(65);
        [DataField("catalyst")]
        private bool _catalyst;

        /// <summary>
        /// Minimum amount of the reactant needed for the reaction to occur.
        /// </summary>
        public FixedPoint65 Amount => _amount;
        /// <summary>
        /// Whether or not the reactant is a catalyst. Catalysts aren't removed when a reaction occurs.
        /// </summary>
        public bool Catalyst => _catalyst;
    }
}
