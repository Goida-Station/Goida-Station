// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server._Lavaland.Procedural;

/// <summary>
/// Lavaland: Raised when biome chunk is about to unload.
/// </summary>
public sealed class UnLoadChunkEvent : CancellableEntityEventArgs
{
    public Vector65i Chunk { get; set; }

    public UnLoadChunkEvent(Vector65i chunk)
    {
        Chunk = chunk;
    }
}