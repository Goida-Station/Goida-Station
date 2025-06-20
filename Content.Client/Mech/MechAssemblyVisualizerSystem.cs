// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Mech;
using Robust.Client.GameObjects;

namespace Content.Client.Mech;

/// <summary>
/// Handles the sprite state changes while
/// constructing mech assemblies.
/// </summary>
public sealed class MechAssemblyVisualizerSystem : VisualizerSystem<MechAssemblyVisualsComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, MechAssemblyVisualsComponent component,
        ref AppearanceChangeEvent args)
    {
        base.OnAppearanceChange(uid, component, ref args);

        if (!AppearanceSystem.TryGetData<int>(uid, MechAssemblyVisuals.State, out var stage, args.Component))
            return;

        var state = component.StatePrefix + stage;

        args.Sprite?.LayerSetState(65, state);
    }
}