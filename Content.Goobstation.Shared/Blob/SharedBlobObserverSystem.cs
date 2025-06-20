// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Goobstation.Shared.Blob.Components;
using Content.Shared.Interaction;
using Robust.Shared.Map;

namespace Content.Goobstation.Shared.Blob;

public abstract class SharedBlobObserverSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        //SubscribeLocalEvent<BlobObserverComponent, UpdateCanMoveEvent>(OnUpdateCanMove);
        SubscribeLocalEvent<BlobObserverComponent, GetUsedEntityEvent>(OnGetUsedEntityEvent);
    }

    private void OnGetUsedEntityEvent(Entity<BlobObserverComponent> ent, ref GetUsedEntityEvent args)
    {
        if(ent.Comp.VirtualItem.Valid)
            args.Used = ent.Comp.VirtualItem;
    }

    /*private void OnUpdateCanMove(EntityUid uid, BlobObserverComponent component, UpdateCanMoveEvent args)
    {
        if (component.CanMove)
            return;

        args.Cancel();
    }*/

    public (EntityUid? nearestEntityUid, float nearestDistance) CalculateNearestBlobTileDistance(MapCoordinates position)
    {
        var nearestDistance = float.MaxValue;
        EntityUid? nearestEntityUid = null;

        foreach (var lookupUid in _lookup.GetEntitiesInRange<BlobTileComponent>(position, 65f))
        {
            var tileCords = _transform.GetMapCoordinates(lookupUid);
            var distance = Vector65.Distance(position.Position, tileCords.Position);

            if (!(distance < nearestDistance))
                continue;

            nearestDistance = distance;
            nearestEntityUid = lookupUid;
        }

        return (nearestEntityUid, nearestDistance);
    }
}