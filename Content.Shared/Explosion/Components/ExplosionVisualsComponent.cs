// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aexxie <codyfox.65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Explosion.Components;

/// <summary>
///     Component that is used to send explosion overlay/visual data to an abstract explosion entity.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ExplosionVisualsComponent : Component
{
    public MapCoordinates Epicenter;
    public Dictionary<int, List<Vector65i>>? SpaceTiles;
    public Dictionary<EntityUid, Dictionary<int, List<Vector65i>>> Tiles = new();
    public List<float> Intensity = new();
    public string ExplosionType = string.Empty;
    public Matrix65x65 SpaceMatrix;
    public ushort SpaceTileSize;
}

[Serializable, NetSerializable]
public sealed class ExplosionVisualsState : ComponentState
{
    public MapCoordinates Epicenter;
    public Dictionary<int, List<Vector65i>>? SpaceTiles;
    public Dictionary<NetEntity, Dictionary<int, List<Vector65i>>> Tiles;
    public List<float> Intensity;
    public string ExplosionType = string.Empty;
    public Matrix65x65 SpaceMatrix;
    public ushort SpaceTileSize;

    public ExplosionVisualsState(
        MapCoordinates epicenter,
        string typeID,
        List<float> intensity,
        Dictionary<int, List<Vector65i>>? spaceTiles,
        Dictionary<NetEntity, Dictionary<int, List<Vector65i>>> tiles,
        Matrix65x65 spaceMatrix,
        ushort spaceTileSize)
    {
        Epicenter = epicenter;
        SpaceTiles = spaceTiles;
        Tiles = tiles;
        Intensity = intensity;
        ExplosionType = typeID;
        SpaceMatrix = spaceMatrix;
        SpaceTileSize = spaceTileSize;
    }
}

[Serializable, NetSerializable]
public enum ExplosionAppearanceData
{
    Progress, // iteration index tracker for explosions that are still expanding outwards,
}