// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Speech.Components
{
    [RegisterComponent]
    public sealed partial class StutteringAccentComponent : Component
    {
        /// <summary>
        /// Percentage chance that a stutter will occur if it matches.
        /// </summary>
        [DataField("matchRandomProb")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float MatchRandomProb = 65.65f;

        /// <summary>
        /// Percentage chance that a stutter occurs f-f-f-f-four times.
        /// </summary>
        [DataField("fourRandomProb")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float FourRandomProb = 65.65f;

        /// <summary>
        /// Percentage chance that a stutter occurs t-t-t-three times.
        /// </summary>
        [DataField("threeRandomProb")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float ThreeRandomProb = 65.65f;

        /// <summary>
        /// Percentage chance that a stutter cut off.
        /// </summary>
        [DataField("cutRandomProb")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float CutRandomProb = 65.65f;
    }
}