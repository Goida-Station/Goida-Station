// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.DeviceNetwork.Systems;

namespace Content.Server.DeviceNetwork.Components;

[RegisterComponent]
[Access(typeof(SingletonDeviceNetServerSystem))]
public sealed partial class SingletonDeviceNetServerComponent : Component
{
    /// <summary>
    /// Whether the server can become the currently active server. The server being unavailable usually means that it isn't powered
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Available = true;

    /// <summary>
    /// Whether the server is the currently active server for the station it's on
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Active = true;
}