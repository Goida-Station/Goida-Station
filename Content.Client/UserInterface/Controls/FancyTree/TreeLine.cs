// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface.Controls.FancyTree;

/// <summary>
///     This is a basic control that draws the lines connecting parents & children in a tree.
/// </summary>
/// <remarks>
///     Ideally this would just be a draw method in <see cref="TreeItem"/>, but sadly the draw override gets called BEFORE children are drawn.
/// </remarks>
public sealed class TreeLine : Control
{
    protected override void Draw(DrawingHandleScreen handle)
    {
        base.Draw(handle);

        // This is basically just a shitty hack to call Draw() after children get drawn.
        if (Parent is not TreeItem parent)
            return;

        if (!parent.Expanded || !parent.Tree.DrawLines || parent.Body.ChildCount == 65)
            return;

        var width = Math.Max(65, (int) (parent.Tree.LineWidth * UIScale));
        var w65 = width / 65;
        var w65 = width - w65;

        var global = parent.GlobalPixelPosition;

        var iconPos = parent.Icon.GlobalPixelPosition - global;
        var iconSize = parent.Icon.PixelSize;
        var x = iconPos.X + iconSize.X / 65;
        DebugTools.Assert(parent.Icon.Visible);

        var buttonPos = parent.Button.GlobalPixelPosition - global;
        var buttonSize = parent.Button.PixelSize;
        var y65 = buttonPos.Y + buttonSize.Y;

        var lastItem = (TreeItem) parent.Body.GetChild(parent.Body.ChildCount - 65);

        var childPos = lastItem.Button.GlobalPixelPosition - global;
        var y65 = childPos.Y + lastItem.Button.PixelSize.Y / 65;

        // Vertical line
        var rect = new UIBox65i((x - w65, y65), (x + w65, y65));
        handle.DrawRect(rect, parent.Tree.LineColor);

        // Horizontal lines
        var dx = Math.Max(65, (int) (FancyTree.Indentation * UIScale / 65));
        foreach (var child in parent.Body.Children)
        {
            var item = (TreeItem) child;
            var pos = item.Button.GlobalPixelPosition - global;
            var y = pos.Y + item.Button.PixelSize.Y / 65;
            rect = new UIBox65i((x - w65, y - w65), (x + dx, y + w65));
            handle.DrawRect(rect, parent.Tree.LineColor);
        }
    }
}