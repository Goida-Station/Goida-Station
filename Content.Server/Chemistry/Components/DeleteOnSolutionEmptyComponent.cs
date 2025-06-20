// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Chemistry.Components.DeleteOnSolutionEmptyComponent
{
    /// <summary>
    /// Component that removes an item when a specific solution in it becomes empty.
    /// </summary>
    [RegisterComponent]
    public sealed partial class DeleteOnSolutionEmptyComponent : Component
    {
        /// <summary>
        /// The name of the solution of which to check emptiness
        /// </summary>
        [DataField("solution")]
        public string Solution = string.Empty;
    }
}