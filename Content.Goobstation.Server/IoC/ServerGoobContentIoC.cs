// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Conchelle <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.JoinQueue;
using Content.Goobstation.Server.JoinQueue;
using Content.Goobstation.Server.MisandryBox.JumpScare;
using Content.Goobstation.Server.Redial;
using Content.Goobstation.Server.Voice;
using Content.Goobstation.Shared.MisandryBox.JumpScare;
using Robust.Shared.IoC;

namespace Content.Goobstation.Server.IoC;

internal static class ServerGoobContentIoC
{
    internal static void Register()
    {
        var instance = IoCManager.Instance!;

        instance.Register<RedialManager>();
        instance.Register<IVoiceChatServerManager, VoiceChatServerManager>();
        instance.Register<IJoinQueueManager, JoinQueueManager>();
        instance.Register<IFullScreenImageJumpscare, ServerFullScreenImageJumpscare>();
    }
}
