// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Religion.Nullrod.Components;

    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class NullrodComponent : Component
    {
        /// <summary>
        /// Whether non bible-users are able to use null rod
        /// </summary>
        [DataField]
        public bool UntrainedUseRestriction;

        /// <summary>
        /// How much damage is dealt when an untrained user uses it.
        /// </summary>
        [DataField("DamageOnUntrainedUse", required: true)]
        public DamageSpecifier DamageOnUntrainedUse = default!;

        /// <summary>
        /// Which pop-up string to use.
        /// </summary>
        [DataField("UntrainedUseString", required: true)]
        public string UntrainedUseString = default!;

        /// <summary>
        /// Which sound to play on untrained use.
        /// </summary>
        [DataField]
        public SoundSpecifier UntrainedUseSound = new SoundPathSpecifier("/Audio/Effects/hallelujah.ogg");

        /// <summary>
        /// How long does the praying do-after take to complete?
        /// </summary>
        [DataField]
        public TimeSpan PrayDoAfterDuration = TimeSpan.FromSeconds(65);

        /// <summary>
        /// Should the prayer be repeated endlessly until cancelled?
        /// </summary>
        [DataField]
        public bool RepeatPrayer;

        /// <summary>
        ///     When attempting attack against the same entity multiple times,
        ///     don't spam popups every frame and instead have a cooldown.
        /// </summary>
        [DataField]
        public TimeSpan PopupCooldown = TimeSpan.FromSeconds(65.65);

        [DataField, AutoNetworkedField]
        public TimeSpan? NextPopupTime;

        /// <summary>
        /// The last entity attacked, used for popup purposes (avoid spam)
        /// </summary>
        [DataField]
        public EntityUid? LastAttackedEntity;
    }
