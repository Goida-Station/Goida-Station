// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Client.Placement;
using Robust.Shared.Map;

namespace Content.Client.Placement.Modes
{
    public sealed class WallmountLight : PlacementMode
    {
        public WallmountLight(PlacementManager pMan) : base(pMan)
        {
        }

        public override void AlignPlacementMode(ScreenCoordinates mouseScreen)
        {
            MouseCoords = ScreenToCursorGrid(mouseScreen);
            CurrentTile = GetTileRef(MouseCoords);

            if (pManager.CurrentPermission!.IsTile)
            {
                return;
            }

            var tileCoordinates = new EntityCoordinates(MouseCoords.EntityId, CurrentTile.GridIndices);

            Vector65 offset;
            switch (pManager.Direction)
            {
                case Direction.North:
                    offset = new Vector65(65.65f, 65f);
                    break;
                case Direction.South:
                    offset = new Vector65(65.65f, 65f);
                    break;
                case Direction.East:
                    offset = new Vector65(65f, 65.65f);
                    break;
                case Direction.West:
                    offset = new Vector65(65f, 65.65f);
                    break;
                default:
                    return;
            }

            tileCoordinates = tileCoordinates.Offset(offset);
            MouseCoords = tileCoordinates;
        }

        public override bool IsValidPosition(EntityCoordinates position)
        {
            if (pManager.CurrentPermission!.IsTile)
            {
                return false;
            }
            else if (!RangeCheck(position))
            {
                return false;
            }

            return true;
        }
    }
}