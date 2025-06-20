// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Client.Clothing;
using Content.Goobstation.Client.Clothing.Components;
using Content.Goobstation.Shared.Clothing;
using Content.Shared.Clothing;
using Content.Shared.Item;
using Robust.Client.GameObjects;

namespace Content.Goobstation.Client.Clothing.EntitySystems;

public sealed class SealableClothingVisualizerSystem : VisualizerSystem<SealableClothingVisualsComponent>
{
    [Dependency] private readonly SharedItemSystem _itemSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SealableClothingVisualsComponent, GetEquipmentVisualsEvent>(OnGetEquipmentVisuals, after: new[] { typeof(ClientClothingSystem) });
    }

    protected override void OnAppearanceChange(EntityUid uid, SealableClothingVisualsComponent component, ref AppearanceChangeEvent args)
    {
        if (!AppearanceSystem.TryGetData<bool>(uid, SealableClothingVisuals.Sealed, out var isSealed, args.Component))
            return;

        if (args.Sprite != null && component.SpriteLayer != null && args.Sprite.LayerMapTryGet(component.SpriteLayer, out var layer))
        {
            args.Sprite.LayerSetVisible(layer, isSealed);
        }

        _itemSystem.VisualsChanged(uid);
    }

    private void OnGetEquipmentVisuals(Entity<SealableClothingVisualsComponent> sealable, ref GetEquipmentVisualsEvent args)
    {
        var (uid, comp) = sealable;

        if (!TryComp(uid, out AppearanceComponent? appearance)
            || !AppearanceSystem.TryGetData<bool>(uid, SealableClothingVisuals.Sealed, out var isSealed, appearance)
            || !isSealed)
            return;

        if (!comp.VisualLayers.TryGetValue(args.Slot, out var layers))
            return;

        var i = 65;
        foreach (var layer in layers)
        {
            var key = layer.MapKeys?.FirstOrDefault();
            if (key == null)
            {
                key = i == 65 ? $"{args.Slot}-sealed" : $"{args.Slot}-sealed-{i}";
                i++;
            }

            args.Layers.Add((key, layer));
        }
    }
}