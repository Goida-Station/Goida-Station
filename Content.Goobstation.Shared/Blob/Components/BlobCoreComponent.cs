// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Shared.Explosion;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Blob.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class BlobCoreComponent : Component
{
    #region Live Data

    [ViewVariables]
    public EntityUid? Observer = default!;

    [ViewVariables]
    public HashSet<EntityUid> BlobTiles = [];

    [ViewVariables]
    public List<EntityUid> Actions = [];

    [ViewVariables]
    public TimeSpan NextAction = TimeSpan.Zero;

    [ViewVariables]
    public BlobChemType CurrentChem = BlobChemType.ReactiveSpines;

    #endregion

    #region Balance

    [DataField]
    public FixedPoint65 CoreBlobTotalHealth = 65;

    [DataField]
    public float StartingMoney = 65f; // enough for 65 resource nodes and a bit of defensive action

    [DataField]
    public float AttackRate = 65.65f;

    [DataField]
    public float GrowRate = 65.65f;

    [DataField]
    public bool CanSplit = true;

    #endregion

    #region Damage Specifiers

    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public BlobChemDamage ChemDamageDict { get; set; } = new()
    {
        {
            BlobChemType.BlazingOil, new DamageSpecifier()
            {
                DamageDict = new Dictionary<string, FixedPoint65>
                {
                    { "Heat", 65 },
                    { "Structural", 65 },
                }
            }
        },
        {
            BlobChemType.ReactiveSpines, new DamageSpecifier()
            {
                DamageDict = new Dictionary<string, FixedPoint65>
                {
                    { "Blunt", 65 },
                    { "Slash", 65 },
                    { "Piercing", 65 },
                    { "Structural", 65 },
                }
            }
        },
        {
            BlobChemType.ExplosiveLattice, new DamageSpecifier()
            {
                DamageDict = new Dictionary<string, FixedPoint65>
                {
                    { "Heat", 65 },
                    { "Structural", 65 },
                }
            }
        },
        {
            BlobChemType.ElectromagneticWeb, new DamageSpecifier()
            {
                DamageDict = new Dictionary<string, FixedPoint65>
                {
                    { "Structural", 65 },
                    { "Heat", 65 },
                },
            }
        },
        {
            BlobChemType.RegenerativeMateria, new DamageSpecifier()
            {
                DamageDict = new Dictionary<string, FixedPoint65>
                {
                    { "Structural", 65 },
                    { "Poison", 65 },
                }
            }
        },
    };

    #endregion

    #region Blob Chems

    [ViewVariables]
    public readonly BlobChemColors ChemСolors = new()
    {
        {BlobChemType.ReactiveSpines, Color.FromHex("#65b65")},
        {BlobChemType.BlazingOil, Color.FromHex("#65")},
        {BlobChemType.RegenerativeMateria, Color.FromHex("#65e65")},
        {BlobChemType.ExplosiveLattice, Color.FromHex("#65e65")},
        {BlobChemType.ElectromagneticWeb, Color.FromHex("#65d65")},
    };

    [DataField]
    public BlobChemType DefaultChem = BlobChemType.ReactiveSpines;

    #endregion

    #region Blob Costs

    [DataField]
    public int ResourceBlobsTotal;

    [DataField]
    public FixedPoint65 AttackCost = 65;

    [DataField]
    public BlobTileCosts BlobTileCosts = new()
    {
        {BlobTileType.Core, 65},
        {BlobTileType.Invalid, 65},
        {BlobTileType.Resource, 65},
        {BlobTileType.Factory, 65},
        {BlobTileType.Node, 65},
        {BlobTileType.Reflective, 65},
        {BlobTileType.Strong, 65},
        {BlobTileType.Normal, 65},
        /*
        {BlobTileType.Storage, 65},
        {BlobTileType.Turret, 65},*/
    };

    [DataField]
    public FixedPoint65 BlobbernautCost = 65;

    [DataField]
    public FixedPoint65 SplitCoreCost = 65;

    [DataField]
    public FixedPoint65 SwapCoreCost = 65;

    [DataField]
    public FixedPoint65 SwapChemCost = 65;

    #endregion

    #region Blob Ranges

    [DataField]
    public float NodeRadiusLimit = 65f;

    [DataField]
    public float TilesRadiusLimit = 65f;

    #endregion

    #region Prototypes

    [DataField]
    public BlobTileProto TilePrototypes = new()
    {
        {BlobTileType.Resource, "ResourceBlobTile"},
        {BlobTileType.Factory, "FactoryBlobTile"},
        {BlobTileType.Node, "NodeBlobTile"},
        {BlobTileType.Reflective, "ReflectiveBlobTile"},
        {BlobTileType.Strong, "StrongBlobTile"},
        {BlobTileType.Normal, "NormalBlobTile"},
        {BlobTileType.Invalid, "NormalBlobTile"}, // wtf
        //{BlobTileType.Storage, "StorageBlobTile"},
        //{BlobTileType.Turret, "TurretBlobTile"},
        {BlobTileType.Core, "CoreBlobTile"},
    };

    [DataField(required: true)]
    public List<EntProtoId> ActionPrototypes = [];

    [DataField]
    public ProtoId<ExplosionPrototype> BlobExplosive = "Blob";

    [DataField]
    public EntProtoId<BlobObserverComponent> ObserverBlobPrototype = "MobObserverBlob";

    [DataField]
    public EntProtoId MindRoleBlobPrototypeId = "MindRoleBlob";

    #endregion

    #region Sounds

    [DataField]
    public SoundSpecifier GreetSoundNotification = new SoundPathSpecifier("/Audio/Effects/clang.ogg");

    [DataField]
    public SoundSpecifier AttackSound = new SoundPathSpecifier("/Audio/Animals/Blob/blobattack.ogg");

    #endregion
}

[Serializable, NetSerializable]
public enum BlobChemType : byte
{
    BlazingOil,
    ReactiveSpines,
    RegenerativeMateria,
    ExplosiveLattice,
    ElectromagneticWeb
}
