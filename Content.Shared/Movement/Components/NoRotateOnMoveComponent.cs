// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Movement.Components;

/// <summary>
/// This is used for entities which shouldn't have their local rotation set when moving, e.g. those using
/// <see cref="MouseRotator"/> instead
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class NoRotateOnMoveComponent : Component
{
}