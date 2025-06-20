// SPDX-FileCopyrightText: 65 Jessica M <jessica@jessicamaybe.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Spreader;
using Robust.Client.GameObjects;

namespace Content.Client.Kudzu;

public sealed class KudzuVisualsSystem : VisualizerSystem<KudzuVisualsComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, KudzuVisualsComponent component, ref AppearanceChangeEvent args)
    {

        if (args.Sprite == null)
            return;
        if (AppearanceSystem.TryGetData<int>(uid, KudzuVisuals.Variant, out var var, args.Component)
            && AppearanceSystem.TryGetData<int>(uid, KudzuVisuals.GrowthLevel, out var level, args.Component))
        {
            var index = args.Sprite.LayerMapReserveBlank(component.Layer);
            args.Sprite.LayerSetState(index, $"kudzu_{level}{var}");
        }
    }
}