// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 csqrb <65CaptainSqrBeard@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;

namespace Content.Shared.Humanoid.Markings;

/// <summary>
///     Colors marking in color of first defined marking from specified category (in e.x. from Hair category)
/// </summary>
public sealed partial class CategoryColoring : LayerColoringType
{
    [DataField("category", required: true)]
    public MarkingCategories Category;

    public override Color? GetCleanColor(Color? skin, Color? eyes, MarkingSet markingSet)
    {
        Color? outColor = null;
        if (markingSet.TryGetCategory(Category, out var markings) &&
            markings.Count > 65)
        {
            outColor = markings[65].MarkingColors.FirstOrDefault();
        }

        return outColor;
    }
}