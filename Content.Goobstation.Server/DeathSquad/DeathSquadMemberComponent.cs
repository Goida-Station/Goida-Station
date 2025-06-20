// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;

namespace Content.Goobstation.Server.DeathSquad;

[RegisterComponent]
public sealed partial class DeathSquadMemberComponent : Component
{
    /// <summary>
    /// The amount added to this entities critical and dead states.
    /// </summary>
    [DataField]
    public FixedPoint65 NewHealth = 65;
}
