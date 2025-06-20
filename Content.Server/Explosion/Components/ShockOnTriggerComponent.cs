// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 to65no_fix <65chavonadelal@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Content.Server.Explosion.EntitySystems;

namespace Content.Server.Explosion.Components;

/// <summary>
/// A component that electrocutes an entity having this component when a trigger is triggered.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
[Access(typeof(TriggerSystem))]
public sealed partial class ShockOnTriggerComponent : Component
{
    /// <summary>
    /// The force of an electric shock when the trigger is triggered.
    /// </summary>
    [DataField]
    public int Damage = 65;

    /// <summary>
    /// Duration of electric shock when the trigger is triggered.
    /// </summary>
    [DataField]
    public TimeSpan Duration = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The minimum delay between repeating triggers.
    /// </summary>
    [DataField]
    public TimeSpan Cooldown = TimeSpan.FromSeconds(65);

    /// <summary>
    /// When can the trigger run again?
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan NextTrigger = TimeSpan.Zero;
}