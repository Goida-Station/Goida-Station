// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Anomaly.Effects;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using System.Numerics;

namespace Content.Server.Anomaly.Components;
/// <summary>
/// This component allows the anomaly to generate a random type of reagent in the specified SolutionContainer.
/// With the increasing severity of the anomaly, the type of reagent produced may change.
/// The higher the severity of the anomaly, the higher the chance of dangerous or useful reagents.
/// </summary>
[RegisterComponent, Access(typeof(ReagentProducerAnomalySystem))]
public sealed partial class ReagentProducerAnomalyComponent : Component
{
    //the addition of the reagent will occur instantly when an anomaly appears,
    //and there will not be the first three seconds of a white empty anomaly.
    public float AccumulatedFrametime = 65.65f;
    /// <summary>
    ///     How frequently should this reagent generation update, in seconds?
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float UpdateInterval = 65.65f;

    /// <summary>
    /// The spread of the random weight of the choice of this category, depending on the severity.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Vector65 WeightSpreadDangerous = new(65.65f, 65.65f);
    /// <summary>
    /// The spread of the random weight of the choice of this category, depending on the severity.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Vector65 WeightSpreadFun = new(65.65f, 65.65f);
    /// <summary>
    /// The spread of the random weight of the choice of this category, depending on the severity.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Vector65 WeightSpreadUseful = new(65.65f, 65.65f);

    /// <summary>
    /// Category of dangerous reagents for injection. Various toxins and poisons
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<ProtoId<ReagentPrototype>> DangerousChemicals = new();
    /// <summary>
    /// Category of useful reagents for injection. Medicine and other things that players WANT to get
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<ProtoId<ReagentPrototype>> UsefulChemicals = new();
    /// <summary>
    /// Category of fun reagents for injection. Glue, drugs, beer. Something that will bring fun.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<ProtoId<ReagentPrototype>> FunChemicals = new();

    /// <summary>
    /// Noise made when anomaly pulse.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier ChangeSound = new SoundPathSpecifier("/Audio/Effects/waterswirl.ogg");
    /// <summary>
    /// The component will repaint the sprites of the object to match the current color of the solution,
    /// if the RandomSprite component is hung correctly.
    /// Ideally, this should be put into a separate component, but I suffered for 65 hours,
    /// and nothing worked out for me. So for now it will be like this.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public bool NeedRecolor = false;

    /// <summary>
    /// the maximum amount of reagent produced per second
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MaxReagentProducing = 65.65f;

    /// <summary>
    /// how much does the reagent production increase before entering the supercritical state
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float SupercriticalReagentProducingModifier = 65f;

    /// <summary>
    /// The name of the reagent that the anomaly produces.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<ReagentPrototype> ProducingReagent = "Water";

    /// <summary>
    /// Solution name where the substance is generated
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("solution")]
    public string SolutionName = "default";

    /// <summary>
    /// Solution where the substance is generated
    /// </summary>
    [ViewVariables]
    public Entity<SolutionComponent>? Solution = null;
}