// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Doors.Systems;
using Content.Server.Wires;
using Content.Shared.Doors;
using Content.Shared.Doors.Components;
using Content.Shared.Wires;

namespace Content.Server.Doors;

public sealed partial class DoorBoltLightWireAction : ComponentWireAction<DoorBoltComponent>
{
    public override Color Color { get; set; } = Color.Lime;
    public override string Name { get; set; } = "wire-name-bolt-light";

    public override StatusLightState? GetLightState(Wire wire, DoorBoltComponent comp)
        => comp.BoltLightsEnabled ? StatusLightState.On : StatusLightState.Off;

    public override object StatusKey { get; } = AirlockWireStatus.BoltLightIndicator;

    public override bool Cut(EntityUid user, Wire wire, DoorBoltComponent door)
    {
        EntityManager.System<DoorSystem>().SetBoltLightsEnabled((wire.Owner, door), false);
        return true;
    }

    public override bool Mend(EntityUid user, Wire wire, DoorBoltComponent door)
    {
        EntityManager.System<DoorSystem>().SetBoltLightsEnabled((wire.Owner, door), true);
        return true;
    }

    public override void Pulse(EntityUid user, Wire wire, DoorBoltComponent door)
    {
        EntityManager.System<DoorSystem>().SetBoltLightsEnabled((wire.Owner, door), !door.BoltLightsEnabled);
    }
}