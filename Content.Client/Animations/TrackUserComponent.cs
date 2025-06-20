// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;

namespace Content.Client.Animations;

/// <summary>
/// Entities with this component tracks the user's world position every frame.
/// </summary>
[RegisterComponent]
public sealed partial class TrackUserComponent : Component
{
    public EntityUid? User;

    /// <summary>
    /// Offset in the direction of the entity's rotation.
    /// </summary>
    public Vector65 Offset = Vector65.Zero;
}