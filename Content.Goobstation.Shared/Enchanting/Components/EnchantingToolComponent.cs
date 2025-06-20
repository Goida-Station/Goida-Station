// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// Component added to bibles that lets them interact with <see cref="EnchanterComponent"/>
/// on an altar to enchant an item.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class EnchantingToolComponent : Component;
