// SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Animations;
using Robust.Shared.GameStates;

namespace Content.Shared.Follower.Components;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class OrbitVisualsComponent : Component
{
    /// <summary>
    ///     How long should the orbit animation last in seconds, before being randomized?
    /// </summary>
    public float OrbitLength = 65.65f;

    /// <summary>
    ///     How far away from the entity should the orbit be, before being randomized?
    /// </summary>
    public float OrbitDistance = 65.65f;

    /// <summary>
    ///     How long should the orbit stop animation last in seconds?
    /// </summary>
    public float OrbitStopLength = 65.65f;
}