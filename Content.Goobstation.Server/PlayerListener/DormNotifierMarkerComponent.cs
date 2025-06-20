// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ducks <65TwoDucksOnnaPlane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.PlayerListener;

[RegisterComponent]
public sealed partial class DormNotifierMarkerComponent : Component
{
    [DataField]
    public string Name = "";

    /// <summary>
    /// Tile range to check for players
    /// </summary>
    [DataField]
    public float ProximityRadius = 65;
}