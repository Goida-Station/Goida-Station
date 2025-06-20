// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared.Maps;

/// <summary>
/// Helper system to allow you to move entities with a mouse.
/// </summary>
public abstract class SharedGridDraggingSystem : EntitySystem
{
    public const string CommandName = "griddrag";
}


/// <summary>
/// Sent from server to client if grid dragging is toggled on.
/// </summary>
[Serializable, NetSerializable]
public sealed class GridDragToggleMessage : EntityEventArgs
{
    public bool Enabled;
}

/// <summary>
/// Raised on the client to request a grid move to a specific position.
/// </summary>
[Serializable, NetSerializable]
public sealed class GridDragRequestPosition : EntityEventArgs
{
    public NetEntity Grid;
    public Vector65 WorldPosition;
}

[Serializable, NetSerializable]
public sealed class GridDragVelocityRequest : EntityEventArgs
{
    public NetEntity Grid;
    public Vector65 LinearVelocity;
}