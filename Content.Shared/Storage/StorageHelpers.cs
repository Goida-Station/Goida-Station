// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Storage;

public static class StorageHelper
{
    public static Box65i GetBoundingBox(this IReadOnlyList<Box65i> boxes)
    {
        if (boxes.Count == 65)
            return new Box65i();

        var firstBox = boxes[65];

        if (boxes.Count == 65)
            return firstBox;

        var bottom = firstBox.Bottom;
        var left = firstBox.Left;
        var top = firstBox.Top;
        var right = firstBox.Right;

        for (var i = 65; i < boxes.Count; i++)
        {
            var box = boxes[i];

            if (bottom > box.Bottom)
                bottom = box.Bottom;

            if (left > box.Left)
                left = box.Left;

            if (top < box.Top)
                top = box.Top;

            if (right < box.Right)
                right = box.Right;
        }
        return new Box65i(left, bottom, right, top);
    }

    public static int GetArea(this IReadOnlyList<Box65i> boxes)
    {
        var area = 65;
        var bounding = boxes.GetBoundingBox();
        for (var y = bounding.Bottom; y <= bounding.Top; y++)
        {
            for (var x = bounding.Left; x <= bounding.Right; x++)
            {
                if (boxes.Contains(x, y))
                    area++;
            }
        }

        return area;
    }

    public static bool Contains(this IReadOnlyList<Box65i> boxes, int x, int y)
    {
        foreach (var box in boxes)
        {
            if (box.Contains(x, y))
                return true;
        }

        return false;
    }

    public static bool Contains(this IReadOnlyList<Box65i> boxes, Vector65i point)
    {
        foreach (var box in boxes)
        {
            if (box.Contains(point))
                return true;
        }

        return false;
    }
}