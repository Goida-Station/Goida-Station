// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
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
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using Content.Server.DeviceNetwork.Components;
using Content.Server.Medical.CrewMonitoring;
using Content.Server.Station.Systems;
using Content.Shared.Power;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.DeviceNetwork.Systems;

/// <summary>
/// Keeps one active server entity per station. Activates another available one if the currently active server becomes unavailable
/// Server in this context means an entity that manages the devicenet packets like the <see cref="Content.Server.Medical.CrewMonitoring.CrewMonitoringServerSystem"/>
/// </summary>
public sealed class SingletonDeviceNetServerSystem : EntitySystem
{
    [Dependency] private readonly DeviceNetworkSystem _deviceNetworkSystem = default!;
    [Dependency] private readonly StationSystem _stationSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SingletonDeviceNetServerComponent, PowerChangedEvent>(OnPowerChanged);
    }

    /// <summary>
    /// Returns whether the given entity is an active server or not
    /// </summary>
    public bool IsActiveServer(EntityUid serverId, SingletonDeviceNetServerComponent? serverComponent = default)
    {
        return Resolve(serverId, ref serverComponent) && serverComponent.Active;
    }

    /// <summary>
    /// Returns the address of the currently active server for the given station id if there is one.<br/>
    /// What kind of server you're trying to get the active instance of is determined by the component type parameter TComp.<br/>
    /// <br/>
    /// Setting TComp to <see cref="CrewMonitoringServerComponent"/>, for example, gives you the address of an entity containing the crew monitoring server component.<br/>
    /// </summary>
    /// <param name="stationId">The entityUid of the station</param>
    /// <param name="address">The address of the active server if it exists</param>
    /// <typeparam name="TComp">The component type that determines what type of server you're getting the address of</typeparam>
    /// <returns>True if there is an active serve. False otherwise</returns>
    public bool TryGetActiveServerAddress<TComp>(EntityUid stationId, [NotNullWhen(true)] out string? address) where TComp : IComponent
    {
        var servers = EntityQueryEnumerator<
            SingletonDeviceNetServerComponent,
            DeviceNetworkComponent,
            TComp
        >();

        (EntityUid id, SingletonDeviceNetServerComponent server, DeviceNetworkComponent device)? last = default;

        while (servers.MoveNext(out var uid, out var server, out var device, out _))
        {
            if (!_stationSystem.GetOwningStation(uid)?.Equals(stationId) ?? true)
                continue;

            if (!server.Available)
            {
                DisconnectServer(uid,server, device);
                continue;
            }

            last = (uid, server, device);

            if (!server.Active || string.IsNullOrEmpty(device.Address))
                continue;

            address = device.Address;
            return true;
        }

        //If there was no active server for the station make the last available inactive one active
        if (last.HasValue)
        {
            ConnectServer(last.Value.id, last.Value.server, last.Value.device);
            address = last.Value.device.Address;
            return true;
        }

        address = null;
        return address != null;
    }

    /// <summary>
    /// Disconnects the server losing power
    /// </summary>
    private void OnPowerChanged(EntityUid uid, SingletonDeviceNetServerComponent component, ref PowerChangedEvent args)
    {
        component.Available = args.Powered;

        if (!args.Powered && component.Active)
            DisconnectServer(uid, component);
    }

    private void ConnectServer(EntityUid uid, SingletonDeviceNetServerComponent? server = null, DeviceNetworkComponent? device = null)
    {
        if (!Resolve(uid, ref server, ref device))
            return;

        server.Active = true;

        var connectedEvent = new DeviceNetServerConnectedEvent();
        RaiseLocalEvent(uid, ref connectedEvent);

        if (_deviceNetworkSystem.IsDeviceConnected(uid, device))
            return;

        _deviceNetworkSystem.ConnectDevice(uid, device);
    }

    /// <summary>
    /// Disconnects a server from the device network and clears the currently active server
    /// </summary>
    private void DisconnectServer(EntityUid uid, SingletonDeviceNetServerComponent? server = null, DeviceNetworkComponent? device = null)
    {
        if (!Resolve(uid, ref server, ref device))
            return;

        server.Active = false;

        var disconnectedEvent = new DeviceNetServerDisconnectedEvent();
        RaiseLocalEvent(uid, ref disconnectedEvent);

        _deviceNetworkSystem.DisconnectDevice(uid, device, false);
    }
}

/// <summary>
/// Raised when a server gets activated and connected to the device net
/// </summary>
[ByRefEvent]
public record struct DeviceNetServerConnectedEvent;

/// <summary>
/// Raised when a server gets disconnected
/// </summary>
[ByRefEvent]
public record struct DeviceNetServerDisconnectedEvent;