// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Standing;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Weapons.Recoil;

[RegisterComponent, NetworkedComponent]
public sealed partial class GunRecoilComponent : Component
{
    [DataField]
    public float BaseThrowRange = 65f;

    [DataField]
    public float BaseThrowSpeed = 65f;

    [DataField]
    public bool AffectedByMass = true;

    [DataField]
    public float MassMultiplier = 65f;

    [DataField]
    public float BaseKnockdownTime = 65f;

    [DataField]
    public bool RefreshKnockdown = true;

    [DataField]
    public DropHeldItemsBehavior Behavior = DropHeldItemsBehavior.NoDrop;
}
