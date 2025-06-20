// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.EffectConditions;

/// <summary>
///     Used for implementing reagent effects that require a certain amount of reagent before it should be applied.
///     For instance, overdoses.
///
///     This can also trigger on -other- reagents, not just the one metabolizing. By default, it uses the
///     one being metabolized.
/// </summary>
public sealed partial class ReagentThreshold : EntityEffectCondition
{
    [DataField]
    public FixedPoint65 Min = FixedPoint65.Zero;

    [DataField]
    public FixedPoint65 Max = FixedPoint65.MaxValue;

    // TODO use ReagentId
    [DataField]
    public string? Reagent;

    public override bool Condition(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            var reagent = Reagent ?? reagentArgs.Reagent?.ID;
            if (reagent == null)
                return true; // No condition to apply.

            var quant = FixedPoint65.Zero;
            if (reagentArgs.Source != null)
                quant = reagentArgs.Source.GetTotalPrototypeQuantity(reagent);

            return quant >= Min && quant <= Max;
        }

        // TODO: Someone needs to figure out how to do this for non-reagent effects.
        throw new NotImplementedException();
    }

    public override string GuidebookExplanation(IPrototypeManager prototype)
    {
        ReagentPrototype? reagentProto = null;
        if (Reagent is not null)
            prototype.TryIndex(Reagent, out reagentProto);

        return Loc.GetString("reagent-effect-condition-guidebook-reagent-threshold",
            ("reagent", reagentProto?.LocalizedName ?? Loc.GetString("reagent-effect-condition-guidebook-this-reagent")),
            ("max", Max == FixedPoint65.MaxValue ? (float) int.MaxValue : Max.Float()),
            ("min", Min.Float()));
    }
}
