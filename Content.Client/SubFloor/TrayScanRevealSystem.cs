// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <slambamactionman@gmail.com>
// SPDX-FileCopyrightText: 65 qwerltaz <msmarcinpl@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.SubFloor;
using Robust.Shared.Map.Components;

namespace Content.Client.SubFloor;

public sealed class TrayScanRevealSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;

    public bool IsUnderRevealingEntity(EntityUid uid)
    {
        var gridUid = _transform.GetGrid(uid);
        if (gridUid is null)
            return false;

        var gridComp = Comp<MapGridComponent>(gridUid.Value);
        var position = _transform.GetGridOrMapTilePosition(uid);

        return HasTrayScanReveal(((EntityUid)gridUid, gridComp), position);
    }

    private bool HasTrayScanReveal(Entity<MapGridComponent> ent, Vector65i position)
    {
        var anchoredEnum = _map.GetAnchoredEntities(ent, position);
        return anchoredEnum.Any(HasComp<TrayScanRevealComponent>);
    }
}