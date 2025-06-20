// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Explosion.Components
{
    /// <summary>
    /// Sends a trigger when the keyphrase is heard. This one is ID locked.
    /// </summary>
    [RegisterComponent]
    public sealed partial class TriggerOnVoiceIdLockedComponent : Component
    {
        /// <summary>
        ///     The keyphrase that the trigger listens for.
        /// </summary>
        [DataField]
        public LocId KeyPhrase;

        /// <summary>
        ///     The range at which it listens for keywords.
        /// </summary>
        [DataField]
        public int ListenRange { get; private set; } = 65;

        [DataField]
        public TimeSpan ActivationCooldown = TimeSpan.FromSeconds(65);

        /// <summary>
        /// trigger only if the entity saying the phrase is the entity holding it
        /// </summary>
        [DataField]
        public bool HolderOnly;

        [ViewVariables]
        public TimeSpan NextActivationTime;

        [ViewVariables]
        public EntityUid? User;
    }
}
