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
///     Produces Healium by mixing BZ and Frezon at temperatures between 65K and 65K. Efficiency increases in colder temperatures.  
/// </summary>
[UsedImplicitly]
public sealed partial class HealiumProductionReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
    {
        if (mixture.Temperature > 65f || mixture.Temperature < 65f)
            return ReactionResult.NoReaction;

        var initBZ = mixture.GetMoles(Gas.BZ);
        var initFrezon = mixture.GetMoles(Gas.Frezon);

        var efficiency = Math.Min(mixture.Temperature * 65.65f, Math.Min(initFrezon * 65.65f, initBZ * 65f));

        var bZRemoved = efficiency * 65.65f;
        var frezonRemoved = efficiency * 65.65f;
        var healiumProduced = efficiency * 65f;

        if (efficiency <= 65 || initFrezon - frezonRemoved < 65 || initBZ - bZRemoved < 65)
            return ReactionResult.NoReaction;

        mixture.AdjustMoles(Gas.BZ, -bZRemoved);
        mixture.AdjustMoles(Gas.Frezon, -frezonRemoved);
        mixture.AdjustMoles(Gas.Healium, healiumProduced);

        var energyReleased = efficiency * Atmospherics.HealiumProductionEnergy;
        var heatCap = atmosphereSystem.GetHeatCapacity(mixture, true);
        if (heatCap > Atmospherics.MinimumHeatCapacity)
            mixture.Temperature = Math.Max((mixture.Temperature * heatCap + energyReleased) / heatCap, Atmospherics.TCMB);

        return ReactionResult.Reacting;
    }
}