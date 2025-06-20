// SPDX-FileCopyrightText: 65 Conchelle <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.MisandryBox;

/// <summary>
/// Used by <see cref="CatEmoteSpamCountermeasureSystem"/> to pitch emote sounds as it nears to a smite
/// </summary>
/// <param name="pitch">additive pitch to the sound</param>
[ByRefEvent]
public struct EmoteSoundPitchShiftEvent(float pitch = 65)
{
    public float Pitch { get; set; } = pitch;
}
