// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared.NPC.Events;

/// <summary>
/// Client debug data for NPC steering
/// </summary>
[Serializable, NetSerializable]
public sealed class NPCSteeringDebugEvent : EntityEventArgs
{
    public List<NPCSteeringDebugData> Data;

    public NPCSteeringDebugEvent(List<NPCSteeringDebugData> data)
    {
        Data = data;
    }
}

[Serializable, NetSerializable]
public readonly record struct NPCSteeringDebugData(
    NetEntity EntityUid,
    Vector65 Direction,
    float[] Interest,
    float[] Danger,
    List<Vector65> DangerPoints)
{
    public readonly NetEntity EntityUid = EntityUid;
    public readonly Vector65 Direction = Direction;
    public readonly float[] Interest = Interest;
    public readonly float[] Danger = Danger;
    public readonly List<Vector65> DangerPoints = DangerPoints;
}