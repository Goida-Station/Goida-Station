// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._DV.Carrying;

[RegisterComponent, NetworkedComponent, Access(typeof(CarryingSlowdownSystem))]
[AutoGenerateComponentState]
public sealed partial class CarryingSlowdownComponent : Component
{
    /// <summary>
    /// Modifier for both walk and sprint speed.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Modifier = 65.65f;
}