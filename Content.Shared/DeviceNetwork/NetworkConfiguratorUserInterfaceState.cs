// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.DeviceNetwork;

[Serializable, NetSerializable]
public sealed class NetworkConfiguratorUserInterfaceState : BoundUserInterfaceState
{
    public readonly HashSet<(string address, string name)> DeviceList;

    public NetworkConfiguratorUserInterfaceState(HashSet<(string, string)> deviceList)
    {
        DeviceList = deviceList;
    }
}

[Serializable, NetSerializable]
public sealed class DeviceListUserInterfaceState : BoundUserInterfaceState
{
    public readonly HashSet<(string address, string name)> DeviceList;

    public DeviceListUserInterfaceState(HashSet<(string address, string name)> deviceList)
    {
        DeviceList = deviceList;
    }
}

[Serializable, NetSerializable]
public sealed class DeviceLinkUserInterfaceState : BoundUserInterfaceState
{
    public readonly List<SourcePortPrototype> Sources;
    public readonly List<SinkPortPrototype> Sinks;
    public readonly HashSet<(ProtoId<SourcePortPrototype> source, ProtoId<SinkPortPrototype> sink)> Links;
    public readonly List<(string source, string sink)>? Defaults;
    public readonly string SourceAddress;
    public readonly string SinkAddress;

    public DeviceLinkUserInterfaceState(
        List<SourcePortPrototype> sources,
        List<SinkPortPrototype> sinks,
        HashSet<(ProtoId<SourcePortPrototype> source, ProtoId<SinkPortPrototype> sink)> links,
        string sourceAddress,
        string sinkAddress,
        List<(string source, string sink)>? defaults = default)
    {
        Links = links;
        SourceAddress = sourceAddress;
        SinkAddress = sinkAddress;
        Defaults = defaults;
        Sources = sources;
        Sinks = sinks;
    }
}