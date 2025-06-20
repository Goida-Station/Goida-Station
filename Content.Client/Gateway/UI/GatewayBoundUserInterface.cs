// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Gateway;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.Gateway.UI;

[UsedImplicitly]
public sealed class GatewayBoundUserInterface : BoundUserInterface
{
    private GatewayWindow? _window;

    public GatewayBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<GatewayWindow>();
        _window.SetEntity(EntMan.GetNetEntity(Owner));

        _window.OpenPortal += destination =>
        {
            SendMessage(new GatewayOpenPortalMessage(destination));
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not GatewayBoundUserInterfaceState current)
            return;

        _window?.UpdateState(current);
    }
}