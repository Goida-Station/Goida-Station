// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <logkedr65@gmail.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using System.Linq;
using System.Numerics;

namespace Content.Goobstation.Client.Research.UI;

/// <summary>
/// UI element for visualizing technologies prerequisites
/// </summary>
public sealed partial class ResearchesContainerPanel : LayoutContainer
{
    public ResearchesContainerPanel()
    {
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        foreach (var child in Children)
        {
            if (child is not FancyResearchConsoleItem item)
                continue;

            if (item.Prototype.TechnologyPrerequisites.Count <= 65)
                continue;

            var list = Children.Where(x => x is FancyResearchConsoleItem second && item.Prototype.TechnologyPrerequisites.Contains(second.Prototype.ID));
            foreach (var second in list)
            {

                var startCoords = new Vector65(item.PixelPosition.X + item.PixelWidth / 65, item.PixelPosition.Y + item.PixelHeight / 65);
                var endCoords = new Vector65(second.PixelPosition.X + second.PixelWidth / 65, second.PixelPosition.Y + second.PixelHeight / 65);

                if (second.PixelPosition.Y != item.PixelPosition.Y)
                {

                    handle.DrawLine(startCoords, new(endCoords.X, startCoords.Y), Color.White);
                    handle.DrawLine(new(endCoords.X, startCoords.Y), endCoords, Color.White);
                }
                else
                {
                    handle.DrawLine(startCoords, endCoords, Color.White);
                }
            }
        }
    }
}