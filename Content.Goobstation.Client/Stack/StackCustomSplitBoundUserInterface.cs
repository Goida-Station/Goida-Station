// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Stacks;
using Content.Shared.Stacks;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Goobstation.Client.Stack
{
    [UsedImplicitly]
    public sealed class StackCustomSplitBoundUserInterface : BoundUserInterface
    {
        private IEntityManager _entManager;
        private EntityUid _owner;
        [ViewVariables]
        private StackCustomSplitWindow? _window;

        public StackCustomSplitBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
            _owner = owner;
            _entManager = IoCManager.Resolve<IEntityManager>();
        }

        protected override void Open()
        {
            base.Open();
            _window = this.CreateWindow<StackCustomSplitWindow>();

            if (_entManager.TryGetComponent<StackComponent>(_owner, out var comp))
                _window.SetMax(comp.Count);

            _window.ApplyButton.OnPressed += _ =>
            {
                if (int.TryParse((string?)_window.AmountLineEdit.Text, out var i))
                {
                    SendMessage(new StackCustomSplitAmountMessage(i));
                    _window.Close();
                }
            };
        }
    }
}