// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Power;
using Robust.Client.UserInterface;

namespace Content.Client.Power.PowerCharge;

public sealed class PowerChargeBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private PowerChargeWindow? _window;

    public PowerChargeBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    public void SetPowerSwitch(bool on)
    {
        SendMessage(new SwitchChargingMachineMessage(on));
    }

    protected override void Open()
    {
        base.Open();
        if (!EntMan.TryGetComponent(Owner, out PowerChargeComponent? component))
            return;

        _window = this.CreateWindow<PowerChargeWindow>();
        _window.UpdateWindow(this, Loc.GetString(component.WindowTitle));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (state is not PowerChargeState chargeState)
            return;

        _window?.UpdateState(chargeState);
    }
}