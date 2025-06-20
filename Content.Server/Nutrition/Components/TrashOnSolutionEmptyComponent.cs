// SPDX-FileCopyrightText: 65 Jessica M <jessica@jessicamaybe.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Nutrition.Components
{
    /// <summary>
    /// Component that tags solution containers as trash when their contents have been emptied.
    /// Used for things like used ketchup packets or used syringes.
    /// </summary>
    [RegisterComponent]
    public sealed partial class TrashOnSolutionEmptyComponent : Component
    {
        /// <summary>
        /// The name of the solution of which to check emptiness
        /// </summary>
        [DataField("solution")]
        public string Solution { get; set; } = string.Empty;
    }
}