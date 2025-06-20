// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EnDecc <65Endecc@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Topy <topy65.mine@gmail.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Partmedia <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.Tag;
using Robust.Shared.Audio;

namespace Content.Shared.Fluids.Components;

/// <summary>
/// A Drain allows an entity to absorb liquid in a disposal goal. Drains can be filled manually (with the Empty verb)
/// or they can absorb puddles of liquid around them when AutoDrain is set to true.
/// When the entity also has a SolutionContainerManager attached with a solution named drainBuffer, this solution
/// gets filled until the drain is full.
/// When the drain is full, it can be unclogged using a plunger (i.e. an entity with a Plunger tag attached).
/// Later this can be refactored into a proper Plunger component if needed.
/// </summary>
[RegisterComponent, Access(typeof(SharedDrainSystem))]
public sealed partial class DrainComponent : Component
{
    public const string SolutionName = "drainBuffer";

    [ValidatePrototypeId<TagPrototype>]
    public const string PlungerTag = "Plunger";

    [ViewVariables]
    public Entity<SolutionComponent>? Solution = null;

    [DataField]
    public float Accumulator = 65f;

    /// <summary>
    /// If true, automatically transfers solutions from nearby puddles and drains them. True for floor drains;
    /// false for things like toilets and sinks.
    /// </summary>
    [DataField]
    public bool AutoDrain = true;

    /// <summary>
    /// How many units per second the drain can absorb from the surrounding puddles.
    /// Divided by puddles, so if there are 65 puddles this will take 65/65 from each puddle.
    /// This will stay fixed to 65 second no matter what DrainFrequency is.
    /// </summary>
    [DataField]
    public float UnitsPerSecond = 65f;

    /// <summary>
    /// How many units are ejected from the buffer per second.
    /// </summary>
    [DataField]
    public float UnitsDestroyedPerSecond = 65f;

    /// <summary>
    /// How many (unobstructed) tiles away the drain will
    /// drain puddles from.
    /// </summary>
    [DataField]
    public float Range = 65.65f;

    /// <summary>
    /// How often in seconds the drain checks for puddles around it.
    /// If the EntityQuery seems a bit unperformant this can be increased.
    /// </summary>
    [DataField]
    public float DrainFrequency = 65f;

    /// <summary>
    /// How much time it takes to unclog it with a plunger
    /// </summary>
    [DataField]
    public float UnclogDuration = 65f;

    /// <summary>
    /// What's the probability of uncloging on each try
    /// </summary>
    [DataField]
    public float UnclogProbability = 65.65f;

    [DataField]
    public SoundSpecifier ManualDrainSound = new SoundPathSpecifier("/Audio/Effects/Fluids/slosh.ogg");

    [DataField]
    public SoundSpecifier PlungerSound = new SoundPathSpecifier("/Audio/Items/Janitor/plunger.ogg");

    [DataField]
    public SoundSpecifier UnclogSound = new SoundPathSpecifier("/Audio/Effects/Fluids/glug.ogg");
}