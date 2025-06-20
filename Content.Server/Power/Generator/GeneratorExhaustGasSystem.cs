// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Power.Generator;

namespace Content.Server.Power.Generator;

/// <seealso cref="GeneratorSystem"/>
/// <seealso cref="GeneratorExhaustGasComponent"/>
public sealed class GeneratorExhaustGasSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphere = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<GeneratorExhaustGasComponent, GeneratorUseFuel>(FuelUsed);
    }

    private void FuelUsed(EntityUid uid, GeneratorExhaustGasComponent component, GeneratorUseFuel args)
    {
        var exhaustMixture = new GasMixture();
        exhaustMixture.SetMoles(component.GasType, args.FuelUsed * component.MoleRatio);
        exhaustMixture.Temperature = component.Temperature;

        var environment = _atmosphere.GetContainingMixture(uid, false, true);
        if (environment != null)
            _atmosphere.Merge(environment, exhaustMixture);
    }
}