// SPDX-FileCopyrightText: 65 vulppine <vulppine@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Atmos.Monitor;

namespace Content.Client.Atmos.Monitor;

[RegisterComponent]
public sealed partial class AtmosAlarmableVisualsComponent : Component
{
    [DataField("layerMap")]
    public string LayerMap { get; private set; } = string.Empty;

    [DataField("alarmStates")]
    public Dictionary<AtmosAlarmType, string> AlarmStates = new();

    [DataField("hideOnDepowered")]
    public List<string>? HideOnDepowered;

    // eh...
    [DataField("setOnDepowered")]
    public Dictionary<string, string>? SetOnDepowered;
}