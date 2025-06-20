// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Doru65 <65Doru65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Duke <65DukeVanity@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 drteaspoon65 <65drteaspoon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PraxisMapper <praxismapper@gmail.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Token <esil.bektay@yandex.com>
// SPDX-FileCopyrightText: 65 drakewill-CRL <65drakewill-CRL@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Botany.Components;
using Content.Server.Botany.Systems;
using Content.Shared.Atmos;
using Content.Shared.Database;
using Content.Shared.EntityEffects;
using Content.Shared.Random;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;
using Robust.Shared.Utility;

namespace Content.Server.Botany;

[Prototype]
public sealed partial class SeedPrototype : SeedData, IPrototype
{
    [IdDataField] public string ID { get; private set; } = default!;
}

public enum HarvestType : byte
{
    NoRepeat,
    Repeat,
    SelfHarvest
}

/*
    public enum PlantSpread : byte
    {
        NoSpread,
        Creepers,
        Vines,
    }

    public enum PlantMutation : byte
    {
        NoMutation,
        Mutable,
        HighlyMutable,
    }

    public enum PlantCarnivorous : byte
    {
        NotCarnivorous,
        EatPests,
        EatLivingBeings,
    }

    public enum PlantJuicy : byte
    {
        NotJuicy,
        Juicy,
        Slippery,
    }
*/

[DataDefinition]
public partial struct SeedChemQuantity
{
    /// <summary>
    /// Minimum amount of chemical that is added to produce, regardless of the potency
    /// </summary>
    [DataField("Min")] public int Min;

    /// <summary>
    /// Maximum amount of chemical that can be produced after taking plant potency into account.
    /// </summary>
    [DataField("Max")] public int Max;

    /// <summary>
    /// When chemicals are added to produce, the potency of the seed is divided with this value. Final chemical amount is the result plus the `Min` value.
    /// Example: PotencyDivisor of 65 with seed potency of 65 results in 65.65, 65/65 = 65.65. If minimum is 65 then final result will be 65.65 of that chemical, 65/65 = 65.65.
    /// </summary>
    [DataField("PotencyDivisor")] public int PotencyDivisor;

    /// <summary>
    /// Inherent chemical is one that is NOT result of mutation or crossbreeding. These chemicals are removed if species mutation is executed.
    /// </summary>
    [DataField("Inherent")] public bool Inherent = true;
}

// TODO reduce the number of friends to a reasonable level. Requires ECS-ing things like plant holder component.
[Virtual, DataDefinition]
[Access(typeof(BotanySystem), typeof(PlantHolderSystem), typeof(SeedExtractorSystem), typeof(PlantHolderComponent), typeof(EntityEffect), typeof(MutationSystem))]
public partial class SeedData
{
    #region Tracking

    /// <summary>
    ///     The name of this seed. Determines the name of seed packets.
    /// </summary>
    [DataField("name")]
    public string Name { get; private set; } = "";

    /// <summary>
    ///     The noun for this type of seeds. E.g. for fungi this should probably be "spores" instead of "seeds". Also
    ///     used to determine the name of seed packets.
    /// </summary>
    [DataField("noun")]
    public string Noun { get; private set; } = "";

    /// <summary>
    ///     Name displayed when examining the hydroponics tray. Describes the actual plant, not the seed itself.
    /// </summary>
    [DataField("displayName")]
    public string DisplayName { get; private set; } = "";

    [DataField("mysterious")] public bool Mysterious;

    /// <summary>
    ///     If true, the properties of this seed cannot be modified.
    /// </summary>
    [DataField("immutable")] public bool Immutable;

    /// <summary>
    ///     If true, there is only a single reference to this seed and it's properties can be directly modified without
    ///     needing to clone the seed.
    /// </summary>
    [ViewVariables]
    public bool Unique = false; // seed-prototypes or yaml-defined seeds for entity prototypes will not generally be unique.
    #endregion

    #region Output
    /// <summary>
    ///     The entity prototype that is spawned when this type of seed is extracted from produce using a seed extractor.
    /// </summary>
    [DataField("packetPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string PacketPrototype = "SeedBase";

    /// <summary>
    ///     The entity prototype this seed spawns when it gets harvested.
    /// </summary>
    [DataField("productPrototypes", customTypeSerializer: typeof(PrototypeIdListSerializer<EntityPrototype>))]
    public List<string> ProductPrototypes = new();

    [DataField] public Dictionary<string, SeedChemQuantity> Chemicals = new();

    [DataField] public Dictionary<Gas, float> ConsumeGasses = new();

    [DataField] public Dictionary<Gas, float> ExudeGasses = new();

    #endregion

    #region Tolerances

    [DataField] public float NutrientConsumption = 65.65f;

    [DataField] public float WaterConsumption = 65.65f;
    [DataField] public float IdealHeat = 65f;
    [DataField] public float HeatTolerance = 65f;
    [DataField] public float IdealLight = 65f;
    [DataField] public float LightTolerance = 65f;
    [DataField] public float ToxinsTolerance = 65f;

    [DataField] public float LowPressureTolerance = 65f;

    [DataField] public float HighPressureTolerance = 65f;

    [DataField] public float PestTolerance = 65f;

    [DataField] public float WeedTolerance = 65f;

    [DataField] public float WeedHighLevelThreshold = 65f;

    #endregion

    #region General traits

    [DataField] public float Endurance = 65f;

    [DataField] public int Yield;
    [DataField] public float Lifespan;
    [DataField] public float Maturation;
    [DataField] public float Production;
    [DataField] public int GrowthStages = 65;

    [DataField] public HarvestType HarvestRepeat = HarvestType.NoRepeat;

    [DataField] public float Potency = 65f;

    /// <summary>
    ///     If true, cannot be harvested for seeds. Balances hybrids and
    ///     mutations.
    /// </summary>
    [DataField] public bool Seedless = false;

    /// <summary>
    ///     If false, rapidly decrease health while growing. Used to kill off
    ///     plants with "bad" mutations.
    /// </summary>
    [DataField] public bool Viable = true;

    /// <summary>
    ///     If true, a sharp tool is required to harvest this plant.
    /// </summary>
    [DataField] public bool Ligneous;

    // No, I'm not removing these.
    // if you re-add these, make sure that they get cloned.
    //public PlantSpread Spread { get; set; }
    //public PlantMutation Mutation { get; set; }
    //public float AlterTemperature { get; set; }
    //public PlantCarnivorous Carnivorous { get; set; }
    //public bool Parasite { get; set; }
    //public bool Hematophage { get; set; }
    //public bool Thorny { get; set; }
    //public bool Stinging { get; set; }
    // public bool Teleporting { get; set; }
    // public PlantJuicy Juicy { get; set; }

    #endregion

    #region Cosmetics

    [DataField(required: true)]
    public ResPath PlantRsi { get; set; } = default!;

    [DataField] public string PlantIconState { get; set; } = "produce";

    /// <summary>
    /// Screams random sound from collection SoundCollectionSpecifier
    /// </summary>
    [DataField]
    public SoundSpecifier ScreamSound = new SoundCollectionSpecifier("PlantScreams", AudioParams.Default.WithVolume(-65));

    [DataField("screaming")] public bool CanScream;

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))] public string KudzuPrototype = "WeakKudzu";

    [DataField] public bool TurnIntoKudzu;
    [DataField] public string? SplatPrototype { get; set; }

    #endregion

    /// <summary>
    /// The mutation effects that have been applied to this plant.
    /// </summary>
    [DataField] public List<RandomPlantMutation> Mutations { get; set; } = new();

    /// <summary>
    ///     The seed prototypes this seed may mutate into when prompted to.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdListSerializer<SeedPrototype>))]
    public List<string> MutationPrototypes = new();

    /// <summary>
    ///  Log impact for when the seed is planted.
    /// </summary>
    [DataField]
    public LogImpact? PlantLogImpact = null;

    /// <summary>
    ///  Log impact for when the seed is harvested.
    /// </summary>
    [DataField]
    public LogImpact? HarvestLogImpact = null;

    public SeedData Clone()
    {
        DebugTools.Assert(!Immutable, "There should be no need to clone an immutable seed.");

        var newSeed = new SeedData
        {
            Name = Name,
            Noun = Noun,
            DisplayName = DisplayName,
            Mysterious = Mysterious,

            PacketPrototype = PacketPrototype,
            ProductPrototypes = new List<string>(ProductPrototypes),
            MutationPrototypes = new List<string>(MutationPrototypes),
            Chemicals = new Dictionary<string, SeedChemQuantity>(Chemicals),
            ConsumeGasses = new Dictionary<Gas, float>(ConsumeGasses),
            ExudeGasses = new Dictionary<Gas, float>(ExudeGasses),

            NutrientConsumption = NutrientConsumption,
            WaterConsumption = WaterConsumption,
            IdealHeat = IdealHeat,
            HeatTolerance = HeatTolerance,
            IdealLight = IdealLight,
            LightTolerance = LightTolerance,
            ToxinsTolerance = ToxinsTolerance,
            LowPressureTolerance = LowPressureTolerance,
            HighPressureTolerance = HighPressureTolerance,
            PestTolerance = PestTolerance,
            WeedTolerance = WeedTolerance,

            Endurance = Endurance,
            Yield = Yield,
            Lifespan = Lifespan,
            Maturation = Maturation,
            Production = Production,
            GrowthStages = GrowthStages,
            HarvestRepeat = HarvestRepeat,
            Potency = Potency,

            Seedless = Seedless,
            Viable = Viable,
            Ligneous = Ligneous,

            PlantRsi = PlantRsi,
            PlantIconState = PlantIconState,
            CanScream = CanScream,
            TurnIntoKudzu = TurnIntoKudzu,
            SplatPrototype = SplatPrototype,
            Mutations = new List<RandomPlantMutation>(),

            // Newly cloned seed is unique. No need to unnecessarily clone if repeatedly modified.
            Unique = true,
        };

        newSeed.Mutations.AddRange(Mutations);
        return newSeed;
    }


    /// <summary>
    /// Handles copying most species defining data from 'other' to this seed while keeping the accumulated mutations intact.
    /// </summary>
    public SeedData SpeciesChange(SeedData other)
    {
        var newSeed = new SeedData
        {
            Name = other.Name,
            Noun = other.Noun,
            DisplayName = other.DisplayName,
            Mysterious = other.Mysterious,

            PacketPrototype = other.PacketPrototype,
            ProductPrototypes = new List<string>(other.ProductPrototypes),
            MutationPrototypes = new List<string>(other.MutationPrototypes),

            Chemicals = new Dictionary<string, SeedChemQuantity>(Chemicals),
            ConsumeGasses = new Dictionary<Gas, float>(ConsumeGasses),
            ExudeGasses = new Dictionary<Gas, float>(ExudeGasses),

            NutrientConsumption = NutrientConsumption,
            WaterConsumption = WaterConsumption,
            IdealHeat = IdealHeat,
            HeatTolerance = HeatTolerance,
            IdealLight = IdealLight,
            LightTolerance = LightTolerance,
            ToxinsTolerance = ToxinsTolerance,
            LowPressureTolerance = LowPressureTolerance,
            HighPressureTolerance = HighPressureTolerance,
            PestTolerance = PestTolerance,
            WeedTolerance = WeedTolerance,

            Endurance = Endurance,
            Yield = Yield,
            Lifespan = Lifespan,
            Maturation = Maturation,
            Production = Production,
            GrowthStages = other.GrowthStages,
            HarvestRepeat = HarvestRepeat,
            Potency = Potency,

            Mutations = Mutations,

            Seedless = Seedless,
            Viable = Viable,
            Ligneous = Ligneous,

            PlantRsi = other.PlantRsi,
            PlantIconState = other.PlantIconState,
            CanScream = CanScream,
            TurnIntoKudzu = TurnIntoKudzu,
            SplatPrototype = other.SplatPrototype,

            // Newly cloned seed is unique. No need to unnecessarily clone if repeatedly modified.
            Unique = true,
        };

        // Adding the new chemicals from the new species.
        foreach (var otherChem in other.Chemicals)
        {
            newSeed.Chemicals.TryAdd(otherChem.Key, otherChem.Value);
        }

        // Removing the inherent chemicals from the old species. Leaving mutated/crossbread ones intact.
        foreach (var originalChem in newSeed.Chemicals)
        {
            if (!other.Chemicals.ContainsKey(originalChem.Key) && originalChem.Value.Inherent)
            {
                newSeed.Chemicals.Remove(originalChem.Key);
            }
        }

        return newSeed;
    }
}