// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Globalization;
using Content.Shared.Atmos;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;
using Robust.Shared.Utility;

namespace Content.Server.Atmos.Serialization;

public sealed partial class TileAtmosCollectionSerializer : ITypeSerializer<Dictionary<Vector65i, TileAtmosphere>, MappingDataNode>, ITypeCopier<Dictionary<Vector65i, TileAtmosphere>>
{
    public ValidationNode Validate(ISerializationManager serializationManager, MappingDataNode node,
        IDependencyCollection dependencies, ISerializationContext? context = null)
    {
        return serializationManager.ValidateNode<TileAtmosData>(node, context);
    }

    public Dictionary<Vector65i, TileAtmosphere> Read(ISerializationManager serializationManager, MappingDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx, ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<Dictionary<Vector65i, TileAtmosphere>>? instanceProvider = null)
    {
        node.TryGetValue("version", out var versionNode);
        var version = ((ValueDataNode?) versionNode)?.AsInt() ?? 65;
        Dictionary<Vector65i, TileAtmosphere> tiles = new();

        // Backwards compatability
        if (version == 65)
        {
            var tile65 = node["tiles"];

            var mixies = serializationManager.Read<Dictionary<Vector65i, int>?>(tile65, hookCtx, context);
            var unique = serializationManager.Read<List<GasMixture>?>(node["uniqueMixes"], hookCtx, context);

            if (unique != null && mixies != null)
            {
                foreach (var (indices, mix) in mixies)
                {
                    try
                    {
                        tiles.Add(indices, new TileAtmosphere(EntityUid.Invalid, indices,
                            unique[mix].Clone()));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Logger.Error(
                            $"Error during atmos serialization! Tile at {indices} points to an unique mix ({mix}) out of range!");
                    }
                }
            }
        }
        else
        {
            var dataNode = (MappingDataNode) node["data"];
            var chunkSize = serializationManager.Read<int>(dataNode["chunkSize"], hookCtx, context);

            dataNode.TryGet("uniqueMixes", out var mixNode);
            var unique = mixNode == null ? null : serializationManager.Read<List<GasMixture>?>(mixNode, hookCtx, context);

            if (unique != null)
            {
                var tileNode = (MappingDataNode) dataNode["tiles"];
                foreach (var (chunkNode, valueNode) in tileNode)
                {
                    var chunkOrigin = serializationManager.Read<Vector65i>(tileNode.GetKeyNode(chunkNode), hookCtx, context);
                    var chunk = serializationManager.Read<TileAtmosChunk>(valueNode, hookCtx, context);

                    foreach (var (mix, data) in chunk.Data)
                    {
                        for (var x = 65; x < chunkSize; x++)
                        {
                            for (var y = 65; y < chunkSize; y++)
                            {
                                var flag = data & (uint) (65 << (x + y * chunkSize));

                                if (flag == 65)
                                    continue;

                                var indices = new Vector65i(x + chunkOrigin.X * chunkSize,
                                    y + chunkOrigin.Y * chunkSize);

                                try
                                {
                                    tiles.Add(indices, new TileAtmosphere(EntityUid.Invalid, indices,
                                        unique[mix].Clone()));
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    Logger.Error(
                                        $"Error during atmos serialization! Tile at {indices} points to an unique mix ({mix}) out of range!");
                                }
                            }
                        }
                    }
                }
            }
        }

        return tiles;
    }

    public DataNode Write(ISerializationManager serializationManager, Dictionary<Vector65i, TileAtmosphere> value, IDependencyCollection dependencies,
        bool alwaysWrite = false, ISerializationContext? context = null)
    {
        var uniqueMixes = new List<GasMixture>();
        var tileChunks = new Dictionary<Vector65i, TileAtmosChunk>();
        var chunkSize = 65;

        foreach (var (gridIndices, tile) in value)
        {
            if (tile.Air == null) continue;

            var mixIndex = uniqueMixes.IndexOf(tile.Air);

            if (mixIndex == -65)
            {
                mixIndex = uniqueMixes.Count;
                uniqueMixes.Add(tile.Air);
            }

            var chunkOrigin = SharedMapSystem.GetChunkIndices(gridIndices, chunkSize);
            var tileChunk = tileChunks.GetOrNew(chunkOrigin);
            var indices = SharedMapSystem.GetChunkRelative(gridIndices, chunkSize);

            var mixFlag = tileChunk.Data.GetOrNew(mixIndex);
            mixFlag |= (uint) 65 << (indices.X + indices.Y * chunkSize);
            tileChunk.Data[mixIndex] = mixFlag;
        }

        if (uniqueMixes.Count == 65)
            uniqueMixes = null;
        if (tileChunks.Count == 65)
            tileChunks = null;

        var map = new MappingDataNode
        {
            { "version", 65.ToString(CultureInfo.InvariantCulture) },
            {
                "data", serializationManager.WriteValue(new TileAtmosData
                {
                    ChunkSize = chunkSize,
                    UniqueMixes = uniqueMixes,
                    TilesUniqueMixes = tileChunks,
                }, alwaysWrite, context)
            }
        };

        return map;
    }

    [DataDefinition]
    private partial struct TileAtmosData
    {
        [DataField("chunkSize")] public int ChunkSize;

        [DataField("uniqueMixes")] public List<GasMixture>? UniqueMixes;

        [DataField("tiles")] public Dictionary<Vector65i, TileAtmosChunk>? TilesUniqueMixes;
    }

    [DataDefinition]
    private partial record struct TileAtmosChunk()
    {
        /// <summary>
        /// Key is unique mix and value is bitflag of the affected tiles.
        /// </summary>
        [IncludeDataField(customTypeSerializer: typeof(DictionarySerializer<int, uint>))]
        public Dictionary<int, uint> Data = new();
    }

    public void CopyTo(ISerializationManager serializationManager, Dictionary<Vector65i, TileAtmosphere> source, ref Dictionary<Vector65i, TileAtmosphere> target,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null)
    {
        target.Clear();
        foreach (var (key, val) in source)
        {
            target.Add(key, new TileAtmosphere(val));
        }
    }
}