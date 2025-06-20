// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using Content.Goobstation.Shared.Redial;
using Robust.Client;
using Robust.Shared.IoC;
using Robust.Shared.Network;

namespace Content.Goobstation.Client.Redial;

public sealed class RedialManager : SharedRedialManager
{
    public override void Initialize()
    {
        _netManager.RegisterNetMessage<MsgRedial>(RedialOnMessage);
    }

    private void RedialOnMessage(MsgRedial message)
        => IoCManager.Resolve<IGameController>().Redial(message.Address);
}