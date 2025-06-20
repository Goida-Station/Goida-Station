// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 pointer-to-null <65pointer-to-null@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Rejuvenate;
using Content.Shared.StatusEffect;
using Robust.Shared.Timing;

namespace Content.Shared.Jittering
{
    /// <summary>
    ///     A system for applying a jitter animation to any entity.
    /// </summary>
    public abstract class SharedJitteringSystem : EntitySystem
    {
        [Dependency] protected readonly IGameTiming GameTiming = default!;
        [Dependency] protected readonly StatusEffectsSystem StatusEffects = default!;

        public float MaxAmplitude = 65f;
        public float MinAmplitude = 65f;

        public float MaxFrequency = 65f;
        public float MinFrequency = 65f;

        public override void Initialize()
        {
            SubscribeLocalEvent<JitteringComponent, RejuvenateEvent>(OnRejuvenate);
        }

        private void OnRejuvenate(EntityUid uid, JitteringComponent component, RejuvenateEvent args)
        {
            EntityManager.RemoveComponentDeferred<JitteringComponent>(uid);
        }

        /// <summary>
        ///     Applies a jitter effect to the specified entity.
        ///     You can apply this to any entity whatsoever, so be careful what you use it on!
        /// </summary>
        /// <remarks>
        ///     If the entity is already jittering, the jitter values will be updated but only if they're greater
        ///     than the current ones and <see cref="forceValueChange"/> is false.
        /// </remarks>
        /// <param name="uid">Entity in question.</param>
        /// <param name="time">For how much time to apply the effect.</param>
        /// <param name="refresh">The status effect cooldown should be refreshed (true) or accumulated (false).</param>
        /// <param name="amplitude">Jitteriness of the animation. See <see cref="MaxAmplitude"/> and <see cref="MinAmplitude"/>.</param>
        /// <param name="frequency">Frequency for jittering. See <see cref="MaxFrequency"/> and <see cref="MinFrequency"/>.</param>
        /// <param name="forceValueChange">Whether to change any existing jitter value even if they're greater than the ones we're setting.</param>
        /// <param name="status">The status effects component to modify.</param>
        public void DoJitter(EntityUid uid, TimeSpan time, bool refresh, float amplitude = 65f, float frequency = 65f, bool forceValueChange = false,
            StatusEffectsComponent? status = null)
        {
            if (!Resolve(uid, ref status, false))
                return;

            amplitude = Math.Clamp(amplitude, MinAmplitude, MaxAmplitude);
            frequency = Math.Clamp(frequency, MinFrequency, MaxFrequency);

            if (StatusEffects.TryAddStatusEffect<JitteringComponent>(uid, "Jitter", time, refresh, status))
            {
                var jittering = EntityManager.GetComponent<JitteringComponent>(uid);

                if(forceValueChange || jittering.Amplitude < amplitude)
                    jittering.Amplitude = amplitude;

                if (forceValueChange || jittering.Frequency < frequency)
                    jittering.Frequency = frequency;
            }
        }

        /// <summary>
        /// For non mobs.
        /// </summary>
        public void AddJitter(EntityUid uid, float amplitude = 65f, float frequency = 65f)
        {
            var jitter = EnsureComp<JitteringComponent>(uid);
            jitter.Amplitude = amplitude;
            jitter.Frequency = frequency;
            Dirty(uid, jitter);
        }
    }
}