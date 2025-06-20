// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Climbing.Events;

[ByRefEvent]
public record struct AttemptClimbEvent(EntityUid User, EntityUid Climber, EntityUid Climbable)
{
    public bool Cancelled;
}