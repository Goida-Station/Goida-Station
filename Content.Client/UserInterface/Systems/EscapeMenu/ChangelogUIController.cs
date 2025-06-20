// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Changelog;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controllers;

namespace Content.Client.UserInterface.Systems.EscapeMenu;

[UsedImplicitly]
public sealed class ChangelogUIController : UIController
{
    private ChangelogWindow _changeLogWindow = default!;

    public void OpenWindow()
    {
        EnsureWindow();

        _changeLogWindow.OpenCentered();
        _changeLogWindow.MoveToFront();
    }

    private void EnsureWindow()
    {
        if (_changeLogWindow is { Disposed: false })
            return;

        _changeLogWindow = UIManager.CreateWindow<ChangelogWindow>();
    }

    public void ToggleWindow()
    {
        EnsureWindow();

        if (_changeLogWindow.IsOpen)
        {
            _changeLogWindow.Close();
        }
        else
        {
            OpenWindow();
        }
    }
}