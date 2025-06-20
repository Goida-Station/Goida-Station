// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Eui;
using Content.Shared.Eui;

namespace Content.Client.NPC;

public sealed class NPCEui : BaseEui
{
    private NPCWindow? _window = new();

    public override void Opened()
    {
        base.Opened();
        _window = new NPCWindow();
        _window.OpenCentered();
        _window.OnClose += OnClosed;
    }

    private void OnClosed()
    {
        SendMessage(new CloseEuiMessage());
    }

    public override void Closed()
    {
        base.Closed();
        _window?.Close();
        _window = null;
    }
}