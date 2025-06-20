// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.UserInterface.Fragments;
using Content.Shared.CartridgeLoader;
using Content.Shared.CartridgeLoader.Cartridges;
using Robust.Client.UserInterface;

namespace Content.Client.CartridgeLoader.Cartridges;

public sealed partial class NotekeeperUi : UIFragment
{
    private NotekeeperUiFragment? _fragment;

    public override Control GetUIFragmentRoot()
    {
        return _fragment!;
    }

    public override void Setup(BoundUserInterface userInterface, EntityUid? fragmentOwner)
    {
        _fragment = new NotekeeperUiFragment();
        _fragment.OnNoteRemoved += note => SendNotekeeperMessage(NotekeeperUiAction.Remove, note, userInterface);
        _fragment.OnNoteAdded += note => SendNotekeeperMessage(NotekeeperUiAction.Add, note, userInterface);
    }

    public override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is not NotekeeperUiState notekeepeerState)
            return;

        _fragment?.UpdateState(notekeepeerState.Notes);
    }

    private void SendNotekeeperMessage(NotekeeperUiAction action, string note, BoundUserInterface userInterface)
    {
        var notekeeperMessage = new NotekeeperUiMessageEvent(action, note);
        var message = new CartridgeUiMessage(notekeeperMessage);
        userInterface.SendMessage(message);
    }
}