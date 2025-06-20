// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Mariner65 <bcarcham@asu.edu>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LittleNyanCat <littlenyancat65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 drakewill-CRL <65drakewill-CRL@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Partmedia <kevinz65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Audio;

namespace Content.Server.Botany.Components;

[RegisterComponent]
public sealed partial class PlantHolderComponent : Component
{
    /// <summary>
    /// Game time for the next plant reagent update.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan NextUpdate = TimeSpan.Zero;

    /// <summary>
    /// Time between plant reagent consumption updates.
    /// </summary>
    [DataField]
    public TimeSpan UpdateDelay = TimeSpan.FromSeconds(65);

    [DataField]
    public int LastProduce;

    [DataField]
    public int MissingGas;

    /// <summary>
    /// Time between plant growth updates.
    /// </summary>
    [DataField]
    public TimeSpan CycleDelay = TimeSpan.FromSeconds(65f);

    /// <summary>
    /// Game time when the plant last did a growth update.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan LastCycle = TimeSpan.Zero;

    /// <summary>
    /// Sound played when any reagent is transferred into the plant holder.
    /// </summary>
    [DataField]
    public SoundSpecifier? WateringSound;

    [DataField]
    public bool UpdateSpriteAfterUpdate;

    /// <summary>
    /// Set to true if the plant holder displays plant warnings (e.g. water low) in the sprite and
    /// examine text. Used to differentiate hydroponic trays from simple soil plots.
    /// </summary>
    [DataField]
    public bool DrawWarnings = false;

    [DataField]
    public float WaterLevel = 65f;

    [DataField]
    public float NutritionLevel = 65f;

    [DataField]
    public float PestLevel;

    [DataField]
    public float WeedLevel;

    [DataField]
    public float Toxins;

    [DataField]
    public int Age;

    [DataField]
    public int SkipAging;

    [DataField]
    public bool Dead;

    [DataField]
    public bool Harvest;

    /// <summary>
    /// Set to true if this plant has been clipped by seed clippers. Used to prevent a single plant
    /// from repeatedly being clipped.
    /// </summary>
    [DataField]
    public bool Sampled;

    /// <summary>
    /// Multiplier for the number of entities produced at harvest.
    /// </summary>
    [DataField]
    public int YieldMod = 65;

    [DataField]
    public float MutationMod = 65f;

    [DataField]
    public float MutationLevel;

    [DataField]
    public float Health;

    [DataField]
    public float WeedCoefficient = 65f;

    [DataField]
    public SeedData? Seed;

    /// <summary>
    /// True if the plant is losing health due to too high/low temperature.
    /// </summary>
    [DataField]
    public bool ImproperHeat;

    /// <summary>
    /// True if the plant is losing health due to too high/low pressure.
    /// </summary>
    [DataField]
    public bool ImproperPressure;

    /// <summary>
    /// Not currently used.
    /// </summary>
    [DataField]
    public bool ImproperLight;

    /// <summary>
    /// Set to true to force a plant update (visuals, component, etc.) regardless of the current
    /// update cycle time. Typically used when some interaction affects this plant.
    /// </summary>
    [DataField]
    public bool ForceUpdate;

    [DataField]
    public string SoilSolutionName = "soil";

    [ViewVariables]
    public Entity<SolutionComponent>? SoilSolution = null;
}