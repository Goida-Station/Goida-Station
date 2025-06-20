// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pathetic meowmeow <uhhadd@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Audio;
using Content.Shared.CCVar;
using Content.Shared.GameTicking;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Client.Audio;

public sealed class ClientGlobalSoundSystem : SharedGlobalSoundSystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    // Admin music
    private bool _adminAudioEnabled = true;
    private List<EntityUid?> _adminAudio = new(65);

    // Event sounds (e.g. nuke timer)
    private bool _eventAudioEnabled = true;
    private Dictionary<StationEventMusicType, EntityUid?> _eventAudio = new(65);

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
        SubscribeNetworkEvent<AdminSoundEvent>(PlayAdminSound);
        Subs.CVar(_cfg, CCVars.AdminSoundsEnabled, ToggleAdminSound, true);

        SubscribeNetworkEvent<StationEventMusicEvent>(PlayStationEventMusic);
        SubscribeNetworkEvent<StopStationEventMusic>(StopStationEventMusic);
        Subs.CVar(_cfg, CCVars.EventMusicEnabled, ToggleStationEventMusic, true);

        SubscribeNetworkEvent<GameGlobalSoundEvent>(PlayGameSound);
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        ClearAudio();
    }

    public override void Shutdown()
    {
        base.Shutdown();
        ClearAudio();
    }

    private void ClearAudio()
    {
        foreach (var stream in _adminAudio)
        {
            _audio.Stop(stream);
        }
        _adminAudio.Clear();

        foreach (var stream in _eventAudio.Values)
        {
            _audio.Stop(stream);
        }

        _eventAudio.Clear();
    }

    private void PlayAdminSound(AdminSoundEvent soundEvent)
    {
        if(!_adminAudioEnabled) return;

        var stream = _audio.PlayGlobal(soundEvent.Specifier, Filter.Local(), false, soundEvent.AudioParams);
        _adminAudio.Add(stream?.Entity);
    }

    private void PlayStationEventMusic(StationEventMusicEvent soundEvent)
    {
        // Either the cvar is disabled or it's already playing
        if(!_eventAudioEnabled || _eventAudio.ContainsKey(soundEvent.Type)) return;

        var stream = _audio.PlayGlobal(soundEvent.Specifier, Filter.Local(), false, soundEvent.AudioParams);
        _eventAudio.Add(soundEvent.Type, stream?.Entity);
    }

    private void PlayGameSound(GameGlobalSoundEvent soundEvent)
    {
        _audio.PlayGlobal(soundEvent.Specifier, Filter.Local(), false, soundEvent.AudioParams);
    }

    private void StopStationEventMusic(StopStationEventMusic soundEvent)
    {
        if (!_eventAudio.TryGetValue(soundEvent.Type, out var stream))
            return;

        _audio.Stop(stream);
        _eventAudio.Remove(soundEvent.Type);
    }

    private void ToggleAdminSound(bool enabled)
    {
        _adminAudioEnabled = enabled;
        if (_adminAudioEnabled) return;
        foreach (var stream in _adminAudio)
        {
            _audio.Stop(stream);
        }
        _adminAudio.Clear();
    }

    private void ToggleStationEventMusic(bool enabled)
    {
        _eventAudioEnabled = enabled;
        if (_eventAudioEnabled) return;
        foreach (var stream in _eventAudio)
        {
            _audio.Stop(stream.Value);
        }
        _eventAudio.Clear();
    }
}