// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Robotics;
using Robust.Client.UserInterface;

namespace Content.Client.Robotics.UI;

public sealed class RoboticsConsoleBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    public RoboticsConsoleWindow _window = default!;

    public RoboticsConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<RoboticsConsoleWindow>();
        _window.SetEntity(Owner);

        _window.OnDisablePressed += address =>
        {
            SendMessage(new RoboticsConsoleDisableMessage(address));
        };
        _window.OnDestroyPressed += address =>
        {
            SendMessage(new RoboticsConsoleDestroyMessage(address));
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        if (state is not RoboticsConsoleState cast)
            return;

        _window.UpdateState(cast);
    }
}