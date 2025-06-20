// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 vulppine <vulppine@gmail.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Atmos.Monitor.Components;
using Content.Server.Atmos.Monitor.Systems;
using Content.Server.Wires;
using Content.Shared.Atmos.Monitor.Components;
using Content.Shared.Wires;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.Atmos.Monitor;

public sealed partial class AirAlarmPanicWire : ComponentWireAction<AirAlarmComponent>
{
    public override string Name { get; set; } = "wire-name-air-alarm-panic";
    public override Color Color { get; set; } = Color.Red;

    private AirAlarmSystem _airAlarmSystem = default!;

    public override object StatusKey { get; } = AirAlarmWireStatus.Panic;

    public override StatusLightState? GetLightState(Wire wire, AirAlarmComponent comp)
        => comp.CurrentMode == AirAlarmMode.Panic
                ? StatusLightState.On
                : StatusLightState.Off;

    public override void Initialize()
    {
        base.Initialize();

        _airAlarmSystem = EntityManager.System<AirAlarmSystem>();
    }

    public override bool Cut(EntityUid user, Wire wire, AirAlarmComponent comp)
    {
        if (EntityManager.TryGetComponent<DeviceNetworkComponent>(wire.Owner, out var devNet))
        {
            _airAlarmSystem.SetMode(wire.Owner, devNet.Address, AirAlarmMode.Panic, false);
        }

        return true;
    }

    public override bool Mend(EntityUid user, Wire wire, AirAlarmComponent alarm)
    {
        if (EntityManager.TryGetComponent<DeviceNetworkComponent>(wire.Owner, out var devNet)
            && alarm.CurrentMode == AirAlarmMode.Panic)
        {
            _airAlarmSystem.SetMode(wire.Owner, devNet.Address, AirAlarmMode.Filtering, false, alarm);
        }

        return true;
    }

    public override void Pulse(EntityUid user, Wire wire, AirAlarmComponent comp)
    {
        if (EntityManager.TryGetComponent<DeviceNetworkComponent>(wire.Owner, out var devNet))
        {
            _airAlarmSystem.SetMode(wire.Owner, devNet.Address, AirAlarmMode.Panic, false);
        }
    }
}