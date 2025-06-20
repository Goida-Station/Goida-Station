// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Movement.Components;

/// <summary>
/// Updates a sprite layer based on whether an entity is moving via input or not.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class SpriteMovementComponent : Component
{
    /// <summary>
    /// Layer and sprite state to use when moving.
    /// </summary>
    [DataField]
    public Dictionary<string, PrototypeLayerData> MovementLayers = new();

    /// <summary>
    /// Layer and sprite state to use when runnin.
    /// </summary>
    [DataField]
    public Dictionary<string, PrototypeLayerData> RunLayers = new();

    /// <summary>
    /// Layer and sprite state to use when walking.
    /// </summary>
    [DataField]
    public Dictionary<string, PrototypeLayerData> WalkLayers = new();

    /// <summary>
    /// Layer and sprite state to use when not moving.
    /// </summary>
    [DataField]
    public Dictionary<string, PrototypeLayerData> NoMovementLayers = new();

    [DataField, AutoNetworkedField]
    public bool IsMoving;
}