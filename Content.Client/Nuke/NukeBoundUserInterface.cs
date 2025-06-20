// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <milonpl.git@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Containers.ItemSlots;
using Content.Shared.Nuke;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Nuke
{
    [UsedImplicitly]
    public sealed class NukeBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private NukeMenu? _menu;

        public NukeBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();

            _menu = this.CreateWindow<NukeMenu>();

            _menu.OnKeypadButtonPressed += i =>
            {
                SendMessage(new NukeKeypadMessage(i));
            };
            _menu.OnEnterButtonPressed += () =>
            {
                SendMessage(new NukeKeypadEnterMessage());
            };
            _menu.OnClearButtonPressed += () =>
            {
                SendMessage(new NukeKeypadClearMessage());
            };

            _menu.EjectButton.OnPressed += _ =>
            {
                SendMessage(new ItemSlotButtonPressedEvent(SharedNukeComponent.NukeDiskSlotId));
            };
            _menu.AnchorButton.OnPressed += _ =>
            {
                SendMessage(new NukeAnchorMessage());
            };
            _menu.ArmButton.OnPressed += _ =>
            {
                SendMessage(new NukeArmedMessage());
            };
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            if (_menu == null)
                return;

            switch (state)
            {
                case NukeUiState msg:
                    _menu.UpdateState(msg);
                    break;
            }
        }
    }
}