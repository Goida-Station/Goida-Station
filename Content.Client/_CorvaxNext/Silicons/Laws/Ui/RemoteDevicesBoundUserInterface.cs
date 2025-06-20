// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 ImHoks <imhokzzzz@gmail.com>
// SPDX-FileCopyrightText: 65 KillanGenifer <killangenifer@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._CorvaxNext.Silicons.Borgs.Components;
using Robust.Client.UserInterface;
using static Content.Shared._CorvaxNext.Silicons.Borgs.Components.AiRemoteControllerComponent;

namespace Content.Client._CorvaxNext.Silicons.Laws.Ui;

public sealed class RemoteDevicesBoundUserInterface : BoundUserInterface
{
    private RemoteDevicesMenu? _menu;
    private EntityUid _owner;

    public RemoteDevicesBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _owner = owner;
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<RemoteDevicesMenu>();
        _menu.OnRemoteDeviceAction += SendAction;
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not RemoteDevicesBuiState msg)
            return;

        _menu?.Update(_owner, msg);
    }

    public void SendAction(RemoteDeviceActionEvent action)
    {
        SendMessage(new RemoteDeviceActionMessage(action));
    }
}
