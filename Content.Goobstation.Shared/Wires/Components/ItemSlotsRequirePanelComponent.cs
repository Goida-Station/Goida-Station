// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Wires.Components;

/// <summary>
/// This is used for items slots that require entity to have wire panel for interactions
/// </summary>

[RegisterComponent]
[NetworkedComponent]
public sealed partial class ItemSlotsRequirePanelComponent : Component
{
    /// <summary>
    /// For each slot: true - slot require opened panel for interaction, false - slot require closed panel for interaction
    /// </summary>
    [DataField]
    public Dictionary<string, bool> Slots = new();
}