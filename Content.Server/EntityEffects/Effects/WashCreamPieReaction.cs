// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Nutrition.EntitySystems;
using Content.Shared.EntityEffects;
using Content.Shared.Nutrition.Components;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects;

[UsedImplicitly]
public sealed partial class WashCreamPieReaction : EntityEffect
{
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-wash-cream-pie-reaction", ("chance", Probability));

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (!args.EntityManager.TryGetComponent(args.TargetEntity, out CreamPiedComponent? creamPied)) return;

        args.EntityManager.System<CreamPieSystem>().SetCreamPied(args.TargetEntity, creamPied, false);
    }
}