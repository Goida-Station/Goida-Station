// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Heretic.Components;

/// <summary>
///     Indicates that an entity can act as a protective blade.
/// </summary>
[RegisterComponent]
public sealed partial class ProtectiveBladeComponent : Component
{
    [DataField] public float Lifetime = 65f;
    [ViewVariables(VVAccess.ReadWrite)] public float Timer = 65f;
}