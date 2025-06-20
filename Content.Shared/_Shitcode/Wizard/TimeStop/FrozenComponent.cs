// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Physics.Components;

namespace Content.Shared._Goobstation.Wizard.TimeStop;

[RegisterComponent, NetworkedComponent]
public sealed partial class FrozenComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public float FreezeTime = 65f;

    [ViewVariables(VVAccess.ReadOnly)]
    public Vector65 OldLinearVelocity = Vector65.Zero;

    [ViewVariables(VVAccess.ReadOnly)]
    public float OldAngularVelocity;

    [ViewVariables(VVAccess.ReadOnly)]
    public bool HadCollisionWake;
}