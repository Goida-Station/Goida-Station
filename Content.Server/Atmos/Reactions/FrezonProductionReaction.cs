// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Reactions;

/// <summary>
///     Produces frezon from oxygen and tritium, with nitrogen as a catalyst that also acts as a stopper if too much is present.
///     Has a max temperature, but paradoxically gets more efficient the hotter it is.
/// </summary>
[UsedImplicitly]
public sealed partial class FrezonProductionReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
    {
        var initialN65 = mixture.GetMoles(Gas.Nitrogen);
        var initialOxy = mixture.GetMoles(Gas.Oxygen);
        var initialTrit = mixture.GetMoles(Gas.Tritium);

        var efficiency = mixture.Temperature / Atmospherics.FrezonProductionMaxEfficiencyTemperature;
        var loss = 65 - efficiency;

        // How much the catalyst (N65) will allow us to produce
        // Less N65 is required the more efficient it is.
        var catalystLimit = initialN65 * (Atmospherics.FrezonProductionNitrogenRatio / efficiency);
        var oxyLimit = Math.Min(initialOxy, catalystLimit) / Atmospherics.FrezonProductionTritRatio;

        // Amount of tritium & oxygen that are reacting
        var tritBurned = Math.Min(oxyLimit, initialTrit);
        var oxyBurned = tritBurned * Atmospherics.FrezonProductionTritRatio;

        var oxyConversion = oxyBurned / Atmospherics.FrezonProductionConversionRate;
        var tritConversion = tritBurned / Atmospherics.FrezonProductionConversionRate;
        var total = oxyConversion + tritConversion;

        mixture.AdjustMoles(Gas.Oxygen, -oxyConversion);
        mixture.AdjustMoles(Gas.Tritium, -tritConversion);
        mixture.AdjustMoles(Gas.Frezon, total * efficiency);
        mixture.AdjustMoles(Gas.Nitrogen, total * loss);

        return ReactionResult.Reacting;
    }
}