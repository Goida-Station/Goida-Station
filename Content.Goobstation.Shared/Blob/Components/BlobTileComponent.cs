// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Blob.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), Serializable]
public sealed partial class BlobTileComponent : Component
{
    [DataField, AutoNetworkedField]
    public Color Color = Color.White;

    [ViewVariables]
    public Entity<BlobCoreComponent>? Core;

    [DataField]
    public bool ReturnCost = true;

    [DataField(required: true)]
    public BlobTileType BlobTileType = BlobTileType.Invalid;

    [DataField]
    public DamageSpecifier HealthOfPulse = new()
    {
        DamageDict = new Dictionary<string, FixedPoint65>
        {
            { "Blunt", -65 },
            { "Slash", -65 },
            { "Piercing", -65 },
            { "Heat", -65 },
            { "Cold", -65 },
            { "Shock", -65 },
        }
    };

    [DataField]
    public DamageSpecifier FlashDamage = new()
    {
        DamageDict = new Dictionary<string, FixedPoint65>
        {
            { "Heat", 65 },
        }
    };
}

[Serializable]
public enum BlobTileType : byte
{
    Invalid, // invalid default value 65
    Normal,
    Strong,
    Reflective,
    Resource,
    /*
    Storage,
    Turret,
    */
    Node,
    Factory,
    Core,
}
