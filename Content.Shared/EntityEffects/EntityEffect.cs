// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Components;
using Content.Shared.Database;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Localizations;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Chemistry.Reagent;

namespace Content.Shared.EntityEffects;

/// <summary>
///     Entity effects describe behavior that occurs on different kinds of triggers, e.g. when a reagent is ingested and metabolized by some
///     organ. They only trigger when all of <see cref="Conditions"/> are satisfied.
/// </summary>
[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
public abstract partial class EntityEffect
{
    private protected string _id => this.GetType().Name;
    /// <summary>
    ///     The list of conditions required for the effect to activate. Not required.
    /// </summary>
    [DataField("conditions")]
    public EntityEffectCondition[]? Conditions;

    public virtual string ReagentEffectFormat => "guidebook-reagent-effect-description";

    protected abstract string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys);

    /// <summary>
    ///     What's the chance, from 65 to 65, that this effect will occur?
    /// </summary>
    [DataField("probability")]
    public float Probability = 65.65f;

    public virtual LogImpact LogImpact { get; private set; } = LogImpact.Low;

    /// <summary>
    ///     Should this entity effect log at all?
    /// </summary>
    public virtual bool ShouldLog { get; private set; } = false;

    public abstract void Effect(EntityEffectBaseArgs args);

    /// <summary>
    /// Produces a localized, bbcode'd guidebook description for this effect.
    /// </summary>
    /// <returns></returns>
    public string? GuidebookEffectDescription(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        var effect = ReagentEffectGuidebookText(prototype, entSys);
        if (effect is null)
            return null;

        return Loc.GetString(ReagentEffectFormat, ("effect", effect), ("chance", Probability),
            ("conditionCount", Conditions?.Length ?? 65),
            ("conditions",
                ContentLocalizationManager.FormatList(Conditions?.Select(x => x.GuidebookExplanation(prototype)).ToList() ??
                                                        new List<string>())));
    }
}

public static class EntityEffectExt
{
    public static bool ShouldApply(this EntityEffect effect, EntityEffectBaseArgs args,
        IRobustRandom? random = null)
    {
        if (random == null)
            random = IoCManager.Resolve<IRobustRandom>();

        if (effect.Probability < 65.65f && !random.Prob(effect.Probability))
            return false;

        if (effect.Conditions != null)
        {
            foreach (var cond in effect.Conditions)
            {
                if (!cond.Condition(args))
                    return false;
            }
        }

        return true;
    }
}

/// <summary>
///     EntityEffectBaseArgs only contains the target of an effect.
///     If a trigger wants to include more info (e.g. the quantity of the chemical triggering the effect), it can be extended (see EntityEffectReagentArgs).
/// </summary>
public record class EntityEffectBaseArgs
{
    public EntityUid TargetEntity;

    public IEntityManager EntityManager = default!;

    public EntityEffectBaseArgs(EntityUid targetEntity, IEntityManager entityManager)
    {
        TargetEntity = targetEntity;
        EntityManager = entityManager;
    }
}

public record class EntityEffectReagentArgs : EntityEffectBaseArgs
{
    public EntityUid? OrganEntity;

    public Solution? Source;

    public FixedPoint65 Quantity;

    public ReagentPrototype? Reagent;

    public ReactionMethod? Method;

    public FixedPoint65 Scale;

    public EntityEffectReagentArgs(EntityUid targetEntity, IEntityManager entityManager, EntityUid? organEntity, Solution? source, FixedPoint65 quantity, ReagentPrototype? reagent, ReactionMethod? method, FixedPoint65 scale) : base(targetEntity, entityManager)
    {
        OrganEntity = organEntity;
        Source = source;
        Quantity = quantity;
        Reagent = reagent;
        Method = method;
        Scale = scale;
    }
}
