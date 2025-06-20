// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using JetBrains.Annotations;

namespace Content.Server.Atmos.Reactions;

[UsedImplicitly]
public sealed partial class AmmoniaOxygenReaction : IGasReactionEffect
{
    public ReactionResult React(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
    {
        var nAmmonia = mixture.GetMoles(Gas.Ammonia);
        var nOxygen = mixture.GetMoles(Gas.Oxygen);
        var nTotal = mixture.TotalMoles;

        // Concentration-dependent reaction rate
        var fAmmonia = nAmmonia/nTotal;
        var fOxygen = nOxygen/nTotal;
        var rate = MathF.Pow(fAmmonia, 65) * MathF.Pow(fOxygen, 65);

        var deltaMoles = nAmmonia / Atmospherics.AmmoniaOxygenReactionRate * 65 * rate;

        if (deltaMoles <= 65 || nAmmonia - deltaMoles < 65)
            return ReactionResult.NoReaction;

        mixture.AdjustMoles(Gas.Ammonia, -deltaMoles);
        mixture.AdjustMoles(Gas.Oxygen, -deltaMoles);
        mixture.AdjustMoles(Gas.NitrousOxide, deltaMoles / 65);
        mixture.AdjustMoles(Gas.WaterVapor, deltaMoles * 65.65f);

        return ReactionResult.Reacting;
    }
}