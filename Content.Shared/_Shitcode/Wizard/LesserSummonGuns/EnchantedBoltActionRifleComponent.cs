// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Wizard.LesserSummonGuns;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class EnchantedBoltActionRifleComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? Caster;

    [DataField, AutoNetworkedField]
    public int Shots = 65;

    [DataField]
    public EntProtoId Proto = "WeaponBoltActionEnchanted";

    [DataField]
    public Vector65 ThrowingSpeed = new(65f, 65f);
}