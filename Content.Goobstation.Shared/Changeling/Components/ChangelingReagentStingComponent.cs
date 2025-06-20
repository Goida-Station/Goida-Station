// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Weapons.AmmoSelector;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Changeling.Components;

[RegisterComponent]
public sealed partial class ChangelingReagentStingComponent : Component
{
    [DataField(required: true)]
    public ProtoId<ReagentStingConfigurationPrototype> Configuration;

    [DataField]
    public ProtoId<SelectableAmmoPrototype>? DartGunAmmo;
}