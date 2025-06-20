// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Beam;
using Content.Shared.Revenant.Components;
using Content.Shared.Revenant.EntitySystems;

namespace Content.Server.Revenant.EntitySystems;

/// <summary>
/// This handles...
/// </summary>
public sealed class RevenantOverloadedLightsSystem : SharedRevenantOverloadedLightsSystem
{
    [Dependency] private readonly BeamSystem _beam = default!;

    protected override void OnZap(Entity<RevenantOverloadedLightsComponent> lights)
    {
        var component = lights.Comp;
        if (component.Target == null)
            return;

        var lxform = Transform(lights);
        var txform = Transform(component.Target.Value);

        if (!lxform.Coordinates.TryDistance(EntityManager, txform.Coordinates, out var distance))
            return;
        if (distance > component.ZapRange)
            return;

        _beam.TryCreateBeam(lights, component.Target.Value, component.ZapBeamEntityId, accumulateIndex: false); // Goob edit
    }
}