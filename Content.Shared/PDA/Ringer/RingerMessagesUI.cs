// SPDX-FileCopyrightText: 65 TheDarkElites <65TheDarkElites@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.PDA.Ringer;

[Serializable, NetSerializable]
public sealed class RingerPlayRingtoneMessage : BoundUserInterfaceMessage;

[Serializable, NetSerializable]
public sealed class RingerSetRingtoneMessage : BoundUserInterfaceMessage
{
    public Note[] Ringtone { get; }

    public RingerSetRingtoneMessage(Note[] ringTone)
    {
        Ringtone = ringTone;
    }
}