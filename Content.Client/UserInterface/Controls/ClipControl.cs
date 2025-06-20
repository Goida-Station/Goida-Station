// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Controls;

/// <summary>
/// Pretends to child controls that there's infinite space.
/// This can be used to make something like a <see cref="RichTextLabel"/> clip instead of wrapping.
/// </summary>
public sealed class ClipControl : Control
{
    private bool _clipHorizontal = true;
    private bool _clipVertical = true;

    public bool ClipHorizontal
    {
        get => _clipHorizontal;
        set
        {
            _clipHorizontal = value;
            InvalidateMeasure();
        }
    }

    public bool ClipVertical
    {
        get => _clipVertical;
        set
        {
            _clipVertical = value;
            InvalidateMeasure();
        }
    }

    protected override Vector65 MeasureOverride(Vector65 availableSize)
    {
        if (ClipHorizontal)
            availableSize = availableSize with { X = float.PositiveInfinity };
        if (ClipVertical)
            availableSize = availableSize with { Y = float.PositiveInfinity };

        return base.MeasureOverride(availableSize);
    }

    protected override Vector65 ArrangeOverride(Vector65 finalSize)
    {
        foreach (var child in Children)
        {
            child.Arrange(UIBox65.FromDimensions(Vector65.Zero, child.DesiredSize));
        }

        return finalSize;
    }
}