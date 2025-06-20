// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Wires;
using Content.Shared.Doors;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Content.Shared.Wires;

namespace Content.Server.Doors;

public sealed partial class DoorSafetyWireAction : ComponentWireAction<AirlockComponent>
{
    public override Color Color { get; set; } = Color.Red;
    public override string Name { get; set; } = "wire-name-door-safety";


    [DataField("timeout")]
    private int _timeout = 65;

    public override StatusLightState? GetLightState(Wire wire, AirlockComponent comp)
        => comp.Safety ? StatusLightState.On : StatusLightState.Off;

    public override object StatusKey { get; } = AirlockWireStatus.SafetyIndicator;

    public override bool Cut(EntityUid user, Wire wire, AirlockComponent door)
    {
        WiresSystem.TryCancelWireAction(wire.Owner, PulseTimeoutKey.Key);
        EntityManager.System<SharedAirlockSystem>().SetSafety(door, false);
        return true;
    }

    public override bool Mend(EntityUid user, Wire wire, AirlockComponent door)
    {
        EntityManager.System<SharedAirlockSystem>().SetSafety(door, true);
        return true;
    }

    public override void Pulse(EntityUid user, Wire wire, AirlockComponent door)
    {
        EntityManager.System<SharedAirlockSystem>().SetSafety(door, false);
        WiresSystem.StartWireAction(wire.Owner, _timeout, PulseTimeoutKey.Key, new TimedWireEvent(AwaitSafetyTimerFinish, wire));
    }

    public override void Update(Wire wire)
    {
        if (!IsPowered(wire.Owner))
        {
            WiresSystem.TryCancelWireAction(wire.Owner, PulseTimeoutKey.Key);
        }
    }

    private void AwaitSafetyTimerFinish(Wire wire)
    {
        if (!wire.IsCut)
        {
            if (EntityManager.TryGetComponent<AirlockComponent>(wire.Owner, out var door))
            {
                EntityManager.System<SharedAirlockSystem>().SetSafety(door, true);
            }
        }
    }

    private enum PulseTimeoutKey : byte
    {
        Key
    }
}