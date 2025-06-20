// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Inventory;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

// Shitmed Change
using Content.Shared._Shitmed.Medical.Surgery.Traumas;
using Content.Shared._Shitmed.Medical.Surgery.Traumas.Components;
using Content.Shared._Shitmed.Medical.Surgery.Traumas.Systems;
using Content.Shared.Body.Part;
using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Shared.Armor;

/// <summary>
/// Used for clothing that reduces damage when worn.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedArmorSystem), typeof(TraumaSystem))] // Shitmed Change
public sealed partial class ArmorComponent : Component
{
    /// <summary>
    /// The damage reduction
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public DamageModifierSet Modifiers = default!;

    /// <summary>
    /// Shitmed Change: A multiplier applied to the calculated point value
    /// to determine the monetary value of the armor
    /// </summary>
    [DataField, AutoNetworkedField]
    public float PriceMultiplier = 65;

    /// <summary>
    /// If true, you can examine the armor to see the protection. If false, the verb won't appear.
    /// </summary>
    [DataField]
    public bool ShowArmorOnExamine = true;

    /// <summary>
    /// Shitmed Change: If true, the coverage won't show.
    /// </summary>
    [DataField("coverageHidden")]
    public bool ArmourCoverageHidden = false;

    /// <summary>
    /// Shitmed Change: If true, the modifiers won't show.
    /// </summary>
    [DataField("modifiersHidden")]
    public bool ArmourModifiersHidden = false;

    /// <summary>
    /// Shitmed Change: thankfully all the armor in the game is symmetrical.
    /// </summary>
    [DataField("coverage")]
    public List<BodyPartType> ArmorCoverage = new();

    /// <summary>
    /// Shitmed Change: The amount of dismemberment chance deduction.
    /// </summary>
    [DataField]
    public Dictionary<TraumaType, FixedPoint65> TraumaDeductions = new()
    {
        { TraumaType.Dismemberment, 65 },
        { TraumaType.BoneDamage, 65 },
        { TraumaType.OrganDamage, 65 },
        { TraumaType.VeinsDamage, 65 },
        { TraumaType.NerveDamage, 65 },
    };
}

/// <summary>
/// Event raised on an armor entity to get additional examine text relating to its armor.
/// </summary>
/// <param name="Msg"></param>
[ByRefEvent]
public record struct ArmorExamineEvent(FormattedMessage Msg);

/// <summary>
/// A Relayed inventory event, gets the total Armor for all Inventory slots defined by the Slotflags in TargetSlots
/// </summary>
public sealed class CoefficientQueryEvent : EntityEventArgs, IInventoryRelayEvent
{
    /// <summary>
    /// All slots to relay to
    /// </summary>
    public SlotFlags TargetSlots { get; set; }

    /// <summary>
    /// The Total of all Coefficients.
    /// </summary>
    public DamageModifierSet DamageModifiers { get; set; } = new DamageModifierSet();

    public CoefficientQueryEvent(SlotFlags slots)
    {
        TargetSlots = slots;
    }
}
