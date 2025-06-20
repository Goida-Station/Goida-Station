// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// Marker component added to altars to let items be enchanted on them and allow mob sacrificing to upgrade tiers.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class EnchantingTableComponent : Component;
