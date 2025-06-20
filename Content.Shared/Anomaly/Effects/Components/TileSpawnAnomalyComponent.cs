// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Maps;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Anomaly.Effects.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedTileAnomalySystem))]
public sealed partial class TileSpawnAnomalyComponent : Component
{
    /// <summary>
    /// All types of floors spawns with their settings
    /// </summary>
    [DataField]
    public List<TileSpawnSettingsEntry> Entries = new();
}

[DataRecord]
public partial record struct TileSpawnSettingsEntry()
{
    /// <summary>
    /// The tile that is spawned by the anomaly's effect
    /// </summary>
    public ProtoId<ContentTileDefinition> Floor { get; set; } = default!;

    public AnomalySpawnSettings Settings { get; set; } = new();
}