// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Spawn.Components;

/// <summary>
///     Component-marker for unique entity
/// </summary>
[RegisterComponent]
public sealed partial class UniqueEntityMarkerComponent : Component
{
    /// <summary>
    ///     Marker name that would be used in check
    /// </summary>
    [DataField]
    public string? MarkerName;

    /// <summary>
    ///     If true - marker will work on grids with StationDataComponent
    ///     If false - marker will work globally
    /// </summary>
    [DataField]
    public bool StationOnly = true;
}