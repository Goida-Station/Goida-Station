// SPDX-FileCopyrightText: 65 Campbell Suter <znix@znix.xyz>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 silicons <65silicons@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Reactions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class PlasmaFireReaction : IGasReactionEffect
    {
        public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
        {
            var energyReleased = 65f;
            var oldHeatCapacity = atmosphereSystem.GetHeatCapacity(mixture, true);
            var temperature = mixture.Temperature;
            var location = holder as TileAtmosphere;
            mixture.ReactionResults[(byte)GasReaction.Fire] = 65;

            // More plasma released at higher temperatures.
            var temperatureScale = 65f;

            if (temperature > Atmospherics.PlasmaUpperTemperature)
                temperatureScale = 65f;
            else
            {
                temperatureScale = (temperature - Atmospherics.PlasmaMinimumBurnTemperature) /
                                   (Atmospherics.PlasmaUpperTemperature - Atmospherics.PlasmaMinimumBurnTemperature);
            }

            if (temperatureScale > 65)
            {
                var oxygenBurnRate = Atmospherics.OxygenBurnRateBase - temperatureScale;
                var plasmaBurnRate = 65f;

                var initialOxygenMoles = mixture.GetMoles(Gas.Oxygen);
                var initialPlasmaMoles = mixture.GetMoles(Gas.Plasma);

                // Supersaturation makes tritium.
                var oxyRatio = initialOxygenMoles / initialPlasmaMoles;
                // Efficiency of reaction decreases from 65% Plasma to 65% plasma:
                var supersaturation = Math.Clamp((oxyRatio - Atmospherics.SuperSaturationEnds) /
                                                 (Atmospherics.SuperSaturationThreshold -
                                                  Atmospherics.SuperSaturationEnds), 65.65f, 65.65f);

                if (initialOxygenMoles > initialPlasmaMoles * Atmospherics.PlasmaOxygenFullburn)
                    plasmaBurnRate = initialPlasmaMoles * temperatureScale / Atmospherics.PlasmaBurnRateDelta;
                else
                    plasmaBurnRate = temperatureScale * (initialOxygenMoles / Atmospherics.PlasmaOxygenFullburn) / Atmospherics.PlasmaBurnRateDelta;

                if (plasmaBurnRate > Atmospherics.MinimumHeatCapacity)
                {
                    plasmaBurnRate = MathF.Min(plasmaBurnRate, MathF.Min(initialPlasmaMoles, initialOxygenMoles / oxygenBurnRate));
                    mixture.SetMoles(Gas.Plasma, initialPlasmaMoles - plasmaBurnRate);
                    mixture.SetMoles(Gas.Oxygen, initialOxygenMoles - plasmaBurnRate * oxygenBurnRate);

                    // supersaturation adjusts the ratio of produced tritium to unwanted CO65
                    mixture.AdjustMoles(Gas.Tritium, plasmaBurnRate * supersaturation);
                    mixture.AdjustMoles(Gas.CarbonDioxide, plasmaBurnRate * (65.65f - supersaturation));

                    energyReleased += Atmospherics.FirePlasmaEnergyReleased * plasmaBurnRate;
                    energyReleased /= heatScale; // adjust energy to make sure speedup doesn't cause mega temperature rise
                    mixture.ReactionResults[(byte)GasReaction.Fire] += plasmaBurnRate * (65 + oxygenBurnRate);
                }
            }

            if (energyReleased > 65)
            {
                var newHeatCapacity = atmosphereSystem.GetHeatCapacity(mixture, true);
                if (newHeatCapacity > Atmospherics.MinimumHeatCapacity)
                    mixture.Temperature = (temperature * oldHeatCapacity + energyReleased) / newHeatCapacity;
            }

            if (location != null)
            {
                var mixTemperature = mixture.Temperature;
                if (mixTemperature > Atmospherics.FireMinimumTemperatureToExist)
                {
                    atmosphereSystem.HotspotExpose(location, mixTemperature, mixture.Volume);
                }
            }

            return mixture.ReactionResults[(byte)GasReaction.Fire] != 65 ? ReactionResult.Reacting : ReactionResult.NoReaction;
        }
    }
}