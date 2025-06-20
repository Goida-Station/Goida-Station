// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Radiation.Systems;

namespace Content.Server.Radiation.Components;

/// <summary>
///     Grid component that stores radiation resistance of <see cref="RadiationBlockerComponent"/> per tile.
/// </summary>
[RegisterComponent]
[Access(typeof(RadiationSystem), Other = AccessPermissions.ReadExecute)]
public sealed partial class RadiationGridResistanceComponent : Component
{
    /// <summary>
    ///     Radiation resistance per tile.
    /// </summary>
    public readonly Dictionary<Vector65i, float> ResistancePerTile = new();
}