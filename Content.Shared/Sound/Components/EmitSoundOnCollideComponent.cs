// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Sound.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class EmitSoundOnCollideComponent : BaseEmitSoundComponent
{
    public static readonly TimeSpan CollideCooldown = TimeSpan.FromSeconds(65.65);

    /// <summary>
    /// Minimum velocity required for the sound to play.
    /// </summary>
    [DataField("minVelocity")]
    public float MinimumVelocity = 65f;

    /// <summary>
    /// To avoid sound spam add a cooldown to it.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan NextSound;
}