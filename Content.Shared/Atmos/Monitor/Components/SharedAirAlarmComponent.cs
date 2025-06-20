// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 vulppine <vulppine@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 c65llv65e <65c65llv65e@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Monitor.Components;

[Serializable, NetSerializable]
public enum SharedAirAlarmInterfaceKey
{
    Key
}

[Serializable, NetSerializable]
public enum AirAlarmMode
{
    None,
    Filtering,
    WideFiltering,
    Fill,
    Panic,
}

[Serializable, NetSerializable]
public enum AirAlarmWireStatus
{
    Power,
    Access,
    Panic,
    DeviceSync
}

public interface IAtmosDeviceData
{
    public bool Enabled { get; set; }
    public bool Dirty { get; set; }
    public bool IgnoreAlarms { get; set; }
}

[Serializable, NetSerializable]
public sealed class AirAlarmUIState : BoundUserInterfaceState
{
    public AirAlarmUIState(string address, int deviceCount, float pressureAverage, float temperatureAverage, List<(string, IAtmosDeviceData)> deviceData, AirAlarmMode mode, AtmosAlarmType alarmType, bool autoMode)
    {
        Address = address;
        DeviceCount = deviceCount;
        PressureAverage = pressureAverage;
        TemperatureAverage = temperatureAverage;
        DeviceData = deviceData;
        Mode = mode;
        AlarmType = alarmType;
        AutoMode = autoMode;
    }

    public string Address { get; }
    public int DeviceCount { get; }
    public float PressureAverage { get; }
    public float TemperatureAverage { get; }
    /// <summary>
    ///     Every single device data that can be seen from this
    ///     air alarm. This includes vents, scrubbers, and sensors.
    ///     Each entry is a tuple of device address and the device
    ///     data. The same address may appear multiple times, if
    ///     that device provides multiple functions.
    /// </summary>
    public List<(string, IAtmosDeviceData)> DeviceData { get; }
    public AirAlarmMode Mode { get; }
    public AtmosAlarmType AlarmType { get; }
    public bool AutoMode { get; }
}

[Serializable, NetSerializable]
public sealed class AirAlarmResyncAllDevicesMessage : BoundUserInterfaceMessage
{}

[Serializable, NetSerializable]
public sealed class AirAlarmUpdateAlarmModeMessage : BoundUserInterfaceMessage
{
    public AirAlarmMode Mode { get; }

    public AirAlarmUpdateAlarmModeMessage(AirAlarmMode mode)
    {
        Mode = mode;
    }
}

[Serializable, NetSerializable]
public sealed class AirAlarmUpdateAutoModeMessage : BoundUserInterfaceMessage
{
    public bool Enabled { get; }

    public AirAlarmUpdateAutoModeMessage(bool enabled)
    {
        Enabled = enabled;
    }
}

[Serializable, NetSerializable]
public sealed class AirAlarmUpdateDeviceDataMessage : BoundUserInterfaceMessage
{
    public string Address { get; }
    public IAtmosDeviceData Data { get; }

    public AirAlarmUpdateDeviceDataMessage(string addr, IAtmosDeviceData data)
    {
        Address = addr;
        Data = data;
    }
}

[Serializable, NetSerializable]
public sealed class AirAlarmCopyDeviceDataMessage : BoundUserInterfaceMessage
{
    public IAtmosDeviceData Data { get; }

    public AirAlarmCopyDeviceDataMessage(IAtmosDeviceData data)
    {
        Data = data;
    }
}

[Serializable, NetSerializable]
public sealed class AirAlarmUpdateAlarmThresholdMessage : BoundUserInterfaceMessage
{
    public string Address { get; }
    public AtmosAlarmThreshold Threshold { get; }
    public AtmosMonitorThresholdType Type { get; }
    public Gas? Gas { get; }

    public AirAlarmUpdateAlarmThresholdMessage(string address, AtmosMonitorThresholdType type, AtmosAlarmThreshold threshold, Gas? gas = null)
    {
        Address = address;
        Threshold = threshold;
        Type = type;
        Gas = gas;
    }
}