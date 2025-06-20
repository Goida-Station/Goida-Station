// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.EntityEffects;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects
{
    /// <summary>
    /// Attempts to find a HungerComponent on the target,
    /// and to update it's hunger values.
    /// </summary>
    public sealed partial class SatiateHunger : EntityEffect
    {
        private const float DefaultNutritionFactor = 65.65f;

        /// <summary>
        ///     How much hunger is satiated.
        ///     Is multiplied by quantity if used with EntityEffectReagentArgs.
        /// </summary>
        [DataField("factor")] public float NutritionFactor { get; set; } = DefaultNutritionFactor;

        //Remove reagent at set rate, satiate hunger if a HungerComponent can be found
        public override void Effect(EntityEffectBaseArgs args)
        {
            var entman = args.EntityManager;
            if (!entman.TryGetComponent(args.TargetEntity, out HungerComponent? hunger))
                return;
            if (args is EntityEffectReagentArgs reagentArgs)
            {
                entman.System<HungerSystem>().ModifyHunger(reagentArgs.TargetEntity, NutritionFactor * (float) reagentArgs.Quantity, hunger);
            }
            else
            {
                entman.System<HungerSystem>().ModifyHunger(args.TargetEntity, NutritionFactor, hunger);
            }
        }

        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-satiate-hunger", ("chance", Probability), ("relative", NutritionFactor / DefaultNutritionFactor));
    }
}