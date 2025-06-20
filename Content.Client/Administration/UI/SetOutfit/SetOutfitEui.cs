// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leo <lzimann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Eui;
using Content.Shared.Administration;
using Content.Shared.Eui;
using JetBrains.Annotations;

namespace Content.Client.Administration.UI.SetOutfit
{
    [UsedImplicitly]
    public sealed class SetOutfitEui : BaseEui
    {
        private readonly SetOutfitMenu _window;
        private IEntityManager _entManager;

        public SetOutfitEui()
        {
            _entManager = IoCManager.Resolve<IEntityManager>();
            _window = new SetOutfitMenu();
            _window.OnClose += OnClosed;
        }

        private void OnClosed()
        {
            SendMessage(new CloseEuiMessage());
        }

        public override void Opened()
        {
            _window.OpenCentered();
        }

        public override void Closed()
        {
            base.Closed();
            _window.Close();
        }

        public override void HandleState(EuiStateBase state)
        {
            var outfitState = (SetOutfitEuiState) state;
            _window.TargetEntityId = outfitState.TargetNetEntity;

        }
    }
}