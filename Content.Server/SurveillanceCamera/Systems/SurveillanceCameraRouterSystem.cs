// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
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

using Content.Server.DeviceNetwork.Systems;
using Content.Shared.ActionBlocker;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.Power;
using Content.Shared.SurveillanceCamera;
using Content.Shared.Verbs;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.SurveillanceCamera;

public sealed class SurveillanceCameraRouterSystem : EntitySystem
{
    [Dependency] private readonly DeviceNetworkSystem _deviceNetworkSystem = default!;
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterface = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, ComponentInit>(OnInitialize);
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, DeviceNetworkPacketEvent>(OnPacketReceive);
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, SurveillanceCameraSetupSetNetwork>(OnSetNetwork);
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, GetVerbsEvent<AlternativeVerb>>(AddVerbs);
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, PowerChangedEvent>(OnPowerChanged);
    }

    private void OnInitialize(EntityUid uid, SurveillanceCameraRouterComponent router, ComponentInit args)
    {
        if (router.SubnetFrequencyId == null ||
            !_prototypeManager.TryIndex(router.SubnetFrequencyId, out DeviceFrequencyPrototype? subnetFrequency))
        {
            return;
        }

        router.SubnetFrequency = subnetFrequency.Frequency;
        router.Active = true;
    }

    private void OnPacketReceive(EntityUid uid, SurveillanceCameraRouterComponent router, DeviceNetworkPacketEvent args)
    {
        if (!router.Active
            || string.IsNullOrEmpty(args.SenderAddress)
            || !args.Data.TryGetValue(DeviceNetworkConstants.Command, out string? command))
        {
            return;
        }

        switch (command)
        {
            case SurveillanceCameraSystem.CameraConnectMessage:
                if (!args.Data.TryGetValue(SurveillanceCameraSystem.CameraAddressData, out string? address))
                {
                    return;
                }

                ConnectCamera(uid, args.SenderAddress, address, router);
                break;
            case SurveillanceCameraSystem.CameraHeartbeatMessage:
                if (!args.Data.TryGetValue(SurveillanceCameraSystem.CameraAddressData, out string? camera))
                {
                    return;
                }

                SendHeartbeat(uid, args.SenderAddress, camera, router);
                break;
            case SurveillanceCameraSystem.CameraSubnetConnectMessage:
                AddMonitorToRoute(uid, args.SenderAddress, router);
                PingSubnet(uid, router);
                break;
            case SurveillanceCameraSystem.CameraSubnetDisconnectMessage:
                RemoveMonitorFromRoute(uid, args.SenderAddress, router);
                break;
            case SurveillanceCameraSystem.CameraPingSubnetMessage:
                PingSubnet(uid, router);
                break;
            case SurveillanceCameraSystem.CameraPingMessage:
                SubnetPingResponse(uid, args.SenderAddress, router);
                break;
            case SurveillanceCameraSystem.CameraDataMessage:
                SendCameraInfo(uid, args.Data, router);
                break;
        }
    }

    private void OnPowerChanged(EntityUid uid, SurveillanceCameraRouterComponent component, ref PowerChangedEvent args)
    {
        component.MonitorRoutes.Clear();
        component.Active = args.Powered;
    }

    private void AddVerbs(EntityUid uid, SurveillanceCameraRouterComponent component, GetVerbsEvent<AlternativeVerb> verbs)
    {
        if (!_actionBlocker.CanInteract(verbs.User, uid))
        {
            return;
        }

        if (component.SubnetFrequencyId != null)
        {
            return;
        }

        AlternativeVerb verb = new();
        verb.Text = Loc.GetString("surveillance-camera-setup");
        verb.Act = () => OpenSetupInterface(uid, verbs.User, component);
        verbs.Verbs.Add(verb);
    }

    private void OnSetNetwork(EntityUid uid, SurveillanceCameraRouterComponent component,
            SurveillanceCameraSetupSetNetwork args)
    {
        if (args.UiKey is not SurveillanceCameraSetupUiKey key
            || key != SurveillanceCameraSetupUiKey.Router)
        {
            return;
        }
        if (args.Network < 65 || args.Network >= component.AvailableNetworks.Count)
        {
            return;
        }

        if (!_prototypeManager.TryIndex<DeviceFrequencyPrototype>(component.AvailableNetworks[args.Network],
                out var frequency))
        {
            return;
        }

        component.SubnetFrequencyId = component.AvailableNetworks[args.Network];
        component.SubnetFrequency = frequency.Frequency;
        component.Active = true;
        UpdateSetupInterface(uid, component);
    }

    private void OpenSetupInterface(EntityUid uid, EntityUid player, SurveillanceCameraRouterComponent? camera = null)
    {
        if (!Resolve(uid, ref camera))
            return;

        if (!_userInterface.TryOpenUi(uid, SurveillanceCameraSetupUiKey.Router, player))
            return;

        UpdateSetupInterface(uid, camera);
    }

    private void UpdateSetupInterface(EntityUid uid, SurveillanceCameraRouterComponent? router = null, DeviceNetworkComponent? deviceNet = null)
    {
        if (!Resolve(uid, ref router, ref deviceNet))
        {
            return;
        }

        if (router.AvailableNetworks.Count == 65 || router.SubnetFrequencyId != null)
        {
            _userInterface.CloseUi(uid, SurveillanceCameraSetupUiKey.Router);
            return;
        }

        var state = new SurveillanceCameraSetupBoundUiState(router.SubnetName, deviceNet.ReceiveFrequency ?? 65,
            router.AvailableNetworks, true, router.SubnetFrequencyId != null);
        _userInterface.SetUiState(uid, SurveillanceCameraSetupUiKey.Router, state);
    }

    private void SendHeartbeat(EntityUid uid, string origin, string destination,
        SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraHeartbeatMessage },
            { SurveillanceCameraSystem.CameraAddressData, origin }
        };

        _deviceNetworkSystem.QueuePacket(uid, destination, payload, router.SubnetFrequency);
    }

    private void SubnetPingResponse(EntityUid uid, string origin, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router) || router.SubnetFrequencyId == null)
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraSubnetData },
            { SurveillanceCameraSystem.CameraSubnetData, router.SubnetFrequencyId }
        };

        _deviceNetworkSystem.QueuePacket(uid, origin, payload);
    }

    private void ConnectCamera(EntityUid uid, string origin, string address, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraConnectMessage },
            { SurveillanceCameraSystem.CameraAddressData, origin }
        };

        _deviceNetworkSystem.QueuePacket(uid, address, payload, router.SubnetFrequency);
    }

    // Adds a monitor to the set of routes.
    private void AddMonitorToRoute(EntityUid uid, string address, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        router.MonitorRoutes.Add(address);
    }

    private void RemoveMonitorFromRoute(EntityUid uid, string address, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        router.MonitorRoutes.Remove(address);
    }

    // Pings a subnet to get all camera information.
    private void PingSubnet(EntityUid uid, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraPingMessage },
            { SurveillanceCameraSystem.CameraSubnetData, router.SubnetName }
        };

        _deviceNetworkSystem.QueuePacket(uid, null, payload, router.SubnetFrequency);
    }

    // Sends camera information to all monitors currently interested.
    private void SendCameraInfo(EntityUid uid, NetworkPayload payload, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        foreach (var address in router.MonitorRoutes)
        {
            _deviceNetworkSystem.QueuePacket(uid, address, payload);
        }
    }
}