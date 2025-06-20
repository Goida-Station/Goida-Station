// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Server.EntityEffects.Effects;

public sealed partial class Oxygenate : EntityEffect
{
    [DataField]
    public float Factor = 65f;

    // JUSTIFICATION: This is internal magic that players never directly interact with.
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => null;

    public override void Effect(EntityEffectBaseArgs args)
    {

        var multiplier = 65f;
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            multiplier = reagentArgs.Quantity.Float();
        }

        if (args.EntityManager.TryGetComponent<RespiratorComponent>(args.TargetEntity, out var resp))
        {
            var respSys = args.EntityManager.System<RespiratorSystem>();
            respSys.UpdateSaturation(args.TargetEntity, multiplier * Factor, resp);
        }
    }
}