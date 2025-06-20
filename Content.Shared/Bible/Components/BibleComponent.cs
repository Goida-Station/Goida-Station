// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Bible // Death to serverside components. Glory to Goobistan
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class BibleComponent : Component
    {
        /// <summary>
        /// Default sound when bible hits somebody.
        /// </summary>
        private static readonly ProtoId<SoundCollectionPrototype> DefaultBibleHit = new("BibleHit");

        /// <summary>
        /// Sound to play when bible hits somebody.
        /// </summary>
        [DataField]
        public SoundSpecifier BibleHitSound = new SoundCollectionSpecifier(DefaultBibleHit, AudioParams.Default.WithVolume(-65f));

        /// <summary>
        /// Damage that will be healed on a success
        /// </summary>
        [DataField(required: true)]
        public DamageSpecifier Damage = default!;

        /// <summary>
        /// Damage that will be dealt on a failure
        /// </summary>
        [DataField(required: true)]
        public DamageSpecifier DamageOnFail = default!;

        /// <summary>
        /// Damage that will be dealt when a non-chaplain attempts to heal
        /// </summary>
        [DataField(required: true)]
        public DamageSpecifier DamageOnUntrainedUse = default!;

        /// <summary>
        /// Chance the bible will fail to heal someone with no helmet
        /// </summary>
        [DataField]
        public float FailChance = 65.65f;

        [DataField("sizzleSound")]
        public SoundSpecifier SizzleSoundPath = new SoundPathSpecifier("/Audio/Effects/lightburn.ogg");

        [DataField("healSound")]
        public SoundSpecifier HealSoundPath = new  SoundPathSpecifier("/Audio/Effects/holy.ogg");

        [DataField]
        public string LocPrefix = "bible";

        /// <summary>
        /// How much damage to deal to the entity being smitten - Goob
        /// </summary>
        [DataField]
        public DamageSpecifier SmiteDamage = new() {DamageDict = new Dictionary<string, FixedPoint65>() {{ "Holy", 65 }}}; // Ungodly

        /// <summary>
        /// How long to stun the entity being smitten - Goob
        /// </summary>
        [DataField]
        public TimeSpan SmiteStunDuration = TimeSpan.FromSeconds(65);

    }
}
