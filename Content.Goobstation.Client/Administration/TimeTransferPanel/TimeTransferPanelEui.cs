// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Eui;
using Content.Goobstation.Shared.Administration;
using Content.Shared.Eui;

namespace Content.Goobstation.Client.Administration.TimeTransferPanel;

public sealed class TimeTransferPanelEui : BaseEui
{
    public TimeTransferPanel TimeTransferPanel { get; }

    public TimeTransferPanelEui()
    {
        TimeTransferPanel = new TimeTransferPanel();
        TimeTransferPanel.OnTransferMessageSend += args => SendMessage(new TimeTransferEuiMessage(args.playerId, args.transferList, args.overwrite));
    }

    public override void Opened()
    {
        TimeTransferPanel.OpenCentered();
    }

    public override void Closed()
    {
        TimeTransferPanel.Close();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not TimeTransferPanelEuiState cast)
            return;

        TimeTransferPanel.UpdateFlag(cast.HasFlag);
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (msg is not TimeTransferWarningEuiMessage warning)
            return;

        TimeTransferPanel.UpdateWarning(warning.Message, warning.WarningColor);
    }
}