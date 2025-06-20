// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Explosion.EntitySystems;
using Robust.Shared.Player;

namespace Content.Server.GhostKick;

public sealed class GhostKickUserOnTriggerSystem : EntitySystem
{
    [Dependency] private readonly GhostKickManager _ghostKickManager = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<GhostKickUserOnTriggerComponent, TriggerEvent>(HandleMineTriggered);
    }

    private void HandleMineTriggered(EntityUid uid, GhostKickUserOnTriggerComponent userOnTriggerComponent, TriggerEvent args)
    {
        if (!TryComp(args.User, out ActorComponent? actor))
            return;

        _ghostKickManager.DoDisconnect(
            actor.PlayerSession.Channel,
            "Tripped over a kick mine, crashed through the fourth wall");

        args.Handled = true;
    }
}