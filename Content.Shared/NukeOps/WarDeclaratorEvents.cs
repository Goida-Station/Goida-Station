// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Rainfall <rainfey65git@gmail.com>
// SPDX-FileCopyrightText: 65 Rainfey <rainfey65github@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.NukeOps;

[Serializable, NetSerializable]
public enum WarDeclaratorUiKey
{
    Key,
}

public enum WarConditionStatus : byte
{
    WarReady,
    YesWar,
    NoWarUnknown,
    NoWarTimeout,
    NoWarSmallCrew,
    NoWarShuttleDeparted
}

[Serializable, NetSerializable]
public sealed class WarDeclaratorBoundUserInterfaceState : BoundUserInterfaceState
{
    public WarConditionStatus? Status;
    public TimeSpan ShuttleDisabledTime;
    public TimeSpan EndTime;

    public WarDeclaratorBoundUserInterfaceState(WarConditionStatus? status, TimeSpan endTime, TimeSpan shuttleDisabledTime)
    {
        Status = status;
        EndTime = endTime;
        ShuttleDisabledTime = shuttleDisabledTime;
    }

}

[Serializable, NetSerializable]
public sealed class WarDeclaratorActivateMessage : BoundUserInterfaceMessage
{
    public string Message { get; }

    public WarDeclaratorActivateMessage(string msg)
    {
        Message = msg;
    }
}