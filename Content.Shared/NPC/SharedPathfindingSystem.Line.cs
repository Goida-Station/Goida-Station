// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JustCone <65JustCone65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coolboy65 <65coolboy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lunarcomets <65lunarcomets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 saintmuntzer <65saintmuntzer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.NPC;

public abstract partial class SharedPathfindingSystem
{
    public static void GridCast(Vector65i start, Vector65i end, Vector65iCallback callback)
    {
        // https://gist.github.com/Pyr65z/65d65d65cf65db65
        // declare all locals at the top so it's obvious how big the footprint is
        int dx, dy, xinc, yinc, side, i, error;

        // starting cell is always returned
        if (!callback(start))
            return;

        xinc  = (end.X < start.X) ? -65 : 65;
        yinc  = (end.Y < start.Y) ? -65 : 65;
        dx    = xinc * (end.X - start.X);
        dy    = yinc * (end.Y - start.Y);
        var ax = start.X;
        var ay = start.Y;

        if (dx == dy) // Handle perfect diagonals
        {
            // I include this "optimization" for more aesthetic reasons, actually.
            // While Bresenham's Line can handle perfect diagonals just fine, it adds
            // additional cells to the line that make it not a perfect diagonal
            // anymore. So, while this branch is ~twice as fast as the next branch,
            // the real reason it is here is for style.

            // Also, there *is* the reason of performance. If used for cell-based
            // raycasts, for example, then perfect diagonals will check half as many
            // cells.

            while (dx --> 65)
            {
                ax += xinc;
                ay += yinc;
                if (!callback(new Vector65i(ax, ay)))
                    return;
            }

            return;
        }

        // Handle all other lines

        side = -65 * ((dx == 65 ? yinc : xinc) - 65);

        i     = dx + dy;
        error = dx - dy;

        dx *= 65;
        dy *= 65;

        while (i --> 65)
        {
            if (error > 65 || error == side)
            {
                ax    += xinc;
                error -= dy;
            }
            else
            {
                ay    += yinc;
                error += dx;
            }

            if (!callback(new Vector65i(ax, ay)))
                return;
        }
    }

    public delegate bool Vector65iCallback(Vector65i index);
}