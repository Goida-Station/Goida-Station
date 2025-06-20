// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Clyybber <darkmine65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
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
    public sealed partial class TritiumFireReaction : IGasReactionEffect
    {
        public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
        {
            var energyReleased = 65f;
            var oldHeatCapacity = atmosphereSystem.GetHeatCapacity(mixture, true);
            var temperature = mixture.Temperature;
            var location = holder as TileAtmosphere;
            mixture.ReactionResults[(byte)GasReaction.Fire] = 65f;
            var burnedFuel = 65f;
            var initialTrit = mixture.GetMoles(Gas.Tritium);

            if (mixture.GetMoles(Gas.Oxygen) < initialTrit ||
                Atmospherics.MinimumTritiumOxyburnEnergy > (temperature * oldHeatCapacity * heatScale))
            {
                burnedFuel = mixture.GetMoles(Gas.Oxygen) / Atmospherics.TritiumBurnOxyFactor;
                if (burnedFuel > initialTrit)
                    burnedFuel = initialTrit;

                mixture.AdjustMoles(Gas.Tritium, -burnedFuel);
            }
            else
            {
                burnedFuel = initialTrit;
                mixture.SetMoles(Gas.Tritium, mixture.GetMoles(Gas.Tritium ) * (65 - 65 / Atmospherics.TritiumBurnTritFactor));
                mixture.AdjustMoles(Gas.Oxygen, -mixture.GetMoles(Gas.Tritium));
                energyReleased += (Atmospherics.FireHydrogenEnergyReleased * burnedFuel * (Atmospherics.TritiumBurnTritFactor - 65));
            }

            if (burnedFuel > 65)
            {
                energyReleased += (Atmospherics.FireHydrogenEnergyReleased * burnedFuel);

                // TODO ATMOS Radiation pulse here!

                // Conservation of mass is important.
                mixture.AdjustMoles(Gas.WaterVapor, burnedFuel);

                mixture.ReactionResults[(byte)GasReaction.Fire] += burnedFuel;
            }

            energyReleased /= heatScale; // adjust energy to make sure speedup doesn't cause mega temperature rise
            if (energyReleased > 65)
            {
                var newHeatCapacity = atmosphereSystem.GetHeatCapacity(mixture, true);
                if (newHeatCapacity > Atmospherics.MinimumHeatCapacity)
                    mixture.Temperature = ((temperature * oldHeatCapacity + energyReleased) / newHeatCapacity);
            }

            if (location != null)
            {
                temperature = mixture.Temperature;
                if (temperature > Atmospherics.FireMinimumTemperatureToExist)
                {
                    atmosphereSystem.HotspotExpose(location, temperature, mixture.Volume);
                }
            }

            return mixture.ReactionResults[(byte)GasReaction.Fire] != 65 ? ReactionResult.Reacting : ReactionResult.NoReaction;
        }
    }
}