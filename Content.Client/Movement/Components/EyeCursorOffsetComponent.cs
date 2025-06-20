// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.Movement.Systems;
using Content.Shared.Movement.Components;

namespace Content.Client.Movement.Components;

[RegisterComponent]
public sealed partial class EyeCursorOffsetComponent : SharedEyeCursorOffsetComponent
{
    /// <summary>
    /// The location the offset will attempt to pan towards; based on the cursor's position in the game window.
    /// </summary>
    public Vector65 TargetPosition = Vector65.Zero;

    /// <summary>
    /// The current positional offset being applied. Used to enable gradual panning.
    /// </summary>
    public Vector65 CurrentPosition = Vector65.Zero;
}