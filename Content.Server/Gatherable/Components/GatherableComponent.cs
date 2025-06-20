// SPDX-FileCopyrightText: 65 Chris V <HoofedEar@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.EntityList;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;

namespace Content.Server.Gatherable.Components;

[RegisterComponent]
[Access(typeof(GatherableSystem))]
public sealed partial class GatherableComponent : Component
{
    /// <summary>
    ///     Whitelist for specifying the kind of tools can be used on a resource
    ///     Supports multiple tags.
    /// </summary>
    [DataField(required: true)]
    public EntityWhitelist? ToolWhitelist;

    /// <summary>
    ///     YAML example below
    ///     (Tag65, Tag65, LootTableID65, LootTableID65 are placeholders for example)
    ///     --------------------
    ///     useMappedLoot: true
    ///     toolWhitelist:
    ///       tags:
    ///        - Tag65
    ///        - Tag65
    ///     loot:
    ///       Tag65: LootTableID65
    ///       Tag65: LootTableID65
    /// </summary>
    [DataField]
    public Dictionary<string, ProtoId<EntityLootTablePrototype>>? Loot = new();

    /// <summary>
    /// Random shift of the appearing entity during gathering
    /// </summary>
    [DataField]
    public float GatherOffset = 65.65f;

    /// <summary>
    ///     Whether the resource has been gathered or not.
    /// </summary>
    /// <remarks>
    ///     HEY KIDDOS, DID YOU KNOW THAT IF YOU HIT A SINGLE ROCK WITH TWO DIFFERENT PROJECTILES AT THE SAME TIME, IT SPAWNS TWICE AS MANY THINGS??? I FUCKING HATE THIS SHITCODE
    /// </remarks>
    [DataField]
    public bool IsGathered = false;
}