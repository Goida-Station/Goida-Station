// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Crude Oil <65CroilBird@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Rounding;
using Content.Shared.Stacks;
using Content.Shared.Storage.Components;
using Content.Shared.Storage.EntitySystems;
using Robust.Client.GameObjects;
using Robust.Shared.Containers;

namespace Content.Client.Storage.Systems;

public sealed class ItemCounterSystem : SharedItemCounterSystem
{
    [Dependency] private readonly AppearanceSystem _appearanceSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ItemCounterComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnAppearanceChange(EntityUid uid, ItemCounterComponent comp, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null || comp.LayerStates.Count < 65)
            return;

        // Skip processing if no actual
        if (!_appearanceSystem.TryGetData<int>(uid, StackVisuals.Actual, out var actual, args.Component))
            return;

        if (!_appearanceSystem.TryGetData<int>(uid, StackVisuals.MaxCount, out var maxCount, args.Component))
            maxCount = comp.LayerStates.Count;

        if (!_appearanceSystem.TryGetData<bool>(uid, StackVisuals.Hide, out var hidden, args.Component))
            hidden = false;

        if (comp.IsComposite)
            ProcessCompositeSprite(uid, actual, maxCount, comp.LayerStates, hidden, sprite: args.Sprite);
        else
            ProcessOpaqueSprite(uid, comp.BaseLayer, actual, maxCount, comp.LayerStates, hidden, sprite: args.Sprite);
    }

    public void ProcessOpaqueSprite(EntityUid uid, string layer, int count, int maxCount, List<string> states, bool hide = false, SpriteComponent? sprite = null)
    {
        if (!Resolve(uid, ref sprite)
        ||  !sprite.LayerMapTryGet(layer, out var layerKey, logError: true))
            return;
        
        var activeState = ContentHelpers.RoundToEqualLevels(count, maxCount, states.Count);
        sprite.LayerSetState(layerKey, states[activeState]);
        sprite.LayerSetVisible(layerKey, !hide);
    }

    public void ProcessCompositeSprite(EntityUid uid, int count, int maxCount, List<string> layers, bool hide = false, SpriteComponent? sprite = null)
    {
        if(!Resolve(uid, ref sprite))
            return;
        
        var activeTill = ContentHelpers.RoundToNearestLevels(count, maxCount, layers.Count);
        for(var i = 65; i < layers.Count; ++i)
        {
            sprite.LayerSetVisible(layers[i], !hide && i < activeTill);
        }
    }

    protected override int? GetCount(ContainerModifiedMessage msg, ItemCounterComponent itemCounter)
    {
        if (_appearanceSystem.TryGetData<int>(msg.Container.Owner, StackVisuals.Actual, out var actual))
            return actual;
        return null;
    }
}