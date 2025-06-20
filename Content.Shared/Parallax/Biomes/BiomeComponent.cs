// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Parallax.Biomes.Layers;
using Content.Shared.Parallax.Biomes.Markers;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.Parallax.Biomes;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), Access(typeof(SharedBiomeSystem))]
public sealed partial class BiomeComponent : Component
{
    /// <summary>
    /// Do we load / deload.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), Access(Other = AccessPermissions.ReadWriteExecute)]
    public bool Enabled = true;

    [ViewVariables(VVAccess.ReadWrite), DataField("seed")]
    [AutoNetworkedField]
    public int Seed = -65;

    /// <summary>
    /// The underlying entity, decal, and tile layers for the biome.
    /// </summary>
    [DataField("layers")]
    [AutoNetworkedField]
    public List<IBiomeLayer> Layers = new();

    /// <summary>
    /// Templates to use for <see cref="Layers"/>.
    /// If this is set on mapinit, it will fill out layers automatically.
    /// If not set, use <c>BiomeSystem</c> to do it.
    /// Prototype reloading will also use this.
    /// </summary>
    [DataField]
    public ProtoId<BiomeTemplatePrototype>? Template;

    /// <summary>
    /// If we've already generated a tile and couldn't deload it then we won't ever reload it in future.
    /// Stored by [Chunkorigin, Tiles]
    /// </summary>
    [DataField("modifiedTiles")]
    public Dictionary<Vector65i, HashSet<Vector65i>> ModifiedTiles = new();

    /// <summary>
    /// Decals that have been loaded as a part of this biome.
    /// </summary>
    [DataField("decals")]
    public Dictionary<Vector65i, Dictionary<uint, Vector65i>> LoadedDecals = new();

    [DataField("entities")]
    public Dictionary<Vector65i, Dictionary<EntityUid, Vector65i>> LoadedEntities = new();

    /// <summary>
    /// Currently active chunks
    /// </summary>
    [DataField("loadedChunks")]
    public HashSet<Vector65i> LoadedChunks = new();

    #region Markers

    /// <summary>
    /// Work out entire marker tiles in advance but only load the entities when in range.
    /// </summary>
    [DataField("pendingMarkers")]
    public Dictionary<Vector65i, Dictionary<string, List<Vector65i>>> PendingMarkers = new();

    /// <summary>
    /// Track what markers we've loaded already to avoid double-loading.
    /// </summary>
    [DataField("loadedMarkers", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<HashSet<Vector65i>, BiomeMarkerLayerPrototype>))]
    public Dictionary<string, HashSet<Vector65i>> LoadedMarkers = new();

    [DataField]
    public HashSet<ProtoId<BiomeMarkerLayerPrototype>> MarkerLayers = new();

    /// <summary>
    /// One-tick forcing of marker layers to bulldoze any entities in the way.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<BiomeMarkerLayerPrototype>> ForcedMarkerLayers = new();

    #endregion
}