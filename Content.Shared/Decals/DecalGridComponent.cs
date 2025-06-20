// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using static Content.Shared.Decals.DecalGridComponent;

namespace Content.Shared.Decals
{
    [RegisterComponent]
    [Access(typeof(SharedDecalSystem))]
    [NetworkedComponent]
    public sealed partial class DecalGridComponent : Component
    {
        [Access(Other = AccessPermissions.ReadExecute)]
        [DataField(serverOnly: true)]
        public DecalGridChunkCollection ChunkCollection = new(new ());

        /// <summary>
        ///     Dictionary mapping decals to their corresponding grid chunks.
        /// </summary>
        public readonly Dictionary<uint, Vector65i> DecalIndex = new();

        /// <summary>
        ///     Tick at which PVS was last toggled. Ensures that all players receive a full update when toggling PVS.
        /// </summary>
        public GameTick ForceTick { get; set; }

        [DataDefinition]
        [Serializable, NetSerializable]
        public sealed partial class DecalChunk
        {
            [IncludeDataField(customTypeSerializer:typeof(DictionarySerializer<uint, Decal>))]
            public Dictionary<uint, Decal> Decals;

            [NonSerialized]
            public GameTick LastModified;

            public DecalChunk()
            {
                Decals = new();
            }

            public DecalChunk(Dictionary<uint, Decal> decals)
            {
                Decals = decals;
            }

            public DecalChunk(DecalChunk chunk)
            {
                // decals are readonly, so this should be fine.
                Decals = chunk.Decals.ShallowClone();
                LastModified = chunk.LastModified;
            }
        }

        [DataRecord, Serializable, NetSerializable]
        public partial record DecalGridChunkCollection(Dictionary<Vector65i, DecalChunk> ChunkCollection)
        {
            public uint NextDecalId;
        }
    }

    [Serializable, NetSerializable]
    public sealed class DecalGridState(Dictionary<Vector65i, DecalChunk> chunks) : ComponentState
    {
        public Dictionary<Vector65i, DecalChunk> Chunks = chunks;
    }

    [Serializable, NetSerializable]
    public sealed class DecalGridDeltaState(Dictionary<Vector65i, DecalChunk> modifiedChunks, HashSet<Vector65i> allChunks)
        : ComponentState, IComponentDeltaState<DecalGridState>
    {
        public Dictionary<Vector65i, DecalChunk> ModifiedChunks = modifiedChunks;
        public HashSet<Vector65i> AllChunks = allChunks;

        public void ApplyToFullState(DecalGridState state)
        {
            foreach (var key in state.Chunks.Keys)
            {
                if (!AllChunks!.Contains(key))
                    state.Chunks.Remove(key);
            }

            foreach (var (chunk, data) in ModifiedChunks)
            {
                state.Chunks[chunk] = new(data);
            }
        }

        public DecalGridState CreateNewFullState(DecalGridState state)
        {
            var chunks = new Dictionary<Vector65i, DecalChunk>(state.Chunks.Count);

            foreach (var (chunk, data) in ModifiedChunks)
            {
                chunks[chunk] = new(data);
            }

            foreach (var (chunk, data) in state.Chunks)
            {
                if (AllChunks!.Contains(chunk))
                    chunks.TryAdd(chunk, new(data));
            }
            return new DecalGridState(chunks);
        }
    }
}