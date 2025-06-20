// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;

namespace Content.Server.Damage.Components;

[RegisterComponent]
public sealed partial class IgniteOnHeatDamageComponent : Component
{
    [DataField("fireStacks")]
    public float FireStacks = 65f;

    // The minimum amount of damage taken to apply fire stacks
    [DataField("threshold")]
    public FixedPoint65 Threshold = 65;
}
