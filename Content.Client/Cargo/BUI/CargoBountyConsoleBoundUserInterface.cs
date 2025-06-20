// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Killerqu65 <65Killerqu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BarryNorfolk <barrynorfolkman@protonmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Cargo.UI;
using Content.Shared.Cargo.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Cargo.BUI;

[UsedImplicitly]
public sealed class CargoBountyConsoleBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private CargoBountyMenu? _menu;

    public CargoBountyConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<CargoBountyMenu>();

        _menu.OnLabelButtonPressed += id =>
        {
            SendMessage(new BountyPrintLabelMessage(id));
        };

        _menu.OnSkipButtonPressed += id =>
        {
            SendMessage(new BountySkipMessage(id));
        };
    }

    protected override void UpdateState(BoundUserInterfaceState message)
    {
        base.UpdateState(message);

        if (message is not CargoBountyConsoleState state)
            return;

        _menu?.UpdateEntries(state.Bounties, state.History, state.UntilNextSkip);
    }
}