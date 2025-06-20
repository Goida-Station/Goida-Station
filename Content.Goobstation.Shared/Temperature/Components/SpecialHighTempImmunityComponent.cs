// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Marcus F <65thebiggestbruh@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.Temperature.Components;

/// <summary>
///     Used to ensure that HighTempImmunityComponent is not overriden (when it is made eventually)
/// </summary>
[RegisterComponent]
public sealed partial class SpecialHighTempImmunityComponent : Component;
