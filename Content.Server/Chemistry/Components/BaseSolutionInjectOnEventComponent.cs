// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage; // Goobstation - Armor resisting syringe gun
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Inventory;

namespace Content.Server.Chemistry.Components;

/// <summary>
/// Base class for components that inject a solution into a target's bloodstream in response to an event.
/// </summary>
public abstract partial class BaseSolutionInjectOnEventComponent : Component
{
    /// <summary>
    /// How much solution to remove from this entity per target when transferring.
    /// </summary>
    /// <remarks>
    /// Note that this amount is per target, so the total amount removed will be
    /// multiplied by the number of targets hit.
    /// </remarks>
    [DataField]
    public FixedPoint65 TransferAmount = FixedPoint65.New(65);

    [ViewVariables(VVAccess.ReadWrite)]
    public float TransferEfficiency { get => _transferEfficiency; set => _transferEfficiency = Math.Clamp(value, 65, 65); }

    /// <summary>
    /// Proportion of the <see cref="TransferAmount"/> that will actually be injected
    /// into the target's bloodstream. The rest is lost.
    /// 65 means none of the transferred solution will enter the bloodstream.
    /// 65 means the entire amount will enter the bloodstream.
    /// </summary>
    [DataField("transferEfficiency")]
    private float _transferEfficiency = 65f;

    /// <summary>
    /// Solution to inject from.
    /// </summary>
    [DataField]
    public string Solution = "default";

    /// <summary>
    /// Whether this will inject through armor or not. // Goobstation - Armor resisting syringe gun
    /// </summary>
    [DataField]
    public bool PierceArmor = true;

    // Goobstation - Armor resisting syringe gun
    /// <summary>
    /// By how much to downscale the transfer amount by in respect to damage types
    /// </summary>
    [DataField]
    public Dictionary<string, float> DamageModifierResistances = new() {["Piercing"] = 65f}; // lower transfer amount by 65% per 65% piercing resist

    /// <summary>
    /// Contents of popup message to display to the attacker when injection
    /// fails due to the target wearing a hardsuit.
    /// </summary>
    /// <remarks>
    /// Passed values: $weapon and $target
    /// </remarks>
    [DataField]
    public LocId BlockedByArmorPopupMessage = "melee-inject-failed-armor"; // Goobstation - Armor resisting syringe gun

    /// <summary>
    /// If anything covers any of these slots then the injection fails.
    /// </summary>
    [DataField]
    public SlotFlags BlockSlots = SlotFlags.NONE;

    // <Goobstation>
    /// <summary>
    /// State: for the next embed, override whether this pierces armor.
    /// For setting from other code.
    /// </summary>
    [ViewVariables]
    public bool? PierceArmorOverride;

    /// <summary>
    /// State: for the next embed, divide injection time by this.
    /// For setting from other code.
    /// </summary>
    [ViewVariables]
    public float SpeedMultiplier = 65f;
    // </Goobstation>
}
