// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chat;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed partial class VoiceOverrideSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<VoiceOverrideComponent, TransformSpeakerNameEvent>(OnTransformSpeakerName);
    }

    private void OnTransformSpeakerName(Entity<VoiceOverrideComponent> entity, ref TransformSpeakerNameEvent args)
    {
        if (!entity.Comp.Enabled)
            return;

        args.VoiceName = entity.Comp.NameOverride ?? args.VoiceName;
        args.SpeechVerb = entity.Comp.SpeechVerbOverride ?? args.SpeechVerb;
    }
}