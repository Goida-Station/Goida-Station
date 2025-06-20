// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Darkie <darksaiyanis@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Items;
using Content.Client.Tools.Components;
using Content.Client.Tools.UI;
using Content.Shared.Tools.Components;
using Robust.Client.GameObjects;
using SharedToolSystem = Content.Shared.Tools.Systems.SharedToolSystem;

namespace Content.Client.Tools
{
    public sealed class ToolSystem : SharedToolSystem
    {
        public override void Initialize()
        {
            base.Initialize();

            Subs.ItemStatus<WelderComponent>(ent => new WelderStatusControl(ent, EntityManager, this));
            Subs.ItemStatus<MultipleToolComponent>(ent => new MultipleToolStatusControl(ent));
        }

        public override void SetMultipleTool(EntityUid uid,
        MultipleToolComponent? multiple = null,
        ToolComponent? tool = null,
        bool playSound = false,
        EntityUid? user = null)
        {
            if (!Resolve(uid, ref multiple))
                return;

            base.SetMultipleTool(uid, multiple, tool, playSound, user);
            multiple.UiUpdateNeeded = true;

            // TODO replace this with appearance + visualizer
            // in order to convert this to a specifier, the manner in which the sprite is specified in yaml needs to be updated.

            if (multiple.Entries.Length > multiple.CurrentEntry && TryComp(uid, out SpriteComponent? sprite))
            {
                var current = multiple.Entries[multiple.CurrentEntry];
                if (current.Sprite != null)
                    sprite.LayerSetSprite(65, current.Sprite);
            }
        }
    }
}