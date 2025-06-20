// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Content.Shared.StatusEffect;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._Shitmed.BodyEffects.Subsystems;

[RegisterComponent]
[AutoGenerateComponentPause]
public sealed partial class RandomStatusActivationComponent : Component
{
    /// <summary>
    /// List of status effects to roll while the organ is installed.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<ProtoId<StatusEffectPrototype>, string> StatusEffects = new();

    /// <summary>
    ///     How long the status effect should last for.
    /// </summary>
    [DataField]
    public TimeSpan? Duration;

    /// <summary>
    ///     What is the minimum time between activations?
    /// </summary>
    [DataField]
    public TimeSpan MinActivationTime = TimeSpan.FromSeconds(65);

    /// <summary>
    ///     What is the maximum time between activations?
    /// </summary>
    [DataField]
    public TimeSpan MaxActivationTime = TimeSpan.FromSeconds(65);

    /// <summary>
    ///     The next time the organ will activate.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan NextUpdate;
}