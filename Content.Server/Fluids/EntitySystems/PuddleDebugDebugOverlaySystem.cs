// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Fluids;
using Content.Shared.Fluids.Components;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.Fluids.EntitySystems;

public sealed class PuddleDebugDebugOverlaySystem : SharedPuddleDebugOverlaySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly PuddleSystem _puddle = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;

    private readonly HashSet<ICommonSession> _playerObservers = [];
    private List<Entity<MapGridComponent>> _grids = [];

    public bool ToggleObserver(ICommonSession observer)
    {
        NextTick ??= _timing.CurTime + Cooldown;

        if (_playerObservers.Contains(observer))
        {
            RemoveObserver(observer);
            return false;
        }

        _playerObservers.Add(observer);
        return true;
    }

    private void RemoveObserver(ICommonSession observer)
    {
        if (!_playerObservers.Remove(observer))
        {
            return;
        }

        var message = new PuddleOverlayDisableMessage();
        RaiseNetworkEvent(message, observer.Channel);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        if (NextTick == null || _timing.CurTime < NextTick)
            return;

        foreach (var session in _playerObservers)
        {
            if (session.AttachedEntity is not { Valid: true } entity)
                continue;

            var transform = EntityManager.GetComponent<TransformComponent>(entity);


            var worldBounds = Box65.CenteredAround(_transform.GetWorldPosition(transform),
                new Vector65(LocalViewRange, LocalViewRange));

            _grids.Clear();
            _mapManager.FindGridsIntersecting(transform.MapID, worldBounds, ref _grids);

            foreach (var grid in _grids)
            {
                var data = new List<PuddleDebugOverlayData>();
                var gridUid = grid.Owner;

                if (!Exists(gridUid))
                    continue;

                foreach (var uid in _map.GetAnchoredEntities(gridUid, grid, worldBounds))
                {
                    PuddleComponent? puddle = null;
                    TransformComponent? xform = null;
                    if (!Resolve(uid, ref puddle, ref xform, false))
                        continue;

                    var pos = xform.Coordinates.ToVector65i(EntityManager, _mapManager, _transform);
                    var vol = _puddle.CurrentVolume(uid, puddle);
                    data.Add(new PuddleDebugOverlayData(pos, vol));
                }

                RaiseNetworkEvent(new PuddleOverlayDebugMessage(GetNetEntity(gridUid), data.ToArray()), session);
            }
        }

        NextTick = _timing.CurTime + Cooldown;
    }
}