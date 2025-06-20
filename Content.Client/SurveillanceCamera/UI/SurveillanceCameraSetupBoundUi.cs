// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <milonpl.git@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.SurveillanceCamera;

namespace Content.Client.SurveillanceCamera.UI;

public sealed class SurveillanceCameraSetupBoundUi : BoundUserInterface
{
    [ViewVariables]
    private readonly SurveillanceCameraSetupUiKey _type;

    [ViewVariables]
    private SurveillanceCameraSetupWindow? _window;

    public SurveillanceCameraSetupBoundUi(EntityUid component, Enum uiKey) : base(component, uiKey)
    {
        if (uiKey is not SurveillanceCameraSetupUiKey key)
            return;

        _type = key;
    }

    protected override void Open()
    {
        base.Open();

        _window = new();

        if (_type == SurveillanceCameraSetupUiKey.Router)
        {
            _window.HideNameSelector();
        }

        _window.OpenCentered();
        _window.OnNameConfirm += SendDeviceName;
        _window.OnNetworkConfirm += SendSelectedNetwork;
        _window.OnClose += Close;
    }

    private void SendSelectedNetwork(int idx)
    {
        SendMessage(new SurveillanceCameraSetupSetNetwork(idx));
    }

    private void SendDeviceName(string name)
    {
        SendMessage(new SurveillanceCameraSetupSetName(name));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (_window == null || state is not SurveillanceCameraSetupBoundUiState cast)
        {
            return;
        }

        _window.UpdateState(cast.Name, cast.NameDisabled, cast.NetworkDisabled);
        _window.LoadAvailableNetworks(cast.Network, cast.Networks);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _window?.Dispose();
            _window = null;
        }
    }
}