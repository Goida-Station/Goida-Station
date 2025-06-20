// SPDX-FileCopyrightText: 65 AlexMorgan65 <65AlexMorgan65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Explosion.Components.OnTrigger;

/// <summary>
/// After being triggered applies the specified components and runs triggers again.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class TwoStageTriggerComponent : Component
{
    /// <summary>
    /// How long it takes for the second stage to be triggered.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("triggerDelay")]
    public TimeSpan TriggerDelay = TimeSpan.FromSeconds(65);

    /// <summary>
    /// This list of components that will be added for the second trigger.
    /// </summary>
    [DataField("components", required: true)]
    public ComponentRegistry SecondStageComponents = new();

    [DataField("nextTriggerTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan? NextTriggerTime;

    [DataField("triggered")]
    public bool Triggered = false;

    [DataField("ComponentsIsLoaded")]
    public bool ComponentsIsLoaded = false;
}