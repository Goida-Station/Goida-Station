// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Scribbles65 <65Scribbles65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 brainfood65 <65brainfood65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Abilities.Mime;
using Content.Server.Chat.Systems;
using Content.Server.Popups;
using Content.Server.Speech.EntitySystems;
using Content.Shared.Chat.Prototypes;
using Content.Shared.Puppet;
using Content.Shared.Speech;
using Content.Shared.Speech.Muting;

namespace Content.Server.Speech.Muting
{
    public sealed class MutingSystem : EntitySystem
    {
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<MutedComponent, SpeakAttemptEvent>(OnSpeakAttempt);
            SubscribeLocalEvent<MutedComponent, EmoteEvent>(OnEmote, before: new[] { typeof(VocalSystem) });
            SubscribeLocalEvent<MutedComponent, ScreamActionEvent>(OnScreamAction, before: new[] { typeof(VocalSystem) });
        }

        private void OnEmote(EntityUid uid, MutedComponent component, ref EmoteEvent args)
        {
            if (args.Handled)
                return;

            //still leaves the text so it looks like they are pantomiming a laugh
            if (args.Emote.Category.HasFlag(EmoteCategory.Vocal))
                args.Handled = true;
        }

        private void OnScreamAction(EntityUid uid, MutedComponent component, ScreamActionEvent args)
        {
            if (args.Handled)
                return;

            if (HasComp<MimePowersComponent>(uid))
                _popupSystem.PopupEntity(Loc.GetString("mime-cant-speak"), uid, uid);

            else
                _popupSystem.PopupEntity(Loc.GetString("speech-muted"), uid, uid);
            args.Handled = true;
        }


        private void OnSpeakAttempt(EntityUid uid, MutedComponent component, SpeakAttemptEvent args)
        {
            // TODO something better than this.

            if (HasComp<MimePowersComponent>(uid))
                _popupSystem.PopupEntity(Loc.GetString("mime-cant-speak"), uid, uid);
            else if (HasComp<VentriloquistPuppetComponent>(uid))
                _popupSystem.PopupEntity(Loc.GetString("ventriloquist-puppet-cant-speak"), uid, uid);
            else
                _popupSystem.PopupEntity(Loc.GetString("speech-muted"), uid, uid);

            args.Cancel();
        }
    }
}