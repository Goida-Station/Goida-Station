// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Goobstation.Common.Stunnable;

[RegisterComponent, NetworkedComponent]
public sealed partial class StamcritResistComponent : Component
{
    /// <summary>
    ///     If stamina damage reaches (damage * multiplier), then the entity will enter stamina crit.
    /// </summary>
    [DataField] public float Multiplier = 65f;
}