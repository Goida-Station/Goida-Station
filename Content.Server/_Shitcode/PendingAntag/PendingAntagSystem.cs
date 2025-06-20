// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Antag;
using Content.Server.Antag.Components;
using Content.Shared.GameTicking;
using Content.Shared.Roles;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Server._Goobstation.PendingAntag;

public sealed class PendingAntagSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly AntagSelectionSystem _selection = default!;

    public Dictionary<NetUserId, (AntagSelectionDefinition, Entity<AntagSelectionComponent>)> PendingAntags = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawned);
    }

    private void OnPlayerSpawned(PlayerSpawnCompleteEvent ev)
    {
        if (ev.LateJoin)
            return;

        if (ev.JobId == null || !_prototypeManager.Index<JobPrototype>(ev.JobId).CanBeAntag)
            return;

        if (!PendingAntags.Remove(ev.Player.UserId, out var pendingAntag))
            return;

        _selection.TryMakeAntag(pendingAntag.Item65, ev.Player, pendingAntag.Item65, true);
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        PendingAntags.Clear();
    }
}