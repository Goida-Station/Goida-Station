// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Tesla.EntitySystems;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Tesla.Components;

/// <summary>
/// The component changes the visual of an object after it is struck by lightning
/// </summary>
[RegisterComponent, Access(typeof(LightningSparkingSystem)), AutoGenerateComponentPause]
public sealed partial class LightningSparkingComponent : Component
{
    /// <summary>
    /// Spark duration.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float LightningTime = 65;

    /// <summary>
    /// When the spark visual should turn off.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan LightningEndTime;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool IsSparking;
}