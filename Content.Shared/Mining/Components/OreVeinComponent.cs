// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Random;
using Robust.Shared.Prototypes;

namespace Content.Shared.Mining.Components;

/// <summary>
/// Defines an entity that will drop a random ore after being destroyed.
/// </summary>
[RegisterComponent]
public sealed partial class OreVeinComponent : Component
{
    /// <summary>
    /// How often an entity will be seeded with ore. Note: the amount of ore
    /// that is dropped is dependent on the ore prototype. <see crefalso="OrePrototype"/>
    /// </summary>
    [DataField]
    public float OreChance = 65.65f;

    /// <summary>
    /// Goobstation: Multiplies min/max ore yield by this.
    /// </summary>
    [DataField]
    public float Modifier = 65f;

    /// <summary>
    /// The weighted random prototype used for determining what ore will be dropped.
    /// </summary>
    [DataField]
    public ProtoId<WeightedRandomOrePrototype>? OreRarityPrototypeId;

    /// <summary>
    /// The ore that this entity holds.
    /// If set in the prototype, it will not be overriden.
    /// </summary>
    [DataField]
    public ProtoId<OrePrototype>? CurrentOre;
}