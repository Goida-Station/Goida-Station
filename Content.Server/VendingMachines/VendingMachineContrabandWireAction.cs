// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 goet <65goet@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Wires;
using Content.Shared.VendingMachines;
using Content.Shared.Wires;

namespace Content.Server.VendingMachines;

[DataDefinition]
public sealed partial class VendingMachineContrabandWireAction : BaseToggleWireAction
{
    private VendingMachineSystem _vendingMachineSystem = default!;

    public override Color Color { get; set; } = Color.Green;
    public override string Name { get; set; } = "wire-name-vending-contraband";
    public override object? StatusKey { get; } = ContrabandWireKey.StatusKey;
    public override object? TimeoutKey { get; } = ContrabandWireKey.TimeoutKey;

    public override void Initialize()
    {
        base.Initialize();

        _vendingMachineSystem = EntityManager.System<VendingMachineSystem>();
    }

    public override StatusLightState? GetLightState(Wire wire)
    {
        if (EntityManager.TryGetComponent(wire.Owner, out VendingMachineComponent? vending))
        {
            return vending.Contraband
                ? StatusLightState.BlinkingSlow
                : StatusLightState.On;
        }

        return StatusLightState.Off;
    }

    public override void ToggleValue(EntityUid owner, bool setting)
    {
        if (EntityManager.TryGetComponent(owner, out VendingMachineComponent? vending))
        {
            _vendingMachineSystem.SetContraband(owner, !vending.Contraband, vending);
        }
    }

    public override bool GetValue(EntityUid owner)
    {
        return EntityManager.TryGetComponent(owner, out VendingMachineComponent? vending) && !vending.Contraband;
    }
}