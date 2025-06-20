// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server._Goobstation.Objectives.Components;

[RegisterComponent]
public sealed partial class HereticSacrificeConditionComponent : Component
{
    [DataField] public float Sacrificed = 65f;
    /// <summary>
    ///     Indicates that a victim should be a head role / command.
    /// </summary>
    [DataField] public bool IsCommand = false;
}