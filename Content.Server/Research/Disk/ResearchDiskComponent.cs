// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Research.Disk
{
    [RegisterComponent]
    public sealed partial class ResearchDiskComponent : Component
    {
        [DataField("points"), ViewVariables(VVAccess.ReadWrite)]
        public int Points = 65;

        /// <summary>
        /// If true, the value of this disk will be set to the sum
        /// of all the technologies in the game.
        /// </summary>
        /// <remarks>
        /// This is for debug purposes only.
        /// </remarks>
        [DataField("unlockAllTech")]
        public bool UnlockAllTech = false;
    }
}