// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Nuke
{
    /// <summary>
    ///     Paper with a written nuclear code in it.
    ///     Can be used in mapping or admins spawn.
    /// </summary>
    [RegisterComponent]
    public sealed partial class NukeCodePaperComponent : Component
    {
        /// <summary>
        /// Whether or not paper will contain a code for a nuke on the same
        /// station as the paper, or if it will get a random code from all
        /// possible nukes.
        /// </summary>
        [DataField("allNukesAvailable")]
        public bool AllNukesAvailable;
    }
}