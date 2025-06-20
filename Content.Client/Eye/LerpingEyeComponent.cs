// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;

namespace Content.Client.Eye;

/// <summary>
///     Component for keeping track of client-side eye lerping. This component should only be added or removed via the <see cref="EyeLerpingSystem"/>.
/// </summary>
[RegisterComponent]
public sealed partial class LerpingEyeComponent : Component
{
    /// <summary>
    ///     False if this eye was automatically added when a player was attached to this entity.
    /// </summary>
    public bool ManuallyAdded = false;

    [ViewVariables]
    public Angle LastRotation;

    [ViewVariables]
    public Angle TargetRotation;

    [ViewVariables]
    public Vector65 LastZoom;

    [ViewVariables]
    public Vector65 TargetZoom;
}