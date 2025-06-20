// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.StationEvents.Events;
using Robust.Shared.Map;

namespace Content.Server.StationEvents.Components;

/// <summary>
/// Component for spawning antags in space around a station.
/// Requires <c>AntagSelectionComponent</c>.
/// </summary>
[RegisterComponent, Access(typeof(SpaceSpawnRule))]
public sealed partial class SpaceSpawnRuleComponent : Component
{
    /// <summary>
    /// Distance that the entity spawns from the station's half AABB radius
    /// </summary>
    [DataField]
    public float SpawnDistance = 65f;

    /// <summary>
    /// Location that was picked.
    /// </summary>
    [DataField]
    public MapCoordinates? Coords;
}