// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._DV.Carrying;

/// <summary>
/// Stores the carrier of an entity being carried.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(CarryingSystem))]
[AutoGenerateComponentState]
public sealed partial class BeingCarriedComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid Carrier;
}