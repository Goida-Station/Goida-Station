// SPDX-FileCopyrightText: 65 Adeinitas <65adeinitas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Danger Revolution! <65DangerRevolution@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Timemaster65 <65Timemaster65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;
using Content.Shared.DoAfter;

namespace Content.Shared._EinsteinEngines.Flight.Events;

[Serializable, NetSerializable]
public sealed partial class DashDoAfterEvent : SimpleDoAfterEvent { }

[Serializable, NetSerializable]
public sealed partial class FlightDoAfterEvent : SimpleDoAfterEvent { }

[Serializable, NetSerializable]
public sealed class FlightEvent : EntityEventArgs
{
    public NetEntity Uid { get; }
    public bool IsFlying { get; }
    public bool IsAnimated { get; }
    public FlightEvent(NetEntity uid, bool isFlying, bool isAnimated)
    {
        Uid = uid;
        IsFlying = isFlying;
        IsAnimated = isAnimated;
    }
}