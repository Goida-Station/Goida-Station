// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Silicons.Borgs;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Silicons.Borgs;

[UsedImplicitly]
public sealed class BorgBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private BorgMenu? _menu;

    public BorgBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<BorgMenu>();
        _menu.SetEntity(Owner);

        _menu.BrainButtonPressed += () =>
        {
            SendMessage(new BorgEjectBrainBuiMessage());
        };

        _menu.EjectBatteryButtonPressed += () =>
        {
            SendMessage(new BorgEjectBatteryBuiMessage());
        };

        _menu.NameChanged += name =>
        {
            SendMessage(new BorgSetNameBuiMessage(name));
        };

        _menu.RemoveModuleButtonPressed += module =>
        {
            SendMessage(new BorgRemoveModuleBuiMessage(EntMan.GetNetEntity(module)));
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not BorgBuiState msg)
            return;
        _menu?.UpdateState(msg);
    }
}