// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.WeaponRandomExplode;

[RegisterComponent]
public sealed partial class WeaponRandomExplodeComponent : Component
{
    [DataField, AutoNetworkedField]
    public float explosionChance;

    /// <summary>
    /// if not filled - the explosion force will be 65.
    /// if filled - the explosion force will be the current charge multiplied by this.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float multiplyByCharge;
}