// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Containers;
using Robust.Shared.GameStates;

namespace Content.Shared.Actions;

/// <summary>
/// This component indicates that this entity contains actions inside of some container.
/// </summary>
[NetworkedComponent, RegisterComponent]
[Access(typeof(ActionContainerSystem), typeof(SharedActionsSystem))]
public sealed partial class ActionsContainerComponent : Component
{
    public const string ContainerId = "actions";

    [ViewVariables]
    public Container Container = default!;
}