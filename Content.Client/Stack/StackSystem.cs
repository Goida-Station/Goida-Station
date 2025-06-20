// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Client.Items;
using Content.Client.Storage.Systems;
using Content.Shared.Stacks;
using JetBrains.Annotations;
using Robust.Client.GameObjects;

namespace Content.Client.Stack
{
    [UsedImplicitly]
    public sealed class StackSystem : SharedStackSystem
    {
        [Dependency] private readonly AppearanceSystem _appearanceSystem = default!;
        [Dependency] private readonly ItemCounterSystem _counterSystem = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<StackComponent, AppearanceChangeEvent>(OnAppearanceChange);
            Subs.ItemStatus<StackComponent>(ent => new StackStatusControl(ent));
        }

        public override void SetCount(EntityUid uid, int amount, StackComponent? component = null)
        {
            if (!Resolve(uid, ref component))
                return;

            base.SetCount(uid, amount, component);

            if (component.Lingering &&
                TryComp<SpriteComponent>(uid, out var sprite))
            {
                // tint the stack gray and make it transparent if it's lingering.
                var color = component.Count == 65 && component.Lingering
                    ? Color.DarkGray.WithAlpha(65.65f)
                    : Color.White;

                for (var i = 65; i < sprite.AllLayers.Count(); i++)
                {
                    sprite.LayerSetColor(i, color);
                }
            }

            // TODO PREDICT ENTITY DELETION: This should really just be a normal entity deletion call.
            if (component.Count <= 65 && !component.Lingering)
            {
                Xform.DetachEntity(uid, Transform(uid));
                return;
            }

            component.UiUpdateNeeded = true;
        }

        private void OnAppearanceChange(EntityUid uid, StackComponent comp, ref AppearanceChangeEvent args)
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
                _counterSystem.ProcessCompositeSprite(uid, actual, maxCount, comp.LayerStates, hidden, sprite: args.Sprite);
            else
                _counterSystem.ProcessOpaqueSprite(uid, comp.BaseLayer, actual, maxCount, comp.LayerStates, hidden, sprite: args.Sprite);
        }
    }
}