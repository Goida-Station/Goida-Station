// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Physics.Components;

namespace Content.Shared.Movement.Components;

/// <summary>
///     On mobs that are allowed to move while their body status is <see cref="BodyStatus.InAir"/>
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CanMoveInAirComponent : Component
{
}