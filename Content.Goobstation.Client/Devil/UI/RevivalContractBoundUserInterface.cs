// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Devil.UI;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Goobstation.Client.Devil.UI;

[UsedImplicitly]
public sealed class RevivalContractBoundUserInterface : BoundUserInterface
{
    private RevivalContractMenu? _menu;

    public RevivalContractBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<RevivalContractMenu>();
        _menu.SetEntity(Owner);
        _menu.Accepted += OnAccepted;
        _menu.Rejected += OnRejected;
        _menu.OnClose += OnClosed;

        _menu.OpenCentered();
    }

    private void OnAccepted()
    {
        SendPredictedMessage(new RevivalContractMessage(true));
        Close();
    }

    private void OnRejected()
    {
        SendPredictedMessage(new RevivalContractMessage(false));
        Close();
    }

    private void OnClosed()
    {
        SendPredictedMessage(new RevivalContractMessage(false));
        Close();
    }
}
