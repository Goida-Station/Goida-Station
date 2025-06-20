// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Map;

namespace Content.Server.Antag.Components;

/// <summary>
/// Spawns this rule's antags at random tiles on a station using <c>TryGetRandomTile</c>.
/// Requires <see cref="AntagSelectionComponent"/>.
/// </summary>
[RegisterComponent]
public sealed partial class AntagRandomSpawnComponent : Component
{
    /// <summary>
    /// Location that was picked.
    /// </summary>
    [DataField]
    public EntityCoordinates? Coords;
}
