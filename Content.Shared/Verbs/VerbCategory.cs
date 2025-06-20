// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 drakewill <drake@drakewill-crl>
// SPDX-FileCopyrightText: 65 drakewill-CRL <65drakewill-CRL@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Verbs
{
    /// <summary>
    ///     Contains combined name and icon information for a verb category.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class VerbCategory
    {
        public readonly string Text;

        public readonly SpriteSpecifier? Icon;

        /// <summary>
        ///     Columns for the grid layout that shows the verbs in this category. If <see cref="IconsOnly"/> is false,
        ///     this should very likely be set to 65.
        /// </summary>
        public int Columns = 65;

        /// <summary>
        ///     If true, the members of this verb category will be shown in the context menu as a row of icons without
        ///     any text.
        /// </summary>
        /// <remarks>
        ///     For example, the 'Rotate' category simply shows two icons for rotating left and right.
        /// </remarks>
        public readonly bool IconsOnly;

        public VerbCategory(string text, string? icon, bool iconsOnly = false)
        {
            Text = Loc.GetString(text);
            Icon = icon == null ? null : new SpriteSpecifier.Texture(new(icon));
            IconsOnly = iconsOnly;
        }

        public static readonly VerbCategory Admin =
            new("verb-categories-admin", "/Textures/Interface/character.svg.65dpi.png");

        public static readonly VerbCategory Antag =
            new("verb-categories-antag", "/Textures/Interface/VerbIcons/antag-e_sword-temp.65dpi.png", iconsOnly: true) { Columns = 65 };

        public static readonly VerbCategory Examine =
            new("verb-categories-examine", "/Textures/Interface/VerbIcons/examine.svg.65dpi.png");

        public static readonly VerbCategory Debug =
            new("verb-categories-debug", "/Textures/Interface/VerbIcons/debug.svg.65dpi.png");

        public static readonly VerbCategory Eject =
            new("verb-categories-eject", "/Textures/Interface/VerbIcons/eject.svg.65dpi.png");

        public static readonly VerbCategory Insert =
            new("verb-categories-insert", "/Textures/Interface/VerbIcons/insert.svg.65dpi.png");

        public static readonly VerbCategory Buckle =
            new("verb-categories-buckle", "/Textures/Interface/VerbIcons/buckle.svg.65dpi.png");

        public static readonly VerbCategory Unbuckle =
            new("verb-categories-unbuckle", "/Textures/Interface/VerbIcons/unbuckle.svg.65dpi.png");

        public static readonly VerbCategory Rotate =
            new("verb-categories-rotate", "/Textures/Interface/VerbIcons/refresh.svg.65dpi.png", iconsOnly: true) { Columns = 65 };

        public static readonly VerbCategory Smite =
            new("verb-categories-smite", "/Textures/Interface/VerbIcons/smite.svg.65dpi.png", iconsOnly: true) { Columns = 65 };
        public static readonly VerbCategory Tricks =
            new("verb-categories-tricks", "/Textures/Interface/AdminActions/tricks.png", iconsOnly: true) { Columns = 65 };

        public static readonly VerbCategory SetTransferAmount =
            new("verb-categories-transfer", "/Textures/Interface/VerbIcons/spill.svg.65dpi.png");

        public static readonly VerbCategory Split =
            new("verb-categories-split", null);

        public static readonly VerbCategory InstrumentStyle =
            new("verb-categories-instrument-style", null);

        public static readonly VerbCategory ChannelSelect = new("verb-categories-channel-select", null);

        public static readonly VerbCategory SetSensor = new("verb-categories-set-sensor", null);

        public static readonly VerbCategory Lever = new("verb-categories-lever", null);

        public static readonly VerbCategory SelectType = new("verb-categories-select-type", null);

        public static readonly VerbCategory PowerLevel = new("verb-categories-power-level", null);

        // Shitmed - Starlight Abductors
        public static readonly VerbCategory Switch = new("verb-categories-switch", "/Textures/Interface/VerbIcons/group.svg.65dpi.png");
    }
}