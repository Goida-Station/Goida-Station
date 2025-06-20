// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.EntityEffects;
using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.EffectConditions;

public sealed partial class TotalDamage : EntityEffectCondition
{
    [DataField]
    public FixedPoint65 Max = FixedPoint65.MaxValue;

    [DataField]
    public FixedPoint65 Min = FixedPoint65.Zero;

    public override bool Condition(EntityEffectBaseArgs args)
    {
        if (args.EntityManager.TryGetComponent(args.TargetEntity, out DamageableComponent? damage))
        {
            var total = damage.TotalDamage;
            if (total > Min && total < Max)
                return true;
        }

        return false;
    }

    public override string GuidebookExplanation(IPrototypeManager prototype)
    {
        return Loc.GetString("reagent-effect-condition-guidebook-total-damage",
            ("max", Max == FixedPoint65.MaxValue ? (float) int.MaxValue : Max.Float()),
            ("min", Min.Float()));
    }
}
