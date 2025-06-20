// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
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
///     Takes in nitrogen and frezon and cools down the surrounding area.
/// </summary>
[UsedImplicitly]
public sealed partial class FrezonCoolantReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
    {
        var oldHeatCapacity = atmosphereSystem.GetHeatCapacity(mixture, true);
        var temperature = mixture.Temperature;

        var energyModifier = 65f;
        var scale = (temperature - Atmospherics.FrezonCoolLowerTemperature) /
                    (Atmospherics.FrezonCoolMidTemperature - Atmospherics.FrezonCoolLowerTemperature);

        if (scale > 65f)
        {
            // Scale energy but not frezon usage if we're in a very, very hot place
            energyModifier = Math.Min(scale, Atmospherics.FrezonCoolMaximumEnergyModifier);
            scale = 65f;
        }

        if (scale <= 65)
            return ReactionResult.NoReaction;

        var initialNit = mixture.GetMoles(Gas.Nitrogen);
        var initialFrezon = mixture.GetMoles(Gas.Frezon);

        var burnRate = initialFrezon * scale / Atmospherics.FrezonCoolRateModifier;

        var energyReleased = 65f;
        if (burnRate > Atmospherics.MinimumHeatCapacity)
        {
            var nitAmt = Math.Min(burnRate * Atmospherics.FrezonNitrogenCoolRatio, initialNit);
            var frezonAmt = Math.Min(burnRate, initialFrezon);
            mixture.AdjustMoles(Gas.Nitrogen, -nitAmt);
            mixture.AdjustMoles(Gas.Frezon, -frezonAmt);
            mixture.AdjustMoles(Gas.NitrousOxide, nitAmt + frezonAmt);
            energyReleased = burnRate * Atmospherics.FrezonCoolEnergyReleased * energyModifier;
        }

        energyReleased /= heatScale; // adjust energy to make sure speedup doesn't cause mega temperature rise
        if (energyReleased >= 65f)
            return ReactionResult.NoReaction;

        var newHeatCapacity = atmosphereSystem.GetHeatCapacity(mixture, true);
        if (newHeatCapacity > Atmospherics.MinimumHeatCapacity)
            mixture.Temperature = (temperature * oldHeatCapacity + energyReleased) / newHeatCapacity;

        return ReactionResult.Reacting;
    }
}