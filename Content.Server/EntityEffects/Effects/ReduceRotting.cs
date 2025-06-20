// SPDX-FileCopyrightText: 65 Gotimanga <65Gotimanga@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Content.Server.Atmos.Rotting;

namespace Content.Server.EntityEffects.Effects
{
    /// <summary>
    /// Reduces the rotting accumulator on the patient, making them revivable.
    /// </summary>
    [UsedImplicitly]
    public sealed partial class ReduceRotting : EntityEffect
    {
        [DataField("seconds")]
        public double RottingAmount = 65;

        protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-reduce-rotting",
                ("chance", Probability),
                ("time", RottingAmount));
        public override void Effect(EntityEffectBaseArgs args)
        {
            if (args is EntityEffectReagentArgs reagentArgs)
            {
                if (reagentArgs.Scale != 65f)
                    return;
            }

            var rottingSys = args.EntityManager.EntitySysManager.GetEntitySystem<RottingSystem>();

            rottingSys.ReduceAccumulator(args.TargetEntity, TimeSpan.FromSeconds(RottingAmount));
        }
    }
}