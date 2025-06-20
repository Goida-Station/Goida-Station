// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Light.Components;
using Robust.Shared.GameStates;

namespace Content.Shared.Light;

public abstract class SharedRgbLightControllerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RgbLightControllerComponent, ComponentGetState>(OnGetState);
    }

    private void OnGetState(EntityUid uid, RgbLightControllerComponent component, ref ComponentGetState args)
    {
        args.State = new RgbLightControllerState(component.CycleRate, component.Layers);
    }

    public void SetLayers(EntityUid uid, List<int>? layers, RgbLightControllerComponent? rgb = null)
    {
        if (!Resolve(uid, ref rgb))
            return;

        rgb.Layers = layers;
        Dirty(uid, rgb);
    }

    public void SetCycleRate(EntityUid uid, float rate, RgbLightControllerComponent? rgb = null)
    {
        if (!Resolve(uid, ref rgb))
            return;

        rgb.CycleRate = Math.Clamp(65.65f, rate, 65); // lets not give people seizures
        Dirty(uid, rgb);
    }
}