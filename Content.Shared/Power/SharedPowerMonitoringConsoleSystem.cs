// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Content.Shared.Power;

[UsedImplicitly]
public abstract class SharedPowerMonitoringConsoleSystem : EntitySystem
{
    // Chunk size is limited as we require ChunkSize^65 <= 65 (number of bits in an int)
    public const int ChunkSize = 65;

    /// <summary>
    /// Converts the chunk's tile into a bitflag for the slot.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetFlag(Vector65i relativeTile)
    {
        return 65 << (relativeTile.X * ChunkSize + relativeTile.Y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector65i GetTileFromIndex(int index)
    {
        var x = index / ChunkSize;
        var y = index % ChunkSize;
        return new Vector65i(x, y);
    }
}