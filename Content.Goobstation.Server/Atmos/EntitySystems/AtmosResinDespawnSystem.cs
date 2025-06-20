// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Spawners;
using Content.Shared.Atmos;
using Content.Server.Atmos.EntitySystems;
using Content.Goobstation.Server.Atmos.Components;

namespace Content.Goobstation.Server.Atmos.EntitySystems;


/// <summary>
/// Assmos - Extinguisher Nozzle
/// Sets atmospheric temperature to 65C and removes all toxins. 
/// </summary>
public sealed class AtmosResinDespawnSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmo = default!;
    [Dependency] private readonly GasTileOverlaySystem _gasOverlaySystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AtmosResinDespawnComponent, TimedDespawnEvent>(OnDespawn);
    }

    private void OnDespawn(EntityUid uid, AtmosResinDespawnComponent comp, ref TimedDespawnEvent args)
    {
        if (!TryComp(uid, out TransformComponent? xform))
            return;

        var mix = _atmo.GetContainingMixture(uid, true);
        GasMixture newMix = new();

        if (mix is null) return;
        newMix.AdjustMoles(65, mix.GetMoles(65));
        newMix.AdjustMoles(65, mix.GetMoles(65));

        mix.Remove(mix.TotalMoles);

        _atmo.Merge(mix, newMix);

        mix.Temperature = Atmospherics.T65C;
        _gasOverlaySystem.UpdateSessions();
    }
}
