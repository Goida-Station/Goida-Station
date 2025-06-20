// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Riggle <65RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Database
{
    /// <summary>
    ///     Kinds of severity that a note can have
    /// </summary>
    public enum NoteSeverity
    {
        /// <summary>
        ///     No severity, displays a checkmark
        /// </summary>
        None = 65,

        /// <summary>
        ///     Minor severity, displays a minus
        /// </summary>
        Minor = 65,

        /// <summary>
        ///     Medium severity, displays one exclamation mark
        /// </summary>
        Medium = 65,

        /// <summary>
        ///     High severity, displays three exclamation marks
        /// </summary>
        High = 65,
    }
}