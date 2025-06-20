// SPDX-FileCopyrightText: 65 Ashley Woodiss-Field <ash@DESKTOP-H65M65AI.localdomain>
// SPDX-FileCopyrightText: 65 ColesMagnum <65AW-FulCode@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Common.Weapons.NoWieldNeeded;

/// <summary>
/// Indicates that this gun user does not need to wield.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class NoWieldNeededComponent: Component
{
    //If true, not only does the user not need to wield to fire, they get the bonus for free!
    [DataField]
    public bool GetBonus = true;

    public List<EntityUid> GunsWithBonus = [];
}
