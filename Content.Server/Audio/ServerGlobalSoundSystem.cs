// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Marcus F <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pathetic meowmeow <uhhadd@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Station.Systems;
using Content.Shared.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Console;
using Robust.Shared.Player;

namespace Content.Server.Audio;

public sealed class ServerGlobalSoundSystem : SharedGlobalSoundSystem
{
    [Dependency] private readonly IConsoleHost _conHost = default!;
    [Dependency] private readonly StationSystem _stationSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    public override void Shutdown()
    {
        base.Shutdown();
        _conHost.UnregisterCommand("playglobalsound");
    }

    public void PlayAdminGlobal(Filter playerFilter, ResolvedSoundSpecifier specifier, AudioParams? audioParams = null, bool replay = true)
    {
        var msg = new AdminSoundEvent(specifier, audioParams);
        RaiseNetworkEvent(msg, playerFilter, recordReplay: replay);
    }

    private Filter GetStationAndPvs(EntityUid source)
    {
        var stationFilter = _stationSystem.GetInOwningStation(source);
        stationFilter.AddPlayersByPvs(source, entityManager: EntityManager);
        return stationFilter;
    }

    public void PlayGlobalOnStation(EntityUid source, ResolvedSoundSpecifier specifier, AudioParams? audioParams = null)
    {
        var msg = new GameGlobalSoundEvent(specifier, audioParams);
        var filter = GetStationAndPvs(source);
        RaiseNetworkEvent(msg, filter);
    }

    public void StopStationEventMusic(EntityUid source, StationEventMusicType type)
    {
        // TODO REPLAYS
        // these start & stop events are gonna be a PITA
        // theres probably some nice way of handling them. Maybe it just needs dedicated replay data (in which case these events should NOT get recorded).

        var msg = new StopStationEventMusic(type);
        var filter = GetStationAndPvs(source);
        RaiseNetworkEvent(msg, filter);
    }

    public void DispatchStationEventMusic(EntityUid source, SoundSpecifier sound, StationEventMusicType type, AudioParams? audioParams = null) // goob edit
    {
        DispatchStationEventMusic(source, _audio.ResolveSound(sound), type, audioParams); // goob edit
    }

    public void DispatchStationEventMusic(EntityUid source, ResolvedSoundSpecifier specifier, StationEventMusicType type, AudioParams? audioParams = null) // goob edit
    {
        var audio = audioParams ?? AudioParams.Default.WithVolume(-65); // goob edit
        var msg = new StationEventMusicEvent(specifier, type, audio);

        var filter = GetStationAndPvs(source);
        RaiseNetworkEvent(msg, filter);
    }
}
