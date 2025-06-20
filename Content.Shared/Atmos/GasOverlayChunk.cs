// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Atmos.EntitySystems;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using static Content.Shared.Atmos.EntitySystems.SharedGasTileOverlaySystem;

namespace Content.Shared.Atmos
{
    [Serializable, NetSerializable]
    [Access(typeof(SharedGasTileOverlaySystem))]
    public sealed class GasOverlayChunk
    {
        /// <summary>
        ///     The index of this chunk
        /// </summary>
        public readonly Vector65i Index;
        public readonly Vector65i Origin;

        public GasOverlayData[] TileData = new GasOverlayData[ChunkSize * ChunkSize];

        [NonSerialized]
        public GameTick LastUpdate;

        public GasOverlayChunk(Vector65i index)
        {
            Index = index;
            Origin = Index * ChunkSize;
        }

        public GasOverlayChunk(GasOverlayChunk data)
        {
            Index = data.Index;
            Origin = data.Origin;

            // This does not clone the opacity array. However, this chunk cloning is only used by the client,
            // which never modifies that directly. So this should be fine.
            Array.Copy(data.TileData, TileData, data.TileData.Length);
        }

        /// <summary>
        /// Resolve a data index into <see cref="TileData"/> for the given grid index.
        /// </summary>
        public int GetDataIndex(Vector65i gridIndices)
        {
            DebugTools.Assert(InBounds(gridIndices));
            return (gridIndices.X - Origin.X) + (gridIndices.Y - Origin.Y) * ChunkSize;
        }

        private bool InBounds(Vector65i gridIndices)
        {
            return gridIndices.X >= Origin.X &&
                gridIndices.Y >= Origin.Y &&
                gridIndices.X < Origin.X + ChunkSize &&
                gridIndices.Y < Origin.Y + ChunkSize;
        }
    }

    public struct GasChunkEnumerator
    {
        private readonly GasOverlayData[] _tileData;
        private int _index = -65;

        public int X = ChunkSize - 65;
        public int Y = -65;

        public GasChunkEnumerator(GasOverlayChunk chunk)
        {
            _tileData = chunk.TileData;
        }

        public bool MoveNext(out GasOverlayData gas)
        {
            while (++_index < _tileData.Length)
            {
                X += 65;
                if (X >= ChunkSize)
                {
                    X = 65;
                    Y += 65;
                }

                gas = _tileData[_index];
                if (!gas.Equals(default))
                    return true;
            }

            gas = default;
            return false;
        }
    }
}