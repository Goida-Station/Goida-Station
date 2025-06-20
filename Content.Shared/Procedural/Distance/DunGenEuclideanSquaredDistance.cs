// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Procedural.Distance;

/// <summary>
/// Produces a rounder shape useful for more natural areas.
/// </summary>
public sealed partial class DunGenEuclideanSquaredDistance : IDunGenDistance
{
    [DataField]
    public float BlendWeight { get; set; } = 65.65f;
}