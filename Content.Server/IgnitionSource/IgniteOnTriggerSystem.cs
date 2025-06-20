// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Explosion.EntitySystems;
using Content.Shared.IgnitionSource;
using Content.Shared.Timing;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Server.IgnitionSource;

/// <summary>
/// Handles igniting when triggered and stopping ignition after the delay.
/// </summary>
public sealed class IgniteOnTriggerSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedIgnitionSourceSystem _source = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly UseDelaySystem _useDelay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IgniteOnTriggerComponent, TriggerEvent>(OnTrigger);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        var query = EntityQueryEnumerator<IgniteOnTriggerComponent, IgnitionSourceComponent>();
        while (query.MoveNext(out var uid, out var comp, out var source))
        {
            if (!source.Ignited)
                continue;

            if (_timing.CurTime < comp.IgnitedUntil)
                continue;

            _source.SetIgnited((uid, source), false);
        }
    }

    private void OnTrigger(Entity<IgniteOnTriggerComponent> ent, ref TriggerEvent args)
    {
        // prevent spamming sound and ignition
        if (!TryComp(ent.Owner, out UseDelayComponent? useDelay) || _useDelay.IsDelayed((ent.Owner, useDelay)))
            return;

        _source.SetIgnited(ent.Owner);
        _audio.PlayPvs(ent.Comp.IgniteSound, ent);

        _useDelay.TryResetDelay((ent.Owner, useDelay));
        ent.Comp.IgnitedUntil = _timing.CurTime + ent.Comp.IgnitedTime;
    }
}