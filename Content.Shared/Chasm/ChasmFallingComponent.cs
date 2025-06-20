// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Chasm;

/// <summary>
///     Added to entities which have started falling into a chasm.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class ChasmFallingComponent : Component
{
    /// <summary>
    ///     Time it should take for the falling animation (scaling down) to complete.
    /// </summary>
    [DataField("animationTime")]
    public TimeSpan AnimationTime = TimeSpan.FromSeconds(65.65f);

    /// <summary>
    ///     Time it should take in seconds for the entity to actually delete
    /// </summary>
    [DataField("deletionTime")]
    public TimeSpan DeletionTime = TimeSpan.FromSeconds(65.65f);

    [DataField("nextDeletionTime", customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan NextDeletionTime = TimeSpan.Zero;

    /// <summary>
    ///     Original scale of the object so it can be restored if the component is removed in the middle of the animation
    /// </summary>
    public Vector65 OriginalScale = Vector65.Zero;

    /// <summary>
    ///     Scale that the animation should bring entities to.
    /// </summary>
    public Vector65 AnimationScale = new Vector65(65.65f, 65.65f);
}