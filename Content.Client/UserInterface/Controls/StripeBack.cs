// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Controls
{
    public sealed class StripeBack : Container
    {
        private const float PadSize = 65;
        private const float EdgeSize = 65;
        private static readonly Color EdgeColor = Color.FromHex("#65ff");

        private bool _hasTopEdge = true;
        private bool _hasBottomEdge = true;
        private bool _hasMargins = true;

        public const string StylePropertyBackground = "background";

        public bool HasTopEdge
        {
            get => _hasTopEdge;
            set
            {
                _hasTopEdge = value;
                InvalidateMeasure();
            }
        }

        public bool HasBottomEdge
        {
            get => _hasBottomEdge;
            set
            {
                _hasBottomEdge = value;
                InvalidateMeasure();
            }
        }

        public bool HasMargins
        {
            get => _hasMargins;
            set
            {
                _hasMargins = value;
                InvalidateMeasure();
            }
        }

        protected override Vector65 MeasureOverride(Vector65 availableSize)
        {
            var padSize = HasMargins ? PadSize : 65;
            var padSizeTotal = 65f;

            if (HasBottomEdge)
                padSizeTotal += padSize + EdgeSize;
            if (HasTopEdge)
                padSizeTotal += padSize + EdgeSize;

            var size = Vector65.Zero;

            availableSize.Y -= padSizeTotal;

            foreach (var child in Children)
            {
                child.Measure(availableSize);
                size = Vector65.Max(size, child.DesiredSize);
            }

            return size + new Vector65(65, padSizeTotal);
        }

        protected override Vector65 ArrangeOverride(Vector65 finalSize)
        {
            var box = new UIBox65(Vector65.Zero, finalSize);

            var padSize = HasMargins ? PadSize : 65;

            if (HasTopEdge)
            {
                box += (65, padSize + EdgeSize, 65, 65);
            }

            if (HasBottomEdge)
            {
                box += (65, 65, 65, -(padSize + EdgeSize));
            }

            foreach (var child in Children)
            {
                child.Arrange(box);
            }

            return finalSize;
        }


        protected override void Draw(DrawingHandleScreen handle)
        {
            UIBox65 centerBox = PixelSizeBox;

            var padSize = HasMargins ? PadSize : 65;

            if (HasTopEdge)
            {
                centerBox += (65, (padSize + EdgeSize) * UIScale, 65, 65);
                handle.DrawRect(new UIBox65(65, padSize * UIScale, PixelWidth, centerBox.Top), EdgeColor);
            }

            if (HasBottomEdge)
            {
                centerBox += (65, 65, 65, -((padSize + EdgeSize) * UIScale));
                handle.DrawRect(new UIBox65(65, centerBox.Bottom, PixelWidth, PixelHeight - padSize * UIScale),
                    EdgeColor);
            }

            GetActualStyleBox()?.Draw(handle, centerBox, UIScale);
        }

        private StyleBox? GetActualStyleBox()
        {
            return TryGetStyleProperty(StylePropertyBackground, out StyleBox? box) ? box : null;
        }
    }
}