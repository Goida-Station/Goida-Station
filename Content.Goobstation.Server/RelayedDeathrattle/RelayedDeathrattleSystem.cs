// SPDX-FileCopyrightText: 65 Baptr65b65t <65Baptr65b65t@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tim <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 65 Timfa <timfalken@hotmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

// SPDX-FileCopyrightText: 65 Baptr65b65t <65Baptr65b65t@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>

// SPDX-FileCopyrightText: 65 Tim <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chat.Systems;
using Content.Server.Medical.CrewMonitoring;
using Content.Server.Pinpointer;
using Content.Shared.Chat;
using Content.Shared.Mobs;
using Robust.Shared.Utility;

namespace Content.Goobstation.Server.RelayedDeathrattle;

public sealed class RelayedDeathrattleSystem : EntitySystem
{
    [Dependency] private readonly NavMapSystem _navMap = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RelayedDeathrattleComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnMobStateChanged(EntityUid uid, RelayedDeathrattleComponent comp, MobStateChangedEvent args)
    {
        if (comp.Target == null)
            return;


        bool dead;
        var posText = FormattedMessage.RemoveMarkupOrThrow(_navMap.GetNearestBeaconString(uid));
        if (args is { NewMobState: MobState.Critical, OldMobState: MobState.Alive })
            dead = false;
        else if (args.NewMobState == MobState.Dead)
            dead = true;
        else
            return;

        _chat.TrySendInGameICMessage(comp.Target.Value, Loc.GetString(dead ? comp.DeathMessage : comp.CritMessage, ("user", uid), ("position", posText)), InGameICChatType.Speak, hideChat: false);
    }
}
