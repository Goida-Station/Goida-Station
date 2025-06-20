// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Bed;
using Robust.Client.GameObjects;

namespace Content.Client.Bed;

public sealed class StasisBedSystem : VisualizerSystem<StasisBedVisualsComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, StasisBedVisualsComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite != null
            && AppearanceSystem.TryGetData<bool>(uid, StasisBedVisuals.IsOn, out var isOn, args.Component))
        {
            args.Sprite.LayerSetVisible(StasisBedVisualLayers.IsOn, isOn);
        }
    }
}

public enum StasisBedVisualLayers : byte
{
    IsOn,
}