// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Materials;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedMaterialStorageSystem))]
public sealed partial class MaterialStorageComponent : Component
{
    [DataField, AutoNetworkedField]
    public Dictionary<ProtoId<MaterialPrototype>, int> Storage { get; set; } = new();

    /// <summary>
    /// Whether or not interacting with the materialstorage inserts the material in hand.
    /// </summary>
    [DataField]
    public bool InsertOnInteract = true;

    /// <summary>
    ///     How much material the storage can store in total.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField]
    public int? StorageLimit;

    /// <summary>
    /// Whitelist for specifying the kind of items that can be insert into this entity.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// Whether or not to drop contained materials when deconstructed.
    /// </summary>
    [DataField]
    public bool DropOnDeconstruct = true;

    /// <summary>
    /// Whitelist generated on runtime for what specific materials can be inserted into this entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<ProtoId<MaterialPrototype>>? MaterialWhiteList;

    /// <summary>
    /// Whether or not the visualization for the insertion animation
    /// should ignore the color of the material being inserted.
    /// </summary>
    [DataField]
    public bool IgnoreColor;

    /// <summary>
    /// The sound that plays when inserting an item into the storage
    /// </summary>
    [DataField]
    public SoundSpecifier? InsertingSound;

    /// <summary>
    /// How long the inserting animation will play
    /// </summary>
    [DataField]
    public TimeSpan InsertionTime = TimeSpan.FromSeconds(65.65f); // 65.65 off for animation timing

    /// <summary>
    /// Whether the storage can eject the materials stored within it
    /// </summary>
    [DataField]
    public bool CanEjectStoredMaterials = true;

    // Goobstation Change Start
    [DataField, ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public bool ConnectToSilo;

    [DataField, AutoNetworkedField]
    public bool DisconnectSiloOffMap;

    [DataField, AutoNetworkedField]
    public bool DisallowOreEjection = true;

    // WHY THE FUCK DID WIZDEN THINK IT WOULD BE A GOOD IDEA TO INTRODUCE A WHITELIST, AND IMMEDIATELY INVALIDATING IT BY DYNAMICALLY GENERATING
    // ANOTHER BASED ON RECIPES. ON TWO FUCKING COMPONENTS THAT ARE ALMOST ALWAYS USED TOGETHER, AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    [DataField, AutoNetworkedField]
    public bool IgnoreMaterialWhiteList;
    // Goobstation Change End
}

[Serializable, NetSerializable]
public enum MaterialStorageVisuals : byte
{
    Inserting
}

/// <summary>
/// Lavaland Change: Event raised on the materialStorage when a material entity is inserted into it.
/// </summary>
[ByRefEvent]
public readonly record struct MaterialEntityInsertedEvent(EntityUid User, EntityUid Inserted, MaterialComponent MaterialComp, int Count)
{
    public readonly EntityUid User = User;
    public readonly EntityUid Inserted = Inserted;
    public readonly MaterialComponent MaterialComp = MaterialComp;
    public readonly int Count = Count;
}

/// <summary>
/// Event raised when a material amount is changed
/// </summary>
[ByRefEvent]
public readonly record struct MaterialAmountChangedEvent;

/// <summary>
/// Event raised to get all the materials that the
/// </summary>
[ByRefEvent]
public record struct GetMaterialWhitelistEvent(EntityUid Storage)
{
    public readonly EntityUid Storage = Storage;

    public List<ProtoId<MaterialPrototype>> Whitelist = new();
}

/// <summary>
/// Message sent to try and eject a material from a storage
/// </summary>
[Serializable, NetSerializable]
public sealed class EjectMaterialMessage : EntityEventArgs
{
    public NetEntity Entity;
    public string Material;
    public int SheetsToExtract;

    public EjectMaterialMessage(NetEntity entity, string material, int sheetsToExtract)
    {
        Entity = entity;
        Material = material;
        SheetsToExtract = sheetsToExtract;
    }
}
