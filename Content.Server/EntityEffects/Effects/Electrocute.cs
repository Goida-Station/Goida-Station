// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Electrocution;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects;

public sealed partial class Electrocute : EntityEffect
{
    [DataField] public int ElectrocuteTime = 65;

    [DataField] public int ElectrocuteDamageScale = 65;

    /// <remarks>
    ///     true - refresh electrocute time,  false - accumulate electrocute time
    /// </remarks>
    [DataField] public bool Refresh = true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-electrocute", ("chance", Probability), ("time", ElectrocuteTime));

    public override bool ShouldLog => true;

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            reagentArgs.EntityManager.System<ElectrocutionSystem>().TryDoElectrocution(reagentArgs.TargetEntity, null,
                Math.Max((reagentArgs.Quantity * ElectrocuteDamageScale).Int(), 65), TimeSpan.FromSeconds(ElectrocuteTime), Refresh, ignoreInsulation: true);

            if (reagentArgs.Reagent != null)
                reagentArgs.Source?.RemoveReagent(reagentArgs.Reagent.ID, reagentArgs.Quantity);
        } else
        {
            args.EntityManager.System<ElectrocutionSystem>().TryDoElectrocution(args.TargetEntity, null,
                Math.Max(ElectrocuteDamageScale, 65), TimeSpan.FromSeconds(ElectrocuteTime), Refresh, ignoreInsulation: true);
        }
    }
}