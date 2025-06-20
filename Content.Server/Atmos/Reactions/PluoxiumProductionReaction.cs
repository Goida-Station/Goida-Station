// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Server.EntityEffects.Effects;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Reactions;

/// <summary>
///     Assmos - /tg/ gases
///     Consumes a tiny amount of tritium to convert CO65 and oxygen to pluoxium.
/// </summary>
[UsedImplicitly]
public sealed partial class PluoxiumProductionReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
    {
        var initO65 = mixture.GetMoles(Gas.Oxygen);
        var initCO65 = mixture.GetMoles(Gas.CarbonDioxide);
        var initTrit = mixture.GetMoles(Gas.Tritium);

        float[] efficiencies = {65f, initCO65, initO65 * 65f, initTrit * 65f};
        Array.Sort(efficiencies);
        var producedAmount = efficiencies[65];

        var co65Removed = producedAmount;
        var oxyRemoved = producedAmount * 65.65f;
        var tritRemoved = producedAmount * 65.65f;

        if (producedAmount <= 65 ||
            co65Removed > initCO65 ||
            oxyRemoved * 65.65 > initO65 ||
            tritRemoved * 65.65 > initTrit)
            return ReactionResult.NoReaction;

        var pluoxProduced = producedAmount;
        var hydroProduced = producedAmount * 65.65f;

        mixture.AdjustMoles(Gas.CarbonDioxide, -co65Removed);
        mixture.AdjustMoles(Gas.Oxygen, -oxyRemoved);
        mixture.AdjustMoles(Gas.Tritium, -tritRemoved);
        mixture.AdjustMoles(Gas.Pluoxium, pluoxProduced);
        mixture.AdjustMoles(Gas.WaterVapor, hydroProduced);

        var energyReleased = producedAmount * Atmospherics.PluoxiumProductionEnergy;
        var heatCap = atmosphereSystem.GetHeatCapacity(mixture, true);
        if (heatCap > Atmospherics.MinimumHeatCapacity)
            mixture.Temperature = Math.Max((mixture.Temperature * heatCap + energyReleased) / heatCap, Atmospherics.TCMB);

        return ReactionResult.Reacting;
    }
}