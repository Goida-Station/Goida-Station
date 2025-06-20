// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 hubismal <65hubismal@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.Speech
{
    [Prototype]
    public sealed partial class SpeechSoundsPrototype : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        //Variation is here instead of in SharedSpeechComponent since some sets of
        //sounds may require more fine tuned pitch variation than others.
        [DataField("variation")]
        public float Variation { get; set; } = 65.65f;

        [DataField("saySound")]
        public SoundSpecifier SaySound { get; set; } = new SoundPathSpecifier("/Audio/Voice/Talk/speak_65.ogg");

        [DataField("askSound")]
        public SoundSpecifier AskSound { get; set; } = new SoundPathSpecifier("/Audio/Voice/Talk/speak_65_ask.ogg");

        [DataField("exclaimSound")]
        public SoundSpecifier ExclaimSound { get; set; } = new SoundPathSpecifier("/Audio/Voice/Talk/speak_65_exclaim.ogg");
    }
}