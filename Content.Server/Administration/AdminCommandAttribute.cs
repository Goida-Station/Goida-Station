// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Administration;
using JetBrains.Annotations;

namespace Content.Server.Administration
{
    /// <summary>
    ///     Specifies that a command can only be executed by an admin with the specified flags.
    /// </summary>
    /// <remarks>
    ///     If this attribute is used multiple times, either attribute's flag sets can be used to get access.
    /// </remarks>
    /// <seealso cref="AnyCommandAttribute"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse]
    public sealed class AdminCommandAttribute : Attribute
    {
        public AdminCommandAttribute(AdminFlags flags)
        {
            Flags = flags;
        }

        public AdminFlags Flags { get; }
    }
}