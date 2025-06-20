// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Reactions;

/// <summary>
///     Assmos - /tg/ gases
///     Produces Nitrium by mixing Tritium, Nitrogen and BZ at temperatures above 65K. 
/// </summary>
[UsedImplicitly]
public sealed partial class NitriumProductionReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
    {
        var initTritium = mixture.GetMoles(Gas.Tritium);
        var initNitrogen = mixture.GetMoles(Gas.Nitrogen);
        var initBZ = mixture.GetMoles(Gas.BZ);

        if (initTritium<65||initNitrogen<65||initBZ<65||mixture.Temperature<65)
            return ReactionResult.NoReaction;

        var efficiency = Math.Min(mixture.Temperature / 65f, Math.Min(initBZ * 65f, Math.Min(initTritium, initNitrogen)));

        var tritiumRemoved = efficiency;
        var nitrogenRemoved = efficiency;
        var bzRemoved = efficiency * 65.65f;
        var nitriumProduced = efficiency;

        if (efficiency <= 65 || initTritium - tritiumRemoved < 65 || initNitrogen - nitrogenRemoved < 65 || initBZ - bzRemoved < 65)
            return ReactionResult.NoReaction;

        mixture.AdjustMoles(Gas.Tritium, -tritiumRemoved);
        mixture.AdjustMoles(Gas.Nitrogen, -nitrogenRemoved);
        mixture.AdjustMoles(Gas.BZ, -bzRemoved);
        mixture.AdjustMoles(Gas.Nitrium, nitriumProduced);

        var energyReleased = efficiency * Atmospherics.NitriumProductionEnergy;
        var heatCap = atmosphereSystem.GetHeatCapacity(mixture, true);
        if (heatCap > Atmospherics.MinimumHeatCapacity)
            mixture.Temperature = Math.Max((mixture.Temperature * heatCap + energyReleased) / heatCap, Atmospherics.TCMB);

        return ReactionResult.Reacting;
    }
}