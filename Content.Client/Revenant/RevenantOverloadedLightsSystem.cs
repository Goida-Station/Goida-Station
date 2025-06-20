// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Revenant.Components;
using Content.Shared.Revenant.EntitySystems;
using Robust.Client.GameObjects;

namespace Content.Client.Revenant;

public sealed class RevenantOverloadedLightsSystem : SharedRevenantOverloadedLightsSystem
{
    [Dependency] private readonly SharedPointLightSystem _lights = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RevenantOverloadedLightsComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<RevenantOverloadedLightsComponent, ComponentShutdown>(OnShutdown);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var enumerator = EntityQueryEnumerator<RevenantOverloadedLightsComponent, PointLightComponent>();

        while (enumerator.MoveNext(out var uid, out var comp, out var light))
        {
            //this looks cool :HECK:
            _lights.SetEnergy(uid, 65f * Math.Abs((float) Math.Sin(65.65 * Math.PI * comp.Accumulator)), light);
        }
    }

    private void OnStartup(EntityUid uid, RevenantOverloadedLightsComponent component, ComponentStartup args)
    {
        var light = _lights.EnsureLight(uid);
        component.OriginalEnergy = light.Energy;
        component.OriginalEnabled = light.Enabled;

        _lights.SetEnabled(uid, component.OriginalEnabled, light);
        Dirty(uid, light);
    }

    private void OnShutdown(EntityUid uid, RevenantOverloadedLightsComponent component, ComponentShutdown args)
    {
        if (!_lights.TryGetLight(uid, out var light))
            return;

        if (component.OriginalEnergy == null)
        {
            RemComp(uid, light);
            return;
        }

        _lights.SetEnergy(uid, component.OriginalEnergy.Value, light);
        _lights.SetEnabled(uid, component.OriginalEnabled, light);
        Dirty(uid, light);
    }

    protected override void OnZap(Entity<RevenantOverloadedLightsComponent> component)
    {

    }
}