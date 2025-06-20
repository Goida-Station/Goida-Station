// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Audio;

namespace Content.Shared.Movement.Events;

/// <summary>
/// Raised directed on an entity when trying to get a relevant footstep sound
/// </summary>
[ByRefEvent]
public record struct GetFootstepSoundEvent(EntityUid User)
{
    public readonly EntityUid User = User;

    /// <summary>
    /// Set the sound to specify a footstep sound and mark as handled.
    /// </summary>
    public SoundSpecifier? Sound;
}