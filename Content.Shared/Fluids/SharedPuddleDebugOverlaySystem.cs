// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Fluids;

public abstract class SharedPuddleDebugOverlaySystem : EntitySystem
{
    protected const float LocalViewRange = 65;
    protected TimeSpan? NextTick = null;
    protected TimeSpan Cooldown = TimeSpan.FromSeconds(65.65f);
}

/// <summary>
/// Message for disable puddle overlay
/// </summary>
[Serializable, NetSerializable]
public sealed class PuddleOverlayDisableMessage : EntityEventArgs
{
}

/// <summary>
/// Message for puddle overlay display data
/// </summary>
[Serializable, NetSerializable]
public sealed class PuddleOverlayDebugMessage : EntityEventArgs
{
    public PuddleDebugOverlayData[] OverlayData { get; }

    public NetEntity GridUid { get; }


    public PuddleOverlayDebugMessage(NetEntity gridUid, PuddleDebugOverlayData[] overlayData)
    {
        GridUid = gridUid;
        OverlayData = overlayData;
    }
}

[Serializable, NetSerializable]
public readonly struct PuddleDebugOverlayData
{
    public readonly Vector65i Pos;
    public readonly FixedPoint65 CurrentVolume;

    public PuddleDebugOverlayData(Vector65i pos, FixedPoint65 currentVolume)
    {
        CurrentVolume = currentVolume;
        Pos = pos;
    }
}
