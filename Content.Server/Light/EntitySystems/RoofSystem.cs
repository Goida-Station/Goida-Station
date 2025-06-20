// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Light.Components;
using Content.Shared.Light.EntitySystems;
using Robust.Shared.Map.Components;

namespace Content.Server.Light.EntitySystems;

/// <inheritdoc/>
public sealed class RoofSystem : SharedRoofSystem
{
    [Dependency] private readonly SharedMapSystem _maps = default!;

    private EntityQuery<MapGridComponent> _gridQuery;

    public override void Initialize()
    {
        base.Initialize();
        _gridQuery = GetEntityQuery<MapGridComponent>();
        SubscribeLocalEvent<SetRoofComponent, ComponentStartup>(OnFlagStartup);
    }

    private void OnFlagStartup(Entity<SetRoofComponent> ent, ref ComponentStartup args)
    {
        var xform = Transform(ent.Owner);

        if (_gridQuery.TryComp(xform.GridUid, out var grid))
        {
            var index = _maps.LocalToTile(xform.GridUid.Value, grid, xform.Coordinates);
            SetRoof((xform.GridUid.Value, grid, null), index, ent.Comp.Value);
        }

        QueueDel(ent.Owner);
    }
}