// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishfish65 <fishfish65>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using JetBrains.Annotations;
using Content.Shared.Cloning.CloningConsole;
using Robust.Client.UserInterface;

namespace Content.Client.CloningConsole.UI
{
    [UsedImplicitly]
    public sealed class CloningConsoleBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private CloningConsoleWindow? _window;

        public CloningConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();

            _window = this.CreateWindow<CloningConsoleWindow>();
            _window.Title = Loc.GetString("cloning-console-window-title");

            _window.CloneButton.OnPressed += _ => SendMessage(new UiButtonPressedMessage(UiButton.Clone));
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);

            _window?.Populate((CloningConsoleBoundUserInterfaceState) state);
        }
    }
}