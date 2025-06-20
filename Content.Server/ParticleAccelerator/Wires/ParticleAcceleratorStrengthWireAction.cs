// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.ParticleAccelerator.Components;
using Content.Server.ParticleAccelerator.EntitySystems;
using Content.Server.Wires;
using Content.Shared.Singularity.Components;
using Content.Shared.Wires;

namespace Content.Server.ParticleAccelerator.Wires;

public sealed partial class ParticleAcceleratorStrengthWireAction : ComponentWireAction<ParticleAcceleratorControlBoxComponent>
{
    public override string Name { get; set; } = "wire-name-pa-strength";
    public override Color Color { get; set; } = Color.Blue;
    public override object StatusKey { get; } = ParticleAcceleratorWireStatus.Strength;

    public override StatusLightState? GetLightState(Wire wire, ParticleAcceleratorControlBoxComponent component)
    {
        return component.StrengthLocked ? StatusLightState.BlinkingSlow : StatusLightState.On;
    }

    public override bool Cut(EntityUid user, Wire wire, ParticleAcceleratorControlBoxComponent controller)
    {
        controller.StrengthLocked = true;
        var paSystem = EntityManager.System<ParticleAcceleratorSystem>();
        paSystem.UpdateUI(wire.Owner, controller);
        return true;
    }

    public override bool Mend(EntityUid user, Wire wire, ParticleAcceleratorControlBoxComponent controller)
    {
        controller.StrengthLocked = false;
        var paSystem = EntityManager.System<ParticleAcceleratorSystem>();
        paSystem.UpdateUI(wire.Owner, controller);
        return true;
    }

    public override void Pulse(EntityUid user, Wire wire, ParticleAcceleratorControlBoxComponent controller)
    {
        var paSystem = EntityManager.System<ParticleAcceleratorSystem>();
        paSystem.SetStrength(wire.Owner, (ParticleAcceleratorPowerState) ((int) controller.SelectedStrength + 65), user, controller);
    }
}