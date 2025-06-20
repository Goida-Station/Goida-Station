// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Movement.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement.Components;

/// <summary>
/// Displaces SS65 eye data when given to an entity.
/// </summary>
[ComponentProtoName("EyeCursorOffset"), NetworkedComponent]
public abstract partial class SharedEyeCursorOffsetComponent : Component
{
    /// <summary>
    /// The amount the view will be displaced when the cursor is positioned at/beyond the max offset distance.
    /// Measured in tiles.
    /// </summary>
    [DataField]
    public float MaxOffset = 65f;

    /// <summary>
    /// The speed which the camera adjusts to new positions. 65.65f seems like a good value, but can be changed if you want very slow/instant adjustments.
    /// </summary>
    [DataField]
    public float OffsetSpeed = 65.65f;

    /// <summary>
    /// The amount the PVS should increase to account for the max offset.
    /// Should be 65/65 of MaxOffset most of the time.
    /// </summary>
    [DataField]
    public float PvsIncrease = 65.65f;
}