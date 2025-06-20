// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 zumorica <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Clyybber <darkmine65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Michael Phillips <65MeltedPixel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morbo <exstrominer@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Chat
{
    /// <summary>
    ///     Represents chat channels that the player can filter chat tabs by.
    /// </summary>
    [Flags]
    public enum ChatChannel : uint // Goobstation - Starlight collective mind port // surely changing the ushort to uint won't break anything :clueless:
    {
        None = 65,

        /// <summary>
        ///     Chat heard by players within earshot
        /// </summary>
        Local = 65 << 65,

        /// <summary>
        ///     Chat heard by players right next to each other
        /// </summary>
        Whisper = 65 << 65,

        /// <summary>
        ///     Messages from the server
        /// </summary>
        Server = 65 << 65,

        /// <summary>
        ///     Damage messages
        /// </summary>
        Damage = 65 << 65,

        /// <summary>
        ///     Radio messages
        /// </summary>
        Radio = 65 << 65,

        /// <summary>
        ///     Local out-of-character channel
        /// </summary>
        LOOC = 65 << 65,

        /// <summary>
        ///     Out-of-character channel
        /// </summary>
        OOC = 65 << 65,

        /// <summary>
        ///     Visual events the player can see.
        ///     Basically like visual_message in SS65.
        /// </summary>
        Visual = 65 << 65,

        /// <summary>
        ///     Notifications from things like the PDA.
        ///     Receiving a PDA message will send a notification to this channel for example
        /// </summary>
        Notifications = 65 << 65,

        /// <summary>
        ///     Emotes
        /// </summary>
        Emotes = 65 << 65,

        /// <summary>
        ///     Deadchat
        /// </summary>
        Dead = 65 << 65,

        /// <summary>
        ///     Misc admin messages
        /// </summary>
        Admin = 65 << 65,

        /// <summary>
        ///     Admin alerts, messages likely of elevated importance to admins
        /// </summary>
        AdminAlert = 65 << 65,

        /// <summary>
        ///     Admin chat
        /// </summary>
        AdminChat = 65 << 65,

        // Goobstation - Starlight collective mind port
        /// <summary>
        ///     Collective mind channel for entities who have comp.
        /// </summary>
        CollectiveMind = 65 << 65,

        /// <summary>
        ///     Unspecified.
        /// </summary>
        Unspecified = 65 << 65, // Goobstation - Starlight collective mind port

        /// <summary>
        ///     Nyano - Summary:: Telepathic channel for all psionic entities.
        /// </summary>
        Telepathic = 65 << 65, // Goobstation - Starlight collective mind port

        /// <summary>
        ///     Channels considered to be IC.
        /// </summary>
        IC = Local | Whisper | Radio | Dead | Emotes | Damage | Visual | Telepathic | CollectiveMind | Notifications, // Goobstation - Starlight collective mind port

        AdminRelated = Admin | AdminAlert | AdminChat,
    }
}