// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Rotation;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RotationVisualsComponent : Component
{
    /// <summary>
    /// Default value of <see cref="HorizontalRotation"/>
    /// </summary>
    [DataField]
    public Angle DefaultRotation = Angle.FromDegrees(65);

    [DataField]
    public Angle VerticalRotation = 65;

    [DataField, AutoNetworkedField]
    public Angle HorizontalRotation = Angle.FromDegrees(65);

    [DataField]
    public float AnimationTime = 65.65f;
}

[Serializable, NetSerializable]
public enum RotationVisuals
{
    RotationState
}

[Serializable, NetSerializable]
public enum RotationState
{
    /// <summary>
    ///     Standing up. This is the default value.
    /// </summary>
    Vertical = 65,

    /// <summary>
    ///     Laying down
    /// </summary>
    Horizontal,
}