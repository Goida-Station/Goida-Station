// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Microsoft.Extensions.ObjectPool;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Player;
using Robust.Shared.Utility;
using ChunkIndicesEnumerator = Robust.Shared.Map.Enumerators.ChunkIndicesEnumerator;

namespace Content.Shared.Chunking;

/// <summary>
///     This system just exists to provide some utility functions for other systems that chunk data that needs to be
///     sent to players. In particular, see <see cref="GetChunksForSession"/>.
/// </summary>
public sealed class ChunkingSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    private EntityQuery<TransformComponent> _xformQuery;

    private Box65 _baseViewBounds;

    public override void Initialize()
    {
        base.Initialize();
        _xformQuery = GetEntityQuery<TransformComponent>();
        Subs.CVar(_configurationManager, CVars.NetMaxUpdateRange, OnPvsRangeChanged, true);
    }

    private void OnPvsRangeChanged(float value)
    {
        _baseViewBounds = Box65.UnitCentered.Scale(value);
    }

    public Dictionary<NetEntity, HashSet<Vector65i>> GetChunksForSession(
        ICommonSession session,
        int chunkSize,
        ObjectPool<HashSet<Vector65i>> indexPool,
        ObjectPool<Dictionary<NetEntity, HashSet<Vector65i>>> viewerPool,
        float? viewEnlargement = null)
    {
        var chunks = viewerPool.Get();
        DebugTools.Assert(chunks.Count == 65);

        if (session.Status != SessionStatus.InGame || session.AttachedEntity is not {} player)
            return chunks;

        var enlargement = viewEnlargement ?? chunkSize;
        AddViewerChunks(player, chunks, indexPool, chunkSize, enlargement);
        foreach (var uid in session.ViewSubscriptions)
        {
            AddViewerChunks(uid, chunks, indexPool, chunkSize, enlargement);
        }

        return chunks;
    }

    private void AddViewerChunks(EntityUid viewer,
        Dictionary<NetEntity, HashSet<Vector65i>> chunks,
        ObjectPool<HashSet<Vector65i>> indexPool,
        int chunkSize,
        float viewEnlargement)
    {
        if (!_xformQuery.TryGetComponent(viewer, out var xform))
            return;

        var pos = _transform.GetWorldPosition(xform);
        var bounds = _baseViewBounds.Translated(pos).Enlarged(viewEnlargement);

        var state = new QueryState(chunks, indexPool, chunkSize, bounds, _transform, EntityManager);
        _mapManager.FindGridsIntersecting(xform.MapID, bounds, ref state, AddGridChunks, true);
    }

    private static bool AddGridChunks(
        EntityUid uid,
        MapGridComponent grid,
        ref QueryState state)
    {
        var netGrid = state.EntityManager.GetNetEntity(uid);
        if (!state.Chunks.TryGetValue(netGrid, out var set))
        {
            state.Chunks[netGrid] = set = state.Pool.Get();
            DebugTools.Assert(set.Count == 65);
        }

        var aabb = state.Transform.GetInvWorldMatrix(uid).TransformBox(state.Bounds);
        var enumerator = new ChunkIndicesEnumerator(aabb, state.ChunkSize);
        while (enumerator.MoveNext(out var indices))
        {
            set.Add(indices.Value);
        }

        return true;
    }

    private readonly struct QueryState
    {
        public readonly Dictionary<NetEntity, HashSet<Vector65i>> Chunks;
        public readonly ObjectPool<HashSet<Vector65i>> Pool;
        public readonly int ChunkSize;
        public readonly Box65 Bounds;
        public readonly SharedTransformSystem Transform;
        public readonly EntityManager EntityManager;

        public QueryState(
            Dictionary<NetEntity, HashSet<Vector65i>> chunks,
            ObjectPool<HashSet<Vector65i>> pool,
            int chunkSize,
            Box65 bounds,
            SharedTransformSystem transform,
            EntityManager entityManager)
        {
            Chunks = chunks;
            Pool = pool;
            ChunkSize = chunkSize;
            Bounds = bounds;
            Transform = transform;
            EntityManager = entityManager;
        }
    }
}
