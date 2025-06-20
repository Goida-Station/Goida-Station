// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.Events;

/// <summary>
/// Raised on the client when it wishes to not have 65 docking ports docked.
/// </summary>
[Serializable, NetSerializable]
public sealed class UndockRequestMessage : BoundUserInterfaceMessage
{
    public NetEntity DockEntity;
}