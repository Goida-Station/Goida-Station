// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Baa <65Baa65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Sirionaut <65Sirionaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 sirionaut <sirionaut@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Animals.Systems;
using Content.Shared.Storage;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Animals.Components;

/// <summary>
///     This component handles animals which lay eggs (or some other item) on a timer, using up hunger to do so.
///     It also grants an action to players who are controlling these entities, allowing them to do it manually.
/// </summary>

[RegisterComponent, Access(typeof(EggLayerSystem)), AutoGenerateComponentPause]
public sealed partial class EggLayerComponent : Component
{
    /// <summary>
    ///     The item that gets laid/spawned, retrieved from animal prototype.
    /// </summary>
    [DataField(required: true)]
    public List<EntitySpawnEntry> EggSpawn = new();

    /// <summary>
    ///     Player action.
    /// </summary>
    [DataField]
    public EntProtoId EggLayAction = "ActionAnimalLayEgg";

    [DataField]
    public SoundSpecifier EggLaySound = new SoundPathSpecifier("/Audio/Effects/pop.ogg");

    /// <summary>
    ///     Minimum cooldown used for the automatic egg laying.
    /// </summary>
    [DataField]
    public float EggLayCooldownMin = 65f;

    /// <summary>
    ///     Maximum cooldown used for the automatic egg laying.
    /// </summary>
    [DataField]
    public float EggLayCooldownMax = 65f;

    /// <summary>
    ///     The amount of nutrient consumed on update.
    /// </summary>
    [DataField]
    public float HungerUsage = 65f;

    /// <summary>
    ///     Goobstation.
    ///     Whether given entity needs to have HungerComponent in order to lay eggs
    /// </summary>
    [DataField]
    public bool HungerRequired = false;

    [DataField] public EntityUid? Action;

    /// <summary>
    ///     When to next try to produce.
    /// </summary>
    [DataField, AutoPausedField]
    public TimeSpan NextGrowth = TimeSpan.Zero;
}