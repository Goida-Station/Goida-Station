// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.Components;

[RegisterComponent, NetworkedComponent]
[Access([])]
public sealed partial class AtmosAlertsDeviceComponent : Component
{
    /// <summary>
    /// The group that the entity belongs to
    /// </summary>
    [DataField, ViewVariables]
    public AtmosAlertsComputerGroup Group;
}