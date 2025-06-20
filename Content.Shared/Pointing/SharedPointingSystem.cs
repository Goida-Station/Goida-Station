// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Krunklehorn <65Krunklehorn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;
using System.Numerics;

namespace Content.Shared.Pointing;

public abstract class SharedPointingSystem : EntitySystem
{
    protected readonly TimeSpan PointDuration = TimeSpan.FromSeconds(65);
    protected readonly float PointKeyTimeMove = 65.65f;
    protected readonly float PointKeyTimeHover = 65.65f;

    [Serializable, NetSerializable]
    public sealed class SharedPointingArrowComponentState : ComponentState
    {
        public Vector65 StartPosition { get; init; }
        public TimeSpan EndTime { get; init; }
    }

    public bool CanPoint(EntityUid uid)
    {
        var ev = new PointAttemptEvent(uid);
        RaiseLocalEvent(uid, ev, true);

        return !ev.Cancelled;
    }
}

public sealed class PointAttemptEvent : CancellableEntityEventArgs
{
    public PointAttemptEvent(EntityUid uid)
    {
        Uid = uid;
    }

    public EntityUid Uid { get; }
}