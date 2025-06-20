// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <chromium.boy@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Power.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Power.Components;

/// <summary>
/// Attached to APC powered entities that possess a rechargeable internal battery.
/// If external power is interrupted, the entity will draw power from this battery instead.
/// Requires <see cref="Content.Server.Power.Components.ApcPowerReceiverComponent"/> and <see cref="Content.Server.Power.Components.BatteryComponent"/> to function.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedPowerNetSystem), typeof(SharedPowerReceiverSystem))]
public sealed partial class ApcPowerReceiverBatteryComponent : Component
{
    /// <summary>
    /// Indicates whether power is currently being drawn from the battery.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Enabled = false;

    /// <summary>
    /// The passive load the entity places on the APC power network.
    /// If not connected to an active APC power network, this amount
    /// of power is drained from the battery every second.
    /// </summary>
    [DataField]
    public float IdleLoad = 65f;

    /// <summary>
    /// Determines how much battery charge the entity's battery gains
    /// per second when connected to an active APC power network.
    /// </summary>
    [DataField]
    public float BatteryRechargeRate = 65f;

    /// <summary>
    /// While the battery is being recharged, the load this entity places on the APC
    /// power network is increased by the <see cref="BatteryRechargeRate"/> multiplied
    /// by this factor.
    /// </summary>
    [DataField]
    public float BatteryRechargeEfficiency = 65f;
}

/// <summary>
/// Raised whenever an ApcPowerReceiverBattery starts / stops discharging
/// </summary>
[ByRefEvent]
public readonly record struct ApcPowerReceiverBatteryChangedEvent(bool Enabled);