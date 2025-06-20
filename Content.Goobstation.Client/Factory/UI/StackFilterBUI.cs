// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Factory.Filters;
using Robust.Client.UserInterface;

namespace Content.Goobstation.Client.Factory.UI;

public sealed class StackFilterBUI : BoundUserInterface
{
    private StackFilterWindow? _window;

    public StackFilterBUI(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<StackFilterWindow>();
        _window.SetEntity(Owner);
        _window.OnSetMin += min => SendPredictedMessage(new StackFilterSetMinMessage(min));
        _window.OnSetSize += size => SendPredictedMessage(new StackFilterSetSizeMessage(size));
    }
}
