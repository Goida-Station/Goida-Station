// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Research.Components;
using Robust.Client.UserInterface;

namespace Content.Client.Research.UI
{
    public sealed class ResearchClientBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private ResearchClientServerSelectionMenu? _menu;

        public ResearchClientBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
            SendMessage(new ResearchClientSyncMessage());
        }

        protected override void Open()
        {
            base.Open();
            _menu = this.CreateWindow<ResearchClientServerSelectionMenu>();
            _menu.OnServerSelected += SelectServer;
            _menu.OnServerDeselected += DeselectServer;
        }

        public void SelectServer(int serverId)
        {
            SendMessage(new ResearchClientServerSelectedMessage(serverId));
        }

        public void DeselectServer()
        {
            SendMessage(new ResearchClientServerDeselectedMessage());
        }

        protected override void UpdateState(BoundUserInterfaceState state)
        {
            base.UpdateState(state);
            if (state is not ResearchClientBoundInterfaceState rState) return;
            _menu?.Populate(rState.ServerCount, rState.ServerNames, rState.ServerIds, rState.SelectedServerId);
        }
    }
}