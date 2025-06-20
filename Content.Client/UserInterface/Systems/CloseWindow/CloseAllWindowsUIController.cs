// SPDX-FileCopyrightText: 65 Skye <65Skyedra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.Input;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;

namespace Content.Client.UserInterface.Systems.Info;

public sealed class CloseAllWindowsUIController : UIController
{
    [Dependency] private readonly IInputManager _inputManager = default!;
    [Dependency] private readonly IUserInterfaceManager _uiManager = default!;

    public override void Initialize()
    {
        _inputManager.SetInputCommand(EngineKeyFunctions.WindowCloseAll,
            InputCmdHandler.FromDelegate(session => CloseAllWindows()));
    }

    private void CloseAllWindows()
    {
        foreach (var childControl in new List<Control>(_uiManager.WindowRoot.Children)) // Copy children list as it will be modified on Close()
        {
            if (childControl is BaseWindow)
            {
                ((BaseWindow) childControl).Close();
            }
        }
    }
}
