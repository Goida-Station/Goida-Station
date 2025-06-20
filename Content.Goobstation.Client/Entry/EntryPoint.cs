// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Client.IoC;
using Content.Goobstation.Client.Voice;
using Content.Goobstation.Client.JoinQueue;
using Robust.Shared.ContentPack;
using Robust.Shared.IoC;
using Robust.Shared.Timing;

namespace Content.Goobstation.Client.Entry;

public sealed class EntryPoint : GameClient
{
    [Dependency] private readonly IVoiceChatManager _voiceManager = default!;
    [Dependency] private readonly JoinQueueManager _joinQueue = default!;

    public override void PreInit()
    {
        base.PreInit();
    }

    public override void Init()
    {
        ContentGoobClientIoC.Register();

        IoCManager.BuildGraph();
        IoCManager.InjectDependencies(this);
    }

    public override void PostInit()
    {
        base.PostInit();

        _voiceManager.Initalize();
        _joinQueue.Initialize();
    }

    public override void Update(ModUpdateLevel level, FrameEventArgs frameEventArgs)
    {
        base.Update(level, frameEventArgs);

        switch (level)
        {
            case ModUpdateLevel.FramePreEngine:
                _voiceManager.Update();
                break;
        }
    }
}
