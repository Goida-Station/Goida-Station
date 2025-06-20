// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Conchelle <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Client.JoinQueue;
using Content.Goobstation.Client.MisandryBox;
using Content.Goobstation.Client.Redial;
using Content.Goobstation.Client.Voice;
using Content.Goobstation.Shared.MisandryBox.JumpScare;
using Robust.Shared.IoC;

namespace Content.Goobstation.Client.IoC;

internal static class ContentGoobClientIoC
{
    internal static void Register()
    {
        var collection = IoCManager.Instance!;

        collection.Register<RedialManager>();
        collection.Register<IVoiceChatManager, VoiceChatClientManager>();
        collection.Register<JoinQueueManager>();
        collection.Register<IFullScreenImageJumpscare, ClientFullScreenImageJumpscare>();
    }
}
