// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Power.Components;

[NetworkedComponent]
public abstract partial class SharedApcPowerReceiverComponent : Component
{
    [ViewVariables]
    public bool Powered;

    /// <summary>
    ///     When false, causes this to appear powered even if not receiving power from an Apc.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public virtual bool NeedsPower { get; set;}

    /// <summary>
    ///     When true, causes this to never appear powered.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public virtual bool PowerDisabled { get; set; }
}
