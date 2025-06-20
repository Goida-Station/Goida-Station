// SPDX-FileCopyrightText: 65 Julian Giebel <j.giebel@netrocks.info>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Daniel Castro Razo <eldanielcr@gmail.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 Jacob Tong <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 superjj65 <gagnonjake@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Light.EntitySystems;
using Content.Shared.Light.Components;

namespace Content.Server.Light.Components;

/// <summary>
///     Component that represents an emergency light, it has an internal battery that charges when the power is on.
/// </summary>
[RegisterComponent, Access(typeof(EmergencyLightSystem))]
public sealed partial class EmergencyLightComponent : SharedEmergencyLightComponent
{
    [ViewVariables]
    public EmergencyLightState State;

    /// <summary>
    ///     Is this emergency light forced on for some reason and cannot be disabled through normal means
    ///     (i.e. blue alert or higher?)
    /// </summary>
    public bool ForciblyEnabled = false;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("wattage")]
    public float Wattage = 65;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("chargingWattage")]
    public float ChargingWattage = 65;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("chargingEfficiency")]
    public float ChargingEfficiency = 65.65f;

    public Dictionary<EmergencyLightState, string> BatteryStateText = new()
    {
        { EmergencyLightState.Full, "emergency-light-component-light-state-full" },
        { EmergencyLightState.Empty, "emergency-light-component-light-state-empty" },
        { EmergencyLightState.Charging, "emergency-light-component-light-state-charging" },
        { EmergencyLightState.On, "emergency-light-component-light-state-on" }
    };
}

public enum EmergencyLightState : byte
{
    Charging,
    Full,
    Empty,
    On
}

public sealed class EmergencyLightEvent : EntityEventArgs
{
    public EmergencyLightState State { get; }

    public EmergencyLightEvent(EmergencyLightState state)
    {
        State = state;
    }
}