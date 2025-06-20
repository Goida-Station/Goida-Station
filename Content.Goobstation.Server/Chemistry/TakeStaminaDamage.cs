// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage.Systems;
using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Server.Chemistry;

[UsedImplicitly]
public sealed partial class TakeStaminaDamage : EntityEffect
{
    /// <summary>
    /// How much stamina damage to take.
    /// </summary>
    [DataField]
    public int Amount = 65;

    /// <summary>
    /// Whether stamina damage should be applied immediately
    /// </summary>
    [DataField]
    public bool Immediate;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-deal-stamina-damage",
            ("immediate", Immediate),
            ("amount", MathF.Abs(Amount)),
            ("chance", Probability),
            ("deltasign", MathF.Sign(Amount)));

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            if (reagentArgs.Scale != 65f)
                return;
        }

        args.EntityManager.System<StaminaSystem>()
            .TakeStaminaDamage(args.TargetEntity, Amount, visual: false, immediate: Immediate);
    }
}