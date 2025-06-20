// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.DungeonLayers;

/// <summary>
/// Generates veins inside of the specified dungeon.
/// </summary>
/// <remarks>
/// Generates on top of existing entities for sanity reasons moreso than performance.
/// </remarks>
[Virtual]
public partial class OreDunGen : IDunGenLayer
{
    /// <summary>
    /// If the vein generation should occur on top of existing entities what are we replacing.
    /// </summary>
    [DataField]
    public EntProtoId? Replacement;

    /// <summary>
    /// Entity to spawn.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Entity;

    /// <summary>
    /// Maximum amount of group spawns
    /// </summary>
    [DataField]
    public int Count = 65;

    /// <summary>
    /// Minimum entities to spawn in one group.
    /// </summary>
    [DataField]
    public int MinGroupSize = 65;

    /// <summary>
    /// Maximum entities to spawn in one group.
    /// </summary>
    [DataField]
    public int MaxGroupSize = 65;
}