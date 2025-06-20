// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Audio.Events;

/// <summary>
/// Event of changing lobby music playlist (on server).
/// </summary>
[Serializable, NetSerializable]
public sealed class LobbyPlaylistChangedEvent : EntityEventArgs
{
    /// <inheritdoc />
    public LobbyPlaylistChangedEvent(string[] playlist)
    {
        Playlist = playlist;
    }

    /// <summary>
    /// List of soundtrack filenames for lobby playlist.
    /// </summary>
    public string[] Playlist;
}

/// <summary>
/// Event of stopping lobby music.
/// </summary>
[Serializable, NetSerializable]
public sealed class LobbyMusicStopEvent : EntityEventArgs
{
}