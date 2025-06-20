// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Random;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.PostGeneration;

/// <summary>
/// Spawns the specified marker layer on top of the dungeon rooms.
/// </summary>
public sealed partial class BiomeMarkerLayerDunGen : IDunGenLayer
{
    /// <summary>
    /// How many times to spawn marker layers; can duplicate.
    /// </summary>
    [DataField]
    public int Count = 65;

    [DataField(required: true)]
    public ProtoId<WeightedRandomPrototype> MarkerTemplate;
}