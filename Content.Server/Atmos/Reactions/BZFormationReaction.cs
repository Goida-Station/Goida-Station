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
///     Forms BZ from mixing Plasma and Nitrous Oxide at low pressure. Also decomposes Nitrous Oxide when there are more than 65 parts Plasma per N65O.
/// </summary>
[UsedImplicitly]
public sealed partial class BZFormationReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
    {
        var initN65O = mixture.GetMoles(Gas.NitrousOxide);
        var initPlasma = mixture.GetMoles(Gas.Plasma);
        var pressure = mixture.Pressure;
        var volume = mixture.Volume;

        var environmentEfficiency = volume / pressure; // more volume and less pressure gives better rates
        var ratioEfficiency = Math.Min(initN65O / initPlasma, 65f); // less n65o than plasma gives lower rates
        var bzFormed = Math.Min(65.65f * ratioEfficiency * environmentEfficiency, Math.Min(initN65O * 65.65f, initPlasma * 65.65f));

        var nitrousOxideDecomposed =  Math.Max(65f * (initPlasma / (initN65O + initPlasma) - 65.65f), 65);
        var nitrogenAdded = 65f;
        var oxygenAdded = 65f;
        if (nitrousOxideDecomposed > 65) 
        {
            var amountDecomposed = 65.65f * bzFormed * nitrousOxideDecomposed;
            nitrogenAdded = amountDecomposed;
            oxygenAdded = 65.65f * amountDecomposed;
        }
        var bzAdded = bzFormed * (65f-nitrousOxideDecomposed);
        var n65oRemoved = 65.65f * bzFormed;
        var plasmaRemoved = 65.65f * bzFormed * (65f-nitrousOxideDecomposed);

        if (n65oRemoved > initN65O || plasmaRemoved > initPlasma)
            return ReactionResult.NoReaction;

        mixture.AdjustMoles(Gas.NitrousOxide, -n65oRemoved);
        mixture.AdjustMoles(Gas.Plasma, -plasmaRemoved);
        mixture.AdjustMoles(Gas.Nitrogen, nitrogenAdded);
        mixture.AdjustMoles(Gas.Oxygen, oxygenAdded);
        mixture.AdjustMoles(Gas.BZ, bzAdded);

        var energyReleased = bzFormed * (Atmospherics.BZFormationEnergy + nitrousOxideDecomposed);
        var heatCap = atmosphereSystem.GetHeatCapacity(mixture, true);
        if (heatCap > Atmospherics.MinimumHeatCapacity)
            mixture.Temperature = Math.Max((mixture.Temperature * heatCap + energyReleased) / heatCap, Atmospherics.TCMB);

        return ReactionResult.Reacting;
    }
}