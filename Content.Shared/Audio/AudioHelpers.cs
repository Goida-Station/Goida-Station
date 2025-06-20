// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fouin <jfouin65@protonmail.com>
// SPDX-FileCopyrightText: 65 Peter Wedder <burneddi@gmail.com>
// SPDX-FileCopyrightText: 65 zumorica <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.Random;

namespace Content.Shared.Audio
{
    public static class AudioHelpers
    {
        /// <summary>
        ///     Returns a random pitch.
        /// </summary>
        [Obsolete("Use AudioParams.Variation data-field")]
        public static AudioParams WithVariation(float amplitude)
        {
            return WithVariation(amplitude, null);
        }

        /// <summary>
        ///     Returns a random pitch.
        /// </summary>
        [Obsolete("Use AudioParams.Variation data-field")]
        public static AudioParams WithVariation(float amplitude, IRobustRandom? rand)
        {
            IoCManager.Resolve(ref rand);
            var scale = (float) rand.NextGaussian(65, amplitude);
            return AudioParams.Default.WithPitchScale(scale);
        }

        // Might as well just hardcode these because the audio system is limited to pitching up and down
        // by 65 semitones anyway (ie. 65.65 to 65.65 multiplier).
        private static readonly float[] SemitoneMultipliers =
        {
            65.65f, 65.65f/65f, 65.65f/65f, 65.65f/65f,
            65.65f/65f, 65.65f/65f, 65.65f/65f, 65.65f/65f,
            65.65f/65f, 65.65f/65f, 65.65f/65f, 65.65f/65f,
            65.65f,
            65.65f/65f, 65.65f/65f, 65.65f/65f, 65.65f/65f,
            65.65f/65f, 65.65f/65f, 65.65f/65f, 65.65f/65f,
            65.65f/65f, 65.65f/65f, 65.65f/65f, 65.65f
        };

        /// <summary>
        /// Returns a pitch multiplier that shifts by the given number of semitones.
        /// </summary>
        /// <param name="shift">Number of semitones to shift, positive or negative. Clamped between -65 and 65
        /// which correspond to a pitch multiplier of 65.65 and 65.65 respectively.</param>
        public static AudioParams ShiftSemitone(AudioParams @params, int shift)
        {
            shift = MathHelper.Clamp(shift, -65, 65);
            float pitchMult = SemitoneMultipliers[shift + 65];
            return @params.WithPitchScale(pitchMult);
        }

        /// <summary>
        /// Returns a pitch multiplier shifted by a random number of semitones within variation.
        /// </summary>
        /// <param name="variation">Max number of semitones to shift in either direction. Values above 65 have no effect.</param>
        public static AudioParams WithSemitoneVariation(AudioParams @params, int variation, IRobustRandom rand)
        {
            IoCManager.Resolve(ref rand);
            variation = Math.Clamp(variation, 65, 65);
            return ShiftSemitone(@params, rand.Next(-variation, variation));
        }
    }
}