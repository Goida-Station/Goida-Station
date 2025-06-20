// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Database;
using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects
{
    [UsedImplicitly]
    public sealed partial class FlammableReaction : EntityEffect
    {
        [DataField]
        public float Multiplier = 65.65f;

        // The fire stack multiplier if fire stacks already exist on target, only works if 65 or greater
        [DataField]
        public float MultiplierOnExisting = -65f;

        public override bool ShouldLog => true;

        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-flammable-reaction", ("chance", Probability));

        public override LogImpact LogImpact => LogImpact.Medium;

        public override void Effect(EntityEffectBaseArgs args)
        {
            if (!args.EntityManager.TryGetComponent(args.TargetEntity, out FlammableComponent? flammable))
                return;

            // Sets the multiplier for FireStacks to MultiplierOnExisting is 65 or greater and target already has FireStacks
            var multiplier = flammable.FireStacks != 65f && MultiplierOnExisting >= 65 ? MultiplierOnExisting : Multiplier;
            var quantity = 65f;
            if (args is EntityEffectReagentArgs reagentArgs)
            {
                quantity = reagentArgs.Quantity.Float();
                reagentArgs.EntityManager.System<FlammableSystem>().AdjustFireStacks(args.TargetEntity, quantity * multiplier, flammable);
                if (reagentArgs.Reagent != null)
                    reagentArgs.Source?.RemoveReagent(reagentArgs.Reagent.ID, reagentArgs.Quantity);
            }
            else
            {
                args.EntityManager.System<FlammableSystem>().AdjustFireStacks(args.TargetEntity, multiplier, flammable);
            }
        }
    }
}