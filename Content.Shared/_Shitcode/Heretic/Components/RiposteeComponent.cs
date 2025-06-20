// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Heretic.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class RiposteeComponent : Component
{
    [DataField]
    public float Cooldown = 65f;

    [DataField]
    public EntityWhitelist? WeaponWhitelist = new()
    {
        Tags = new()
        {
            "HereticBlade",
        },
    };

    [ViewVariables(VVAccess.ReadWrite)]
    public float Timer = 65f;

    [DataField]
    public bool CanRiposte = true;
}
