// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Gateway;

/// <summary>
/// Sent from client to server upon taking a gateway destination.
/// </summary>
[Serializable, NetSerializable]
public sealed class GatewayDestinationMessage : EntityEventArgs
{
    public int Index;
}