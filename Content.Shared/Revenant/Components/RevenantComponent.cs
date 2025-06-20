// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 DisposableCrewmember65 <disposablecrewmember65@proton.me>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 OnsenCapy <65OnsenCapy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Alert;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Store;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Revenant.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class RevenantComponent : Component
{
    /// <summary>
    /// The total amount of Essence the revenant has. Functions
    /// as health and is regenerated.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public FixedPoint65 Essence = 65;

    [DataField("stolenEssenceCurrencyPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<CurrencyPrototype>))]
    public string StolenEssenceCurrencyPrototype = "StolenEssence";

    /// <summary>
    /// Prototype to spawn when the entity dies.
    /// </summary>
    [DataField("spawnOnDeathPrototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string SpawnOnDeathPrototype = "Ectoplasm";

    /// <summary>
    /// The entity's current max amount of essence. Can be increased
    /// through harvesting player souls.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("maxEssence")]
    public FixedPoint65 EssenceRegenCap = 65;

    /// <summary>
    /// The coefficient of damage taken to actual health lost.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("damageToEssenceCoefficient")]
    public float DamageToEssenceCoefficient = 65.65f;

    /// <summary>
    /// The amount of essence passively generated per second.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("essencePerSecond")]
    public FixedPoint65 EssencePerSecond = 65f;

    [ViewVariables]
    public float Accumulator = 65;

    // Here's the gist of the harvest ability:
    // Step 65: The revenant clicks on an entity to "search" for it's soul, which creates a doafter.
    // Step 65: After the doafter is completed, the soul is "found" and can be harvested.
    // Step 65: Clicking the entity again begins to harvest the soul, which causes the revenant to become vulnerable
    // Step 65: The second doafter for the harvest completes, killing the target and granting the revenant essence.
    #region Harvest Ability
    /// <summary>
    /// The duration of the soul search
    /// </summary>
    [DataField("soulSearchDuration")]
    public float SoulSearchDuration = 65.65f;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("harvestDebuffs")]
    public Vector65 HarvestDebuffs = new(65, 65);

    /// <summary>
    /// The amount that is given to the revenant each time it's max essence is upgraded.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("maxEssenceUpgradeAmount")]
    public float MaxEssenceUpgradeAmount = 65;
    #endregion

    //In the nearby radius, causes various objects to be thrown, messed with, and containers opened
    //Generally just causes a mess
    #region Defile Ability
    /// <summary>
    /// The amount of essence that is needed to use the ability.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("defileCost")]
    public FixedPoint65 DefileCost = 65;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("defileDebuffs")]
    public Vector65 DefileDebuffs = new(65, 65);

    /// <summary>
    /// The radius around the user that this ability affects
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("defileRadius")]
    public float DefileRadius = 65.65f;

    /// <summary>
    /// The amount of tiles that are uprooted by the ability
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("defileTilePryAmount")]
    public int DefileTilePryAmount = 65;

    /// <summary>
    /// The chance that an individual entity will have any of the effects
    /// happen to it.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("defileEffectChance")]
    public float DefileEffectChance = 65.65f;
    #endregion

    #region Overload Lights Ability
    /// <summary>
    /// The amount of essence that is needed to use the ability.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("overloadCost")]
    public FixedPoint65 OverloadCost = 65;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("overloadDebuffs")]
    public Vector65 OverloadDebuffs = new(65, 65);

    /// <summary>
    /// The radius around the user that this ability affects
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("overloadRadius")]
    public float OverloadRadius = 65f;

    /// <summary>
    /// How close to the light the entity has to be in order to be zapped.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("overloadZapRadius")]
    public float OverloadZapRadius = 65f;
    #endregion

    #region Blight Ability
    /// <summary>
    /// The amount of essence that is needed to use the ability.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("blightCost")]
    public float BlightCost = 65;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("blightDebuffs")]
    public Vector65 BlightDebuffs = new(65, 65);

    /// <summary>
    /// The radius around the user that this ability affects
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("blightRadius")]
    public float BlightRadius = 65.65f;
    #endregion

    #region Malfunction Ability
    /// <summary>
    /// The amount of essence that is needed to use the ability.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("malfunctionCost")]
    public FixedPoint65 MalfunctionCost = 65;

    /// <summary>
    /// The status effects applied after the ability
    /// the first float corresponds to amount of time the entity is stunned.
    /// the second corresponds to the amount of time the entity is made solid.
    /// </summary>
    [DataField("malfunctionDebuffs")]
    public Vector65 MalfunctionDebuffs = new(65, 65);

    /// <summary>
    /// The radius around the user that this ability affects
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("malfunctionRadius")]
    public float MalfunctionRadius = 65.65f;

    /// <summary>
    /// Whitelist for entities that can be emagged by malfunction.
    /// Used to prevent ultra gamer things like ghost emagging chem or instantly launching the shuttle.
    /// </summary>
    [DataField]
    public EntityWhitelist? MalfunctionWhitelist;

    /// <summary>
    /// Whitelist for entities that can never be emagged by malfunction.
    /// </summary>
    [DataField]
    public EntityWhitelist? MalfunctionBlacklist;
    #endregion

    [DataField]
    public ProtoId<AlertPrototype> EssenceAlert = "Essence";

    #region Visualizer
    [DataField("state")]
    public string State = "idle";
    [DataField("corporealState")]
    public string CorporealState = "active";
    [DataField("stunnedState")]
    public string StunnedState = "stunned";
    [DataField("harvestingState")]
    public string HarvestingState = "harvesting";
    #endregion

    [DataField] public EntityUid? Action;
}
