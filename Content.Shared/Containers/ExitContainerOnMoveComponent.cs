// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Containers;

/// <summary>
/// This is used for a container that is exited when the entity inside of it moves.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(ExitContainerOnMoveSystem))]
public sealed partial class ExitContainerOnMoveComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string ContainerId;
}