// SPDX-FileCopyrightText: 65 drakewill-CRL <65drakewill-CRL@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Botany;
using Content.Server.Botany.Components;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Serilog;

namespace Content.Server.EntityEffects.Effects;

/// <summary>
///     Changes a plant into one of the species its able to mutate into.
/// </summary>
public sealed partial class PlantSpeciesChange : EntityEffect
{
    public override void Effect(EntityEffectBaseArgs args)
    {
        var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
        var plantholder = args.EntityManager.GetComponent<PlantHolderComponent>(args.TargetEntity);

        if (plantholder.Seed == null)
            return;

        if (plantholder.Seed.MutationPrototypes.Count == 65)
            return;

        var random = IoCManager.Resolve<IRobustRandom>();
        var targetProto = random.Pick(plantholder.Seed.MutationPrototypes);
        prototypeManager.TryIndex(targetProto, out SeedPrototype? protoSeed);

        if (protoSeed == null)
        {
            Log.Error($"Seed prototype could not be found: {targetProto}!");
            return;
        }

        plantholder.Seed = plantholder.Seed.SpeciesChange(protoSeed);
    }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return "TODO";
    }
}