// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chat.Systems;
using Content.Server.Speech;
using Content.Shared.Speech;
using Content.Shared.Chat;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;

namespace Content.Server.SurveillanceCamera;

/// <summary>
///     This handles speech for surveillance camera monitors.
/// </summary>
public sealed class SurveillanceCameraSpeakerSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly SpeechSoundSystem _speechSound = default!;
    [Dependency] private readonly ChatSystem _chatSystem = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<SurveillanceCameraSpeakerComponent, SurveillanceCameraSpeechSendEvent>(OnSpeechSent);
    }

    private void OnSpeechSent(EntityUid uid, SurveillanceCameraSpeakerComponent component,
        SurveillanceCameraSpeechSendEvent args)
    {
        if (!component.SpeechEnabled)
        {
            return;
        }

        var time = _gameTiming.CurTime;
        var cd = TimeSpan.FromSeconds(component.SpeechSoundCooldown);

        // this part's mostly copied from speech
        //     what is wrong with you?
        if (time - component.LastSoundPlayed < cd
            && TryComp<SpeechComponent>(args.Speaker, out var speech))
        {
            var sound = _speechSound.GetSpeechSound((args.Speaker, speech), args.Message);
            _audioSystem.PlayPvs(sound, uid);

            component.LastSoundPlayed = time;
        }

        var nameEv = new TransformSpeakerNameEvent(args.Speaker, Name(args.Speaker));
        RaiseLocalEvent(args.Speaker, nameEv);

        var name = Loc.GetString("speech-name-relay", ("speaker", Name(uid)),
            ("originalName", nameEv.VoiceName));

        // log to chat so people can identity the speaker/source, but avoid clogging ghost chat if there are many radios
        _chatSystem.TrySendInGameICMessage(uid, args.Message, InGameICChatType.Speak, ChatTransmitRange.GhostRangeLimit, nameOverride: name);
    }
}