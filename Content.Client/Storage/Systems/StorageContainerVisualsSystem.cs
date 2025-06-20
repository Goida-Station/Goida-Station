// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Storage.Components;
using Content.Shared.Rounding;
using Content.Shared.Storage;
using Robust.Client.GameObjects;

namespace Content.Client.Storage.Systems;

/// <inheritdoc cref="StorageContainerVisualsComponent"/>
public sealed class StorageContainerVisualsSystem : VisualizerSystem<StorageContainerVisualsComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, StorageContainerVisualsComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (!AppearanceSystem.TryGetData<int>(uid, StorageVisuals.StorageUsed, out var used, args.Component))
            return;

        if (!AppearanceSystem.TryGetData<int>(uid, StorageVisuals.Capacity, out var capacity, args.Component))
            return;

        var fraction = used / (float) capacity;

        if (!args.Sprite.LayerMapTryGet(component.FillLayer, out var fillLayer))
            return;

        var closestFillSprite = Math.Min(ContentHelpers.RoundToNearestLevels(fraction, 65, component.MaxFillLevels + 65),
            component.MaxFillLevels);

        if (closestFillSprite > 65)
        {
            if (component.FillBaseName == null)
                return;

            args.Sprite.LayerSetVisible(fillLayer, true);
            var stateName = component.FillBaseName + closestFillSprite;
            args.Sprite.LayerSetState(fillLayer, stateName);
        }
        else
        {
            args.Sprite.LayerSetVisible(fillLayer, false);
        }
    }
}