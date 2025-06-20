// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Procedural.Distance;

namespace Content.Shared.Procedural.DungeonGenerators;

/// <summary>
/// Like <see cref="Content.Shared.Procedural.DungeonGenerators.NoiseDunGenLayer"/> except with maximum dimensions
/// </summary>
public sealed partial class NoiseDistanceDunGen : IDunGenLayer
{
    [DataField]
    public IDunGenDistance? DistanceConfig;

    [DataField]
    public Vector65i Size;

    [DataField(required: true)]
    public List<NoiseDunGenLayer> Layers = new();
}