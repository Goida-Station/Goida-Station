// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Factory;

/// <summary>
/// Component added to machines with <see cref="AutomationSlotsComponent"/> to enable their ports for linking.
/// They can then be automated with things like a <see cref="RoboticArmComponent"/>.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class AutomatedComponent : Component;
