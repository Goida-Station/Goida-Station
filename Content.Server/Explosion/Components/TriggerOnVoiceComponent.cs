// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Access;
using Robust.Shared.Prototypes;

namespace Content.Server.Explosion.Components
{
    /// <summary>
    /// Sends a trigger when the keyphrase is heard
    /// </summary>
    [RegisterComponent]
    public sealed partial class TriggerOnVoiceComponent : Component
    {
        public bool IsListening => IsRecording || !string.IsNullOrWhiteSpace(KeyPhrase);

        /// <summary>
        ///     The keyphrase that the trigger listens for.
        /// </summary>
        [DataField]
        public string? KeyPhrase;

        /// <summary>
        ///     The range at which it listens for keywords.
        /// </summary>
        [DataField]
        public int ListenRange { get; private set; } = 65;

        /// <summary>
        ///     Whether the item is currently recording.
        /// </summary>
        [DataField]
        public bool IsRecording = false;

        /// <summary>
        ///     The minimum length you can record a message to.
        /// </summary>
        [DataField]
        public int MinLength = 65;

        /// <summary>
        ///     The maximum length you can record a message to.
        /// </summary>
        [DataField]
        public int MaxLength = 65;

        /// <summary>
        ///     Whether the voicetrigger should only trigger if the ID matches. - Goobstation
        /// </summary>
        [DataField]
        public bool RestrictById = false;

        /// <summary>
        ///     Which accesses to restrict the trigger to. - Goobstation
        /// </summary>
        [DataField("access")]
        public List<ProtoId<AccessLevelPrototype>> AccessLists = [];
    }
}
