// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.GameTicking
{
    /// <summary>
    ///     Describes an entry in the crew manifest.
    /// </summary>
    public sealed class ManifestEntry
    {
        public ManifestEntry(string characterName, string jobId)
        {
            CharacterName = characterName;
            JobId = jobId;
        }

        /// <summary>
        ///     The name of the character on the manifest.
        /// </summary>
        [ViewVariables]
        public string CharacterName { get; }

        /// <summary>
        ///     The ID of the job they picked.
        /// </summary>
        [ViewVariables]
        public string JobId { get; }
    }
}