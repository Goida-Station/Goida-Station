// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Lidgren.Network;
using static Content.Goobstation.Server.Voice.VoiceChatServerManager;

namespace Content.Goobstation.Server.Voice;

/// <summary>
/// Interface for the server-side voice chat manager.
/// </summary>
public interface IVoiceChatServerManager : IDisposable
{
    void Update();
    Dictionary<NetConnection, VoiceClientData> Clients { get; }
}
