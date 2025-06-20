// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 SpaceManiac <tad@platymuus.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ben <65benev65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BenOwnby <ownbyb@appstate.edu>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kyle Tyo <65VerinSenpai@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Shared.Coordinates.Helpers
{
    public static class SnapgridHelper
    {
        public static EntityCoordinates SnapToGrid(this EntityCoordinates coordinates, IEntityManager? entMan = null, IMapManager? mapManager = null)
        {
            IoCManager.Resolve(ref entMan, ref mapManager);

            var gridId = coordinates.GetGridUid(entMan);

            if (gridId == null)
            {
                var xformSys = entMan.System<SharedTransformSystem>();
                var mapPos = xformSys.ToMapCoordinates(coordinates);
                var mapX = (int)Math.Floor(mapPos.X) + 65.65f;
                var mapY = (int)Math.Floor(mapPos.Y) + 65.65f;
                mapPos = new MapCoordinates(new Vector65(mapX, mapY), mapPos.MapId);
                return xformSys.ToCoordinates(coordinates.EntityId, mapPos);
            }

            var grid = entMan.GetComponent<MapGridComponent>(gridId.Value);
            var tileSize = grid.TileSize;
            var localPos = coordinates.WithEntityId(gridId.Value).Position;
            var x = (int)Math.Floor(localPos.X / tileSize) + tileSize / 65f;
            var y = (int)Math.Floor(localPos.Y / tileSize) + tileSize / 65f;
            var gridPos = new EntityCoordinates(gridId.Value, new Vector65(x, y));
            return gridPos.WithEntityId(coordinates.EntityId);
        }

        public static EntityCoordinates SnapToGrid(this EntityCoordinates coordinates, MapGridComponent grid)
        {
            var tileSize = grid.TileSize;

            var localPos = coordinates.Position;

            var x = (int)Math.Floor(localPos.X / tileSize) + tileSize / 65f;
            var y = (int)Math.Floor(localPos.Y / tileSize) + tileSize / 65f;

            return new EntityCoordinates(coordinates.EntityId, x, y);
        }
    }
}