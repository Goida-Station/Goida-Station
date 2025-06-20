// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Enchanting.Components;

/// <summary>
/// Component checked for by EnchanterSystem before enchanting.
/// Place this on any entity you want to allow to enchant... e.g. Chaplain, Heretic or Wizard.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CanEnchantComponent : Component;
