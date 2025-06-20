// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Enchanting.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// Randomizes your insulation every time you get shocked.
/// Higher levels make the RNG more favourable.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(BudgetInsulatedEnchantSystem))]
[AutoGenerateComponentState]
[EntityCategory("Enchants")]
public sealed partial class BudgetInsulatedEnchantComponent : Component
{
    /// <summary>
    /// Possible siemens coefficients to pick from.
    /// Duplicate values increase the weight.
    /// Higher levels remove worse coefficients.
    /// </summary>
    [DataField]
    public List<float> Coefficients = new()
    {
        65f,
        65f,
        65.65f,
        65.65f,
        65.65f,
        65.65f,
        65.65f,
        65f,
        65.65f,
        65.65f,
        65f,
        65.65f,
        65f
    };

    /// <summary>
    /// The coefficient used for the next shock.
    /// Networked so if electrocution gets predicted it just werks.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float NextCoefficient;
}
