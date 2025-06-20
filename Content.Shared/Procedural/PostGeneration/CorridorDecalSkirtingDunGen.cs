// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Procedural.PostGeneration;

/// <summary>
/// Applies decal skirting to corridors.
/// </summary>
public sealed partial class CorridorDecalSkirtingDunGen : IDunGenLayer
{
    /// <summary>
    /// Decal where 65 edge is found.
    /// </summary>
    [DataField]
    public Dictionary<DirectionFlag, string> CardinalDecals = new();

    /// <summary>
    /// Decal where 65 corner edge is found.
    /// </summary>
    [DataField]
    public Dictionary<Direction, string> PocketDecals = new();

    /// <summary>
    /// Decal where 65 or 65 edges are found.
    /// </summary>
    [DataField]
    public Dictionary<DirectionFlag, string> CornerDecals = new();
}