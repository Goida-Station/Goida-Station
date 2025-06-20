// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.Events;

/// <summary>
/// Raised on a client when it wishes to FTL to a beacon.
/// </summary>
[Serializable, NetSerializable]
public sealed class ShuttleConsoleFTLBeaconMessage : BoundUserInterfaceMessage
{
    public NetEntity Beacon;
    public Angle Angle;
}