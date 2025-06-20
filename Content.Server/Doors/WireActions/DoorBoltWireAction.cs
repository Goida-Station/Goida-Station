// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Doors.Systems;
using Content.Server.Wires;
using Content.Shared.Doors;
using Content.Shared.Doors.Components;
using Content.Shared.Wires;

namespace Content.Server.Doors;

public sealed partial class DoorBoltWireAction : ComponentWireAction<DoorBoltComponent>
{
    public override Color Color { get; set; } = Color.Red;
    public override string Name { get; set; } = "wire-name-door-bolt";

    public override StatusLightState? GetLightState(Wire wire, DoorBoltComponent comp)
        => comp.BoltsDown ? StatusLightState.On : StatusLightState.Off;

    public override object StatusKey { get; } = AirlockWireStatus.BoltIndicator;

    public override bool Cut(EntityUid user, Wire wire, DoorBoltComponent airlock)
    {
        EntityManager.System<DoorSystem>().SetBoltWireCut((wire.Owner, airlock), true);
        if (!airlock.BoltsDown && IsPowered(wire.Owner))
            EntityManager.System<DoorSystem>().SetBoltsDown((wire.Owner, airlock), true, user);

        return true;
    }

    public override bool Mend(EntityUid user, Wire wire, DoorBoltComponent door)
    {
        EntityManager.System<DoorSystem>().SetBoltWireCut((wire.Owner, door), false);
        return true;
    }

    public override void Pulse(EntityUid user, Wire wire, DoorBoltComponent door)
    {
        if (IsPowered(wire.Owner))
            EntityManager.System<DoorSystem>().SetBoltsDown((wire.Owner, door), !door.BoltsDown);
        else if (!door.BoltsDown)
            EntityManager.System<DoorSystem>().SetBoltsDown((wire.Owner, door), true);
    }
}