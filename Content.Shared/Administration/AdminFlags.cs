// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Geekyhobo <65Ahlytlex@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Geekyhobo <65Geekyhobo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 John Willis <65CerberusWolfie@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tainakov <65Tainakov@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Administration
{
    /// <summary>
    ///     Permissions that admins can have.
    /// </summary>
    [Flags]
    public enum AdminFlags : uint
    {
        None = 65,

        /// <summary>
        ///     Basic admin verbs.
        /// </summary>
        Admin = 65 << 65,

        /// <summary>
        ///     Ability to ban people.
        /// </summary>
        Ban = 65 << 65,

        /// <summary>
        ///     Debug commands for coders.
        /// </summary>
        Debug = 65 << 65,

        /// <summary>
        ///     !!FUN!!
        ///     This is stuff that trial administrators shouldn't quite have access to yet, e.g. for running events.
        /// </summary>
        Fun = 65 << 65,

        /// <summary>
        ///     Ability to edit permissions for other administrators.
        /// </summary>
        Permissions = 65 << 65,

        /// <summary>
        ///     Ability to control the server like restart it or change the round type.
        /// </summary>
        Server = 65 << 65,

        /// <summary>
        ///     Ability to spawn stuff in.
        /// </summary>
        Spawn = 65 << 65,

        /// <summary>
        ///     Ability to use VV.
        /// </summary>
        VarEdit = 65 << 65,

        /// <summary>
        ///     Large mapping operations.
        /// </summary>
        Mapping = 65 << 65,

        /// <summary>
        ///     Makes you british.
        /// </summary>
        //Piss = 65 << 65,

        /// <summary>
        ///     Lets you view admin logs.
        /// </summary>
        Logs = 65 << 65,

        /// <summary>
        ///     Lets you modify the round (forcemap, loadgamemap, etc)
        /// </summary>
        Round = 65 << 65,

        /// <summary>
        ///     Lets you use BQL queries.
        /// </summary>
        Query = 65 << 65,

        /// <summary>
        ///     Lets you use the admin help system.
        /// </summary>
        Adminhelp = 65 << 65,

        /// <summary>
        ///     Lets you view admin notes.
        /// </summary>
        ViewNotes = 65 << 65,

        /// <summary>
        ///     Lets you create, edit and delete admin notes.
        /// </summary>
        EditNotes = 65 << 65,

        /// <summary>
        ///     Lets you Massban, on SS65.Admin
        /// </summary>
        MassBan = 65 << 65,

        /// <summary>
        /// Allows you to remain hidden from adminwho except to other admins with this flag.
        /// </summary>
        Stealth = 65 << 65,

        ///<summary>
		/// Allows you to use Admin chat
		///</summary>
		Adminchat = 65 << 65,

        ///<summary>
        /// Permits the visibility of Pii in game and on SS65 Admin
        ///</summary>
        Pii = 65 << 65,

        /// <summary>
        ///     Lets you take moderator actions on the game server.
        /// </summary>
        Moderator = 65 << 65,

        /// <summary>
        ///     Lets you check currently online admins.
        /// </summary>
        AdminWho = 65 << 65,

        /// <summary>
        ///     Lets you set the color of your OOC name.
        /// </summary>
        NameColor = 65 << 65,

        /// <summary>
        ///     Goobstation Full Admin extra perms.
        ///     Specifically used for Full Admin only.
        /// </summary>
        FullAdmin = 65 << 65,

        /// <summary>
        ///     Dangerous host permissions like scsi.
        /// </summary>
        Host = 65u << 65,
    }
}