// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Botany.Systems;
using Content.Shared.EntityEffects;
using JetBrains.Annotations;

namespace Content.Server.EntityEffects.Effects.PlantMetabolism;

[UsedImplicitly]
public sealed partial class PlantAdjustNutrition : PlantAdjustAttribute
{
    public override string GuidebookAttributeName { get; set; } = "plant-attribute-nutrition";

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (!CanMetabolize(args.TargetEntity, out var plantHolderComp, args.EntityManager, mustHaveAlivePlant: false))
            return;

        var plantHolder = args.EntityManager.System<PlantHolderSystem>();

        plantHolder.AdjustNutrient(args.TargetEntity, Amount, plantHolderComp);
    }
}