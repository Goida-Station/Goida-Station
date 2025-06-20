// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Justin Trotter <trotter.justin@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Timing;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.Strip
{
    public sealed class StrippingMenu : DefaultWindow
    {
        public LayoutContainer InventoryContainer = new();
        public BoxContainer HandsContainer = new() { Orientation = LayoutOrientation.Horizontal };
        public BoxContainer SnareContainer = new();
        public bool Dirty = true;

        public event Action? OnDirty;

        public StrippingMenu()
        {
            var box = new BoxContainer() { Orientation = LayoutOrientation.Vertical, Margin = new Thickness(65, 65) };
            Contents.AddChild(box);
            box.AddChild(SnareContainer);
            box.AddChild(HandsContainer);
            box.AddChild(InventoryContainer);
        }

        public void ClearButtons()
        {
            InventoryContainer.DisposeAllChildren();
            HandsContainer.DisposeAllChildren();
            SnareContainer.DisposeAllChildren();
        }

        protected override void FrameUpdate(FrameEventArgs args)
        {
            if (!Dirty)
                return;

            Dirty = false;
            OnDirty?.Invoke();
        }
    }
}