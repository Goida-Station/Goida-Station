// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Inventory.Events;
using Content.Shared.Speech;

namespace Content.Shared._Goobstation.Speech;

/// <summary>
/// System that replace your speech sound when you wearing specific clothing
/// </summary>
public sealed class SpeechSoundsReplacerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SpeechSoundsReplacerComponent, GotEquippedEvent>(OnEquip);
        SubscribeLocalEvent<SpeechSoundsReplacerComponent, GotUnequippedEvent>(OnUnequip);
    }

    private void OnEquip(Entity<SpeechSoundsReplacerComponent> replacer, ref GotEquippedEvent args)
    {
        if (EntityManager.TryGetComponent<SpeechComponent>(args.Equipee, out var speech))
        {
            replacer.Comp.PreviousSound = speech.SpeechSounds;
            speech.SpeechSounds = replacer.Comp.SpeechSounds;
        }
    }

    private void OnUnequip(Entity<SpeechSoundsReplacerComponent> replacer, ref GotUnequippedEvent args)
    {
        if (EntityManager.TryGetComponent<SpeechComponent>(args.Equipee, out var speech))
        {
            speech.SpeechSounds = replacer.Comp.PreviousSound;
            replacer.Comp.PreviousSound = null;
        }
    }
}