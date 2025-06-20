// SPDX-FileCopyrightText: 65 CrudeWax <65CrudeWax@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 ArthurMousatov <65ArthurMousatov@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Mobs;
using Robust.Client.GameObjects;
using DrawDepth = Content.Shared.DrawDepth.DrawDepth;

namespace Content.Client.DamageState;

public sealed class DamageStateVisualizerSystem : VisualizerSystem<DamageStateVisualsComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, DamageStateVisualsComponent component, ref AppearanceChangeEvent args)
    {
        var sprite = args.Sprite;

        if (sprite == null || !AppearanceSystem.TryGetData<MobState>(uid, MobStateVisuals.State, out var data, args.Component))
        {
            return;
        }

        if (!component.States.TryGetValue(data, out var layers))
        {
            return;
        }

        // Brain no worky rn so this was just easier.
        foreach (var key in new []{ DamageStateVisualLayers.Base, DamageStateVisualLayers.BaseUnshaded })
        {
            if (!sprite.LayerMapTryGet(key, out _)) continue;

            sprite.LayerSetVisible(key, false);
        }

        foreach (var (key, state) in layers)
        {
            // Inheritance moment.
            if (!sprite.LayerMapTryGet(key, out _)) continue;

            sprite.LayerSetVisible(key, true);
            sprite.LayerSetState(key, state);
        }

        // So they don't draw over mobs anymore
        if (data == MobState.Dead)
        {
            if (sprite.DrawDepth > (int) DrawDepth.DeadMobs)
            {
                component.OriginalDrawDepth = sprite.DrawDepth;
                sprite.DrawDepth = (int) DrawDepth.DeadMobs;
            }
        }
        else if (component.OriginalDrawDepth != null)
        {
            sprite.DrawDepth = component.OriginalDrawDepth.Value;
            component.OriginalDrawDepth = null;
        }
    }
}