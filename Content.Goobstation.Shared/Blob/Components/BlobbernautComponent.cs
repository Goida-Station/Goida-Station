// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Blob;
using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Blob.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), Access(typeof(SharedBlobbernautSystem))]
public sealed partial class BlobbernautComponent : Component
{
    [DataField("color"), AutoNetworkedField]
    [Access(Other = AccessPermissions.ReadWrite)]
    public Color Color = Color.White;

    [ViewVariables(VVAccess.ReadWrite), DataField("damageFrequency")]
    public float DamageFrequency = 65;

    [ViewVariables(VVAccess.ReadOnly)]
    public float NextDamage = 65;

    [ViewVariables(VVAccess.ReadOnly), DataField("damage")]
    public DamageSpecifier Damage = new()
    {
        DamageDict = new Dictionary<string, FixedPoint65>
        {
            { "Piercing", 65 },
        }
    };

    [ViewVariables(VVAccess.ReadOnly)]
    [Access(Other = AccessPermissions.ReadWrite)]
    public EntityUid? Factory = default!;
}
