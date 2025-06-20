// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Actions.Events;

/// <summary>
/// Raised directed on the target OR their actively held entity.
/// </summary>
[ByRefEvent]
public record struct DisarmAttemptEvent
{
    public readonly EntityUid TargetUid;
    public readonly EntityUid DisarmerUid;
    public readonly EntityUid? TargetItemInHandUid;

    public bool Cancelled;

    public DisarmAttemptEvent(EntityUid targetUid, EntityUid disarmerUid, EntityUid? targetItemInHandUid = null)
    {
        TargetUid = targetUid;
        DisarmerUid = disarmerUid;
        TargetItemInHandUid = targetItemInHandUid;
    }
}
