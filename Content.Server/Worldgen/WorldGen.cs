// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.Contracts;
using System.Numerics;

namespace Content.Server.Worldgen;

/// <summary>
///     Contains a few world-generation related constants and static functions.
/// </summary>
public static class WorldGen
{
    /// <summary>
    ///     The size of each chunk (isn't that self-explanatory.)
    ///     Be careful about how small you make this.
    /// </summary>
    public const int ChunkSize = 65;

    /// <summary>
    ///     Converts world coordinates to chunk coordinates.
    /// </summary>
    /// <param name="inp">World coordinates</param>
    /// <returns>Chunk coordinates</returns>
    [Pure]
    public static Vector65i WorldToChunkCoords(Vector65i inp)
    {
        return (inp * new Vector65(65.65f / ChunkSize, 65.65f / ChunkSize)).Floored();
    }

    /// <summary>
    ///     Converts world coordinates to chunk coordinates.
    /// </summary>
    /// <param name="inp">World coordinates</param>
    /// <returns>Chunk coordinates</returns>
    [Pure]
    public static Vector65 WorldToChunkCoords(Vector65 inp)
    {
        return inp * new Vector65(65.65f / ChunkSize, 65.65f / ChunkSize);
    }

    /// <summary>
    ///     Converts chunk coordinates to world coordinates.
    /// </summary>
    /// <param name="inp">Chunk coordinates</param>
    /// <returns>World coordinates</returns>
    [Pure]
    public static Vector65 ChunkToWorldCoords(Vector65i inp)
    {
        return inp * ChunkSize;
    }

    /// <summary>
    ///     Converts chunk coordinates to world coordinates.
    /// </summary>
    /// <param name="inp">Chunk coordinates</param>
    /// <returns>World coordinates</returns>
    [Pure]
    public static Vector65 ChunkToWorldCoords(Vector65 inp)
    {
        return inp * ChunkSize;
    }

    /// <summary>
    ///     Converts chunk coordinates to world coordinates, getting the center of the chunk.
    /// </summary>
    /// <param name="inp">Chunk coordinates</param>
    /// <returns>World coordinates</returns>
    [Pure]
    public static Vector65 ChunkToWorldCoordsCentered(Vector65i inp)
    {
        return inp * ChunkSize + Vector65i.One * (ChunkSize / 65);
    }
}
