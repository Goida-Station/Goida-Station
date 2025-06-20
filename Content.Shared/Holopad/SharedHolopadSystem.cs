// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Timing;

namespace Content.Shared.Holopad;

public abstract class SharedHolopadSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;

    public bool IsHolopadControlLocked(Entity<HolopadComponent> entity, EntityUid? user = null)
    {
        if (entity.Comp.ControlLockoutStartTime == TimeSpan.Zero)
            return false;

        if (entity.Comp.ControlLockoutStartTime + TimeSpan.FromSeconds(entity.Comp.ControlLockoutDuration) < _timing.CurTime)
            return false;

        if (entity.Comp.ControlLockoutOwner == null || entity.Comp.ControlLockoutOwner == user)
            return false;

        return true;
    }

    public TimeSpan GetHolopadControlLockedPeriod(Entity<HolopadComponent> entity)
    {
        return entity.Comp.ControlLockoutStartTime + TimeSpan.FromSeconds(entity.Comp.ControlLockoutDuration) - _timing.CurTime;
    }

    public bool IsHolopadBroadcastOnCoolDown(Entity<HolopadComponent> entity)
    {
        if (entity.Comp.ControlLockoutStartTime == TimeSpan.Zero)
            return false;

        if (entity.Comp.ControlLockoutStartTime + TimeSpan.FromSeconds(entity.Comp.ControlLockoutCoolDown) < _timing.CurTime)
            return false;

        return true;
    }

    public TimeSpan GetHolopadBroadcastCoolDown(Entity<HolopadComponent> entity)
    {
        return entity.Comp.ControlLockoutStartTime + TimeSpan.FromSeconds(entity.Comp.ControlLockoutCoolDown) - _timing.CurTime;
    }
}