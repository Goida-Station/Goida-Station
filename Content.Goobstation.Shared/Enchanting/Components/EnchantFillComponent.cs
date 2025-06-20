// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Enchanting.Systems;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// Gives an entity enchants by default.
/// </summary>
[RegisterComponent, Access(typeof(EnchantFillSystem))]
public sealed partial class EnchantFillComponent : Component
{
    /// <summary>
    /// Dictionary of enchant ids and the level to assign.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<EntProtoId<EnchantComponent>, int> Enchants = new();
}
