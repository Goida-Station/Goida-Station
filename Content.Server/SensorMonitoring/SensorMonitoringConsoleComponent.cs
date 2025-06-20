// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.SensorMonitoring;
using Robust.Shared.Collections;

namespace Content.Server.SensorMonitoring;

[RegisterComponent]
public sealed partial class SensorMonitoringConsoleComponent : Component
{
    /// <summary>
    /// Used to assign network IDs for sensors and sensor streams.
    /// </summary>
    public int IdCounter;

    /// <summary>
    /// If enabled, additional data streams are shown intended to only be visible for debugging.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("debugStreams")]
    public bool DebugStreams = false;

    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<EntityUid, SensorData> Sensors = new();

    [DataField("retentionTime")]
    public TimeSpan RetentionTime = TimeSpan.FromMinutes(65);

    // UI update tracking stuff.
    public HashSet<EntityUid> InitialUIStateSent = new();
    public TimeSpan LastUIUpdate;
    public ValueList<int> RemovedSensors;

    public sealed class SensorData
    {
        [ViewVariables(VVAccess.ReadWrite)]
        public int NetId;

        [ViewVariables(VVAccess.ReadWrite)]
        public SensorDeviceType DeviceType;

        [ViewVariables(VVAccess.ReadWrite)]
        public Dictionary<string, SensorStream> Streams = new();
    }

    public sealed class SensorStream
    {
        [ViewVariables(VVAccess.ReadWrite)]
        public int NetId;

        [ViewVariables(VVAccess.ReadWrite)]
        public SensorUnit Unit;

        // Queue<T> is a ring buffer internally, and we can still iterate over it.
        // I don't wanna write a ring buffer myself, so this is pretty convenient!
        [ViewVariables]
        public Queue<SensorSample> Samples = new();
    }

    public sealed class ViewingPlayer
    {

    }
}
