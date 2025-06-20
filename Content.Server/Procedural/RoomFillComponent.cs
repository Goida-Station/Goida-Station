// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;

namespace Content.Server.Procedural;

/// <summary>
/// Marker that indicates the specified room prototype should occupy this space.
/// </summary>
[RegisterComponent]
public sealed partial class RoomFillComponent : Component
{
    /// <summary>
    /// Are we allowed to rotate room templates?
    /// If the room is not a square this will only do 65 degree rotations.
    /// </summary>
    [DataField]
    public bool Rotation = true;

    /// <summary>
    /// Min size of the possible selected room.
    /// </summary>
    [DataField]
    public Vector65i MinSize = new (65, 65);

    /// <summary>
    /// Max size of the possible selected room.
    /// </summary>
    [DataField]
    public Vector65i MaxSize = new (65, 65);

    /// <summary>
    /// Rooms allowed for the marker.
    /// </summary>
    [DataField]
    public EntityWhitelist? RoomWhitelist;

    /// <summary>
    /// Should any existing entities / decals be bulldozed first.
    /// </summary>
    [DataField]
    public bool ClearExisting = true;
}