// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.NetworkConfigurator.Systems;
using Content.Shared.DeviceNetwork;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.NetworkConfigurator;

public sealed class NetworkConfiguratorBoundUserInterface : BoundUserInterface
{
    private readonly NetworkConfiguratorSystem _netConfig;

    [ViewVariables]
    private NetworkConfiguratorConfigurationMenu? _configurationMenu;

    [ViewVariables]
    private NetworkConfiguratorLinkMenu? _linkMenu;

    [ViewVariables]
    private NetworkConfiguratorListMenu? _listMenu;

    public NetworkConfiguratorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        _netConfig = EntMan.System<NetworkConfiguratorSystem>();
    }

    public void OnRemoveButtonPressed(string address)
    {
        SendMessage(new NetworkConfiguratorRemoveDeviceMessage(address));
    }

    protected override void Open()
    {
        base.Open();

        switch (UiKey)
        {
            case NetworkConfiguratorUiKey.List:
                _listMenu = this.CreateWindow<NetworkConfiguratorListMenu>();
                _listMenu.ClearButton.OnPressed += _ => OnClearButtonPressed();
                _listMenu.OnRemoveAddress += OnRemoveButtonPressed;
                break;
            case NetworkConfiguratorUiKey.Configure:
                _configurationMenu = this.CreateWindow<NetworkConfiguratorConfigurationMenu>();
                _configurationMenu.Set.OnPressed += _ => OnConfigButtonPressed(NetworkConfiguratorButtonKey.Set);
                _configurationMenu.Add.OnPressed += _ => OnConfigButtonPressed(NetworkConfiguratorButtonKey.Add);
                //_configurationMenu.Edit.OnPressed += _ => OnConfigButtonPressed(NetworkConfiguratorButtonKey.Edit);
                _configurationMenu.Clear.OnPressed += _ => OnConfigButtonPressed(NetworkConfiguratorButtonKey.Clear);
                _configurationMenu.Copy.OnPressed += _ => OnConfigButtonPressed(NetworkConfiguratorButtonKey.Copy);
                _configurationMenu.Show.OnPressed += OnShowPressed;
                _configurationMenu.Show.Pressed = _netConfig.ConfiguredListIsTracked(Owner);
                _configurationMenu.OnRemoveAddress += OnRemoveButtonPressed;
                break;
            case NetworkConfiguratorUiKey.Link:
                _linkMenu = this.CreateWindow<NetworkConfiguratorLinkMenu>();
                _linkMenu.OnLinkDefaults += args =>
                {
                    SendMessage(new NetworkConfiguratorLinksSaveMessage(args));
                };

                _linkMenu.OnToggleLink += (left, right) =>
                {
                    SendMessage(new NetworkConfiguratorToggleLinkMessage(left, right));
                };

                _linkMenu.OnClearLinks += () =>
                {
                    SendMessage(new NetworkConfiguratorClearLinksMessage());
                };
                break;
        }
    }

    private void OnShowPressed(BaseButton.ButtonEventArgs args)
    {
        _netConfig.ToggleVisualization(Owner, args.Button.Pressed);
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        switch (state)
        {
            case NetworkConfiguratorUserInterfaceState configState:
                _listMenu?.UpdateState(configState);
                break;
            case DeviceListUserInterfaceState listState:
                _configurationMenu?.UpdateState(listState);
                break;
            case DeviceLinkUserInterfaceState linkState:
                _linkMenu?.UpdateState(linkState);
                break;
        }
    }

    private void OnClearButtonPressed()
    {
        SendMessage(new NetworkConfiguratorClearDevicesMessage());
    }

    private void OnConfigButtonPressed(NetworkConfiguratorButtonKey buttonKey)
    {
        SendMessage(new NetworkConfiguratorButtonPressedMessage(buttonKey));
    }
}