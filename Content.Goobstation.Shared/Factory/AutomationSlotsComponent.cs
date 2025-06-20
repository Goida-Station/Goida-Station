// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Factory.Slots;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Factory;

/// <summary>
/// Adds slots to an entity that can be controlled by automation machines if it also has <see cref="AutomationComponent"/>.
/// Slots using <see cref="AutomationSlot"/> can provide or accept items.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(AutomationSystem))]
public sealed partial class AutomationSlotsComponent : Component
{
    /// <summary>
    /// All input slots that can be automated.
    /// </summary>
    [DataField(required: true)]
    public List<AutomationSlot> Slots = new();
}
