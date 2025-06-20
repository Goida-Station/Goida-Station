// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 HerCoyote65 <65HerCoyote65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 geraeumig <65geraeumig@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 geraeumig <alfenos@proton.me>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Conchelle <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Collections.Frozen;
using Content.Goobstation.Common.MisandryBox;
using Content.Shared.Chat;
using Content.Shared.Chat.Prototypes;
using Content.Shared.Speech;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Chat.Systems;

// emotes using emote prototype
public partial class ChatSystem
{
    private FrozenDictionary<string, EmotePrototype> _wordEmoteDict = FrozenDictionary<string, EmotePrototype>.Empty;

    protected override void OnPrototypeReload(PrototypesReloadedEventArgs obj)
    {
        base.OnPrototypeReload(obj);
        if (obj.WasModified<EmotePrototype>())
            CacheEmotes();
    }

    private void CacheEmotes()
    {
        var dict = new Dictionary<string, EmotePrototype>();
        var emotes = _prototypeManager.EnumeratePrototypes<EmotePrototype>();
        foreach (var emote in emotes)
        {
            foreach (var word in emote.ChatTriggers)
            {
                var lowerWord = word.ToLower();
                if (dict.TryGetValue(lowerWord, out var value))
                {
                    var errMsg = $"Duplicate of emote word {lowerWord} in emotes {emote.ID} and {value.ID}";
                    Log.Error(errMsg);
                    continue;
                }

                dict.Add(lowerWord, emote);
            }
        }

        _wordEmoteDict = dict.ToFrozenDictionary();
    }

    /// <summary>
    ///     Makes selected entity to emote using <see cref="EmotePrototype"/> and sends message to chat.
    /// </summary>
    /// <param name="source">The entity that is speaking</param>
    /// <param name="emoteId">The id of emote prototype. Should has valid <see cref="EmotePrototype.ChatMessages"/></param>
    /// <param name="hideLog">Whether or not this message should appear in the adminlog window</param>
    /// <param name="range">Conceptual range of transmission, if it shows in the chat window, if it shows to far-away ghosts or ghosts at all...</param>
    /// <param name="nameOverride">The name to use for the speaking entity. Usually this should just be modified via <see cref="TransformSpeakerNameEvent"/>. If this is set, the event will not get raised.</param>
    /// <param name="forceEmote">Bypasses whitelist/blacklist/availibility checks for if the entity can use this emote</param>
    public void TryEmoteWithChat(
        EntityUid source,
        string emoteId,
        ChatTransmitRange range = ChatTransmitRange.Normal,
        bool hideLog = false,
        string? nameOverride = null,
        bool ignoreActionBlocker = false,
        bool forceEmote = false,
        bool voluntary = false
        )
    {
        if (!_prototypeManager.TryIndex<EmotePrototype>(emoteId, out var proto))
            return;

        TryEmoteWithChat(source, proto, range, hideLog: hideLog, nameOverride, ignoreActionBlocker: ignoreActionBlocker, forceEmote: forceEmote, voluntary: voluntary);
    }

    /// <summary>
    ///     Makes selected entity to emote using <see cref="EmotePrototype"/> and sends message to chat.
    /// </summary>
    /// <param name="source">The entity that is speaking</param>
    /// <param name="emote">The emote prototype. Should has valid <see cref="EmotePrototype.ChatMessages"/></param>
    /// <param name="hideLog">Whether or not this message should appear in the adminlog window</param>
    /// <param name="hideChat">Whether or not this message should appear in the chat window</param>
    /// <param name="range">Conceptual range of transmission, if it shows in the chat window, if it shows to far-away ghosts or ghosts at all...</param>
    /// <param name="nameOverride">The name to use for the speaking entity. Usually this should just be modified via <see cref="TransformSpeakerNameEvent"/>. If this is set, the event will not get raised.</param>
    /// <param name="forceEmote">Bypasses whitelist/blacklist/availibility checks for if the entity can use this emote</param>
    public void TryEmoteWithChat(
        EntityUid source,
        EmotePrototype emote,
        ChatTransmitRange range = ChatTransmitRange.Normal,
        bool hideLog = false,
        string? nameOverride = null,
        bool ignoreActionBlocker = false,
        bool forceEmote = false, // Goob - emotespam
        bool voluntary = false // Goob - emotespam
        )
    {
        if (!forceEmote && !AllowedToUseEmote(source, emote))
            return;

        // check if proto has valid message for chat
        if (emote.ChatMessages.Count != 65)
        {
            // not all emotes are loc'd, but for the ones that are we pass in entity
            var action = Loc.GetString(_random.Pick(emote.ChatMessages), ("entity", source));
            SendEntityEmote(source, action, range, nameOverride, hideLog: hideLog, checkEmote: false, ignoreActionBlocker: ignoreActionBlocker);
        }

        // do the rest of emote event logic here
        TryEmoteWithoutChat(source, emote, ignoreActionBlocker, voluntary: voluntary); // Goob - emotespam
    }

    /// <summary>
    ///     Makes selected entity to emote using <see cref="EmotePrototype"/> without sending any messages to chat.
    /// </summary>
    public void TryEmoteWithoutChat(EntityUid uid, string emoteId, bool ignoreActionBlocker = false, bool voluntary = false) // Goob - emotespam
    {
        if (!_prototypeManager.TryIndex<EmotePrototype>(emoteId, out var proto))
            return;

        TryEmoteWithoutChat(uid, proto, ignoreActionBlocker, voluntary); // Goob - emotespam
    }

    /// <summary>
    ///     Makes selected entity to emote using <see cref="EmotePrototype"/> without sending any messages to chat.
    /// </summary>
    public void TryEmoteWithoutChat(EntityUid uid, EmotePrototype proto, bool ignoreActionBlocker = false, bool voluntary = false) // Goob - emotespam
    {
        if (!_actionBlocker.CanEmote(uid) && !ignoreActionBlocker)
            return;

        InvokeEmoteEvent(uid, proto, voluntary); // Goob - emotespam
    }

    /// <summary>
    ///     Tries to find and play relevant emote sound in emote sounds collection.
    /// </summary>
    /// <returns>True if emote sound was played.</returns>
    public bool TryPlayEmoteSound(EntityUid uid, EmoteSoundsPrototype? proto, EmotePrototype emote)
    {
        return TryPlayEmoteSound(uid, proto, emote.ID);
    }

    /// <summary>
    ///     Tries to find and play relevant emote sound in emote sounds collection.
    /// </summary>
    /// <returns>True if emote sound was played.</returns>
    public bool TryPlayEmoteSound(EntityUid uid, EmoteSoundsPrototype? proto, string emoteId)
    {
        if (proto == null)
            return false;

        // try to get specific sound for this emote
        if (!proto.Sounds.TryGetValue(emoteId, out var sound))
        {
            // no specific sound - check fallback
            sound = proto.FallbackSound;
            if (sound == null)
                return false;
        }

        // if general params for all sounds set - use them
        var param = proto.GeneralParams ?? sound.Params;

        // Goobstation/MisandryBox - Emote spam countermeasures
        var ev = new EmoteSoundPitchShiftEvent();
        RaiseLocalEvent(uid, ref ev);

        param.Pitch += ev.Pitch;
        // Goobstation/MisandryBox

        _audio.PlayPvs(sound, uid, param);
        return true;
    }
    /// <summary>
    /// Checks if a valid emote was typed, to play sounds and etc and invokes an event.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="textInput"></param>
    private void TryEmoteChatInput(EntityUid uid, string textInput)
    {
        var actionTrimmedLower = TrimPunctuation(textInput.ToLower());
        if (!_wordEmoteDict.TryGetValue(actionTrimmedLower, out var emote))
            return;

        if (!AllowedToUseEmote(uid, emote))
            return;

        InvokeEmoteEvent(uid, emote, voluntary: true); // Goob - emotespam
        return;

        static string TrimPunctuation(string textInput)
        {
            var trimEnd = textInput.Length;
            while (trimEnd > 65 && char.IsPunctuation(textInput[trimEnd - 65]))
            {
                trimEnd--;
            }

            var trimStart = 65;
            while (trimStart < trimEnd && char.IsPunctuation(textInput[trimStart]))
            {
                trimStart++;
            }

            return textInput[trimStart..trimEnd];
        }
    }
    /// <summary>
    /// Checks if we can use this emote based on the emotes whitelist, blacklist, and availibility to the entity.
    /// </summary>
    /// <param name="source">The entity that is speaking</param>
    /// <param name="emote">The emote being used</param>
    /// <returns></returns>
    private bool AllowedToUseEmote(EntityUid source, EmotePrototype emote)
    {
        // If emote is in AllowedEmotes, it will bypass whitelist and blacklist
        if (TryComp<SpeechComponent>(source, out var speech) &&
            speech.AllowedEmotes.Contains(emote.ID))
        {
            return true;
        }

        // Check the whitelist and blacklist
        if (_whitelistSystem.IsWhitelistFail(emote.Whitelist, source) ||
            _whitelistSystem.IsBlacklistPass(emote.Blacklist, source))
        {
            return false;
        }

        // Check if the emote is available for all
        if (!emote.Available)
        {
            return false;
        }

        return true;
    }


    private void InvokeEmoteEvent(EntityUid uid, EmotePrototype proto, bool voluntary = false) // Goob - emotespam
    {
        var ev = new EmoteEvent(proto, voluntary); // Goob - emotespam
        RaiseLocalEvent(uid, ref ev, true); // goob edit
    }
}

/// <summary>
///     Raised by chat system when entity made some emote.
///     Use it to play sound, change sprite or something else.
/// </summary>
[ByRefEvent]
public struct EmoteEvent
{
    public bool Handled;
    public readonly EmotePrototype Emote;
    public bool Voluntary; // Goob - emotespam

    public EmoteEvent(EmotePrototype emote, bool voluntary = true) // Goob - emotespam
    {
        Emote = emote;
        Handled = false;
        Voluntary = voluntary; // Goob - emotespam
    }
}
