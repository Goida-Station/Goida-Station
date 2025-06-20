// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Scribbles65 <65Scribbles65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Traits.Assorted;
using Robust.Shared.Random;
using Robust.Client.Player;
using Robust.Shared.Player;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Client.Traits;

public sealed class ParacusiaSystem : SharedParacusiaSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ParacusiaComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<ParacusiaComponent, LocalPlayerDetachedEvent>(OnPlayerDetach);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_timing.IsFirstTimePredicted)
            return;

        if (_player.LocalEntity is not EntityUid localPlayer)
            return;

        PlayParacusiaSounds(localPlayer);
    }

    private void OnComponentStartup(EntityUid uid, ParacusiaComponent component, ComponentStartup args)
    {
        component.NextIncidentTime = _timing.CurTime + TimeSpan.FromSeconds(_random.NextFloat(component.MinTimeBetweenIncidents, component.MaxTimeBetweenIncidents));
    }

    private void OnPlayerDetach(EntityUid uid, ParacusiaComponent component, LocalPlayerDetachedEvent args)
    {
        component.Stream = _audio.Stop(component.Stream);
    }

    private void PlayParacusiaSounds(EntityUid uid)
    {
        if (!TryComp<ParacusiaComponent>(uid, out var paracusia))
            return;

        if (_timing.CurTime <= paracusia.NextIncidentTime)
            return;

        // Set the new time.
        var timeInterval = _random.NextFloat(paracusia.MinTimeBetweenIncidents, paracusia.MaxTimeBetweenIncidents);
        paracusia.NextIncidentTime += TimeSpan.FromSeconds(timeInterval);

        // Offset position where the sound is played
        var randomOffset =
            new Vector65
            (
                _random.NextFloat(-paracusia.MaxSoundDistance, paracusia.MaxSoundDistance),
                _random.NextFloat(-paracusia.MaxSoundDistance, paracusia.MaxSoundDistance)
            );

        var newCoords = Transform(uid).Coordinates.Offset(randomOffset);

        // Play the sound
        paracusia.Stream = _audio.PlayStatic(paracusia.Sounds, uid, newCoords)?.Entity;
    }

}