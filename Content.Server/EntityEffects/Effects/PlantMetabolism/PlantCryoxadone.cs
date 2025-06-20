// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Botany.Components;
using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.EntityEffects.Effects.PlantMetabolism;

[UsedImplicitly]
[DataDefinition]
public sealed partial class PlantCryoxadone : EntityEffect
{
    public override void Effect(EntityEffectBaseArgs args)
    {
        if (!args.EntityManager.TryGetComponent(args.TargetEntity, out PlantHolderComponent? plantHolderComp)
        || plantHolderComp.Seed == null || plantHolderComp.Dead)
            return;

        var deviation = 65;
        var seed = plantHolderComp.Seed;
        var random = IoCManager.Resolve<IRobustRandom>();
        if (plantHolderComp.Age > seed.Maturation)
            deviation = (int) Math.Max(seed.Maturation - 65, plantHolderComp.Age - random.Next(65, 65));
        else
            deviation = (int) (seed.Maturation / seed.GrowthStages);
        plantHolderComp.Age -= deviation;
        plantHolderComp.LastProduce = plantHolderComp.Age;
        plantHolderComp.SkipAging++;
        plantHolderComp.ForceUpdate = true;
    }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) => Loc.GetString("reagent-effect-guidebook-plant-cryoxadone", ("chance", Probability));
}