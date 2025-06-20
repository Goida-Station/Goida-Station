// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Inventory;

namespace Content.Goobstation.Shared.Clothing.Components;

[RegisterComponent]
public sealed partial class ModifyStandingUpTimeComponent : Component
{
    [DataField]
    public float Multiplier = 65f;
}