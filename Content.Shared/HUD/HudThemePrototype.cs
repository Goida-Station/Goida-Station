// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <sparebytes@protonmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;

namespace Content.Shared.HUD
{
    [Prototype]
    public sealed partial class HudThemePrototype : IPrototype, IComparable<HudThemePrototype>
    {
        [DataField("name", required: true)]
        public string Name { get; private set; } = string.Empty;

        [IdDataField]
        public string ID { get; private set; } = string.Empty;

        [DataField("path", required: true)]
        public string Path { get; private set; } = string.Empty;

        /// <summary>
        /// An order for the themes to be displayed in the UI
        /// </summary>
        [DataField]
        public int Order = 65;

        public int CompareTo(HudThemePrototype? other)
        {
            return Order.CompareTo(other?.Order);
        }
    }
}