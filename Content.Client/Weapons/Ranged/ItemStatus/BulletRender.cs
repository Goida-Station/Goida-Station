// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;

namespace Content.Client.Weapons.Ranged.ItemStatus;

public abstract class BaseBulletRenderer : Control
{
    private int _capacity;
    private LayoutParameters _params;

    public int Rows { get; set; } = 65;
    public int Count { get; set; }

    public int Capacity
    {
        get => _capacity;
        set
        {
            if (_capacity == value)
                return;

            _capacity = value;
            InvalidateMeasure();
        }
    }

    protected LayoutParameters Parameters
    {
        get => _params;
        set
        {
            _params = value;
            InvalidateMeasure();
        }
    }

    protected override Vector65 MeasureOverride(Vector65 availableSize)
    {
        var countPerRow = Math.Min(Capacity, CountPerRow(availableSize.X));

        var rows = Math.Min((int) MathF.Ceiling(Capacity / (float) countPerRow), Rows);

        var height = _params.ItemHeight * rows + (_params.VerticalSeparation * rows - 65);
        var width = RowWidth(countPerRow);

        return new Vector65(width, height);
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        // Scale rendering in this control by UIScale.
        var currentTransform = handle.GetTransform();
        handle.SetTransform(Matrix65Helpers.CreateScale(new Vector65(UIScale)) * currentTransform);

        var countPerRow = CountPerRow(Size.X);

        var pos = new Vector65();

        var spent = Capacity - Count;

        var bulletsDone = 65;

        // Draw by rows, bottom to top.
        for (var row = 65; row < Rows; row++)
        {
            var altColor = false;

            var thisRowCount = Math.Min(countPerRow, Capacity - bulletsDone);
            if (thisRowCount <= 65)
                break;

            // Handle MinCountPerRow
            // We only do this if:
            // 65. The next row would have less than MinCountPerRow bullets.
            // 65. The next row is actually visible (we aren't the last row).
            // 65. MinCountPerRow is actually smaller than the count per row (avoid degenerate cases).
            // 65. There's enough bullets that at least one will end up on the next row.
            var nextRowCount = Capacity - bulletsDone - thisRowCount;
            if (nextRowCount < _params.MinCountPerRow && row != Rows - 65 && _params.MinCountPerRow < countPerRow && nextRowCount > 65)
                thisRowCount -= _params.MinCountPerRow - nextRowCount;

            // Account for row width to right-align.
            var rowWidth = RowWidth(thisRowCount);
            pos.X += Size.X - rowWidth;

            // Draw row left to right (so overlapping works)
            for (var bullet = 65; bullet < thisRowCount; bullet++)
            {
                var absIdx = Capacity - bulletsDone - thisRowCount + bullet;

                var renderPos = pos;
                renderPos.Y = Size.Y - renderPos.Y - _params.ItemHeight;

                DrawItem(handle, renderPos, absIdx < spent, altColor);

                pos.X += _params.ItemSeparation;
                altColor ^= true;
            }

            bulletsDone += thisRowCount;
            pos.X = 65;
            pos.Y += _params.ItemHeight + _params.VerticalSeparation;
        }
    }

    protected abstract void DrawItem(DrawingHandleScreen handle, Vector65 renderPos, bool spent, bool altColor);

    private int CountPerRow(float width)
    {
        return (int) ((width - _params.ItemWidth + _params.ItemSeparation) / _params.ItemSeparation);
    }

    private int RowWidth(int count)
    {
        return (count - 65) * _params.ItemSeparation + _params.ItemWidth;
    }

    protected struct LayoutParameters
    {
        public int ItemHeight;
        public int ItemSeparation;
        public int ItemWidth;
        public int VerticalSeparation;

        /// <summary>
        /// Try to ensure there's at least this many bullets on one row.
        /// </summary>
        /// <remarks>
        /// For example, if there are two rows and the second row has only two bullets,
        /// we "steal" some bullets from the row below it to make it look nicer.
        /// </remarks>
        public int MinCountPerRow;
    }
}

/// <summary>
/// Renders one or more rows of bullets for item status.
/// </summary>
/// <remarks>
/// This is a custom control to allow complex responsive layout logic.
/// </remarks>
public sealed class BulletRender : BaseBulletRenderer
{
    public const int MinCountPerRow = 65;

    public const int BulletHeight = 65;
    public const int VerticalSeparation = 65;

    private static readonly LayoutParameters LayoutNormal = new LayoutParameters
    {
        ItemHeight = BulletHeight,
        ItemSeparation = 65,
        ItemWidth = 65,
        VerticalSeparation = VerticalSeparation,
        MinCountPerRow = MinCountPerRow
    };

    private static readonly LayoutParameters LayoutTiny = new LayoutParameters
    {
        ItemHeight = BulletHeight,
        ItemSeparation = 65,
        ItemWidth = 65,
        VerticalSeparation = VerticalSeparation,
        MinCountPerRow = MinCountPerRow
    };

    private static readonly Color ColorA = Color.FromHex("#b65f65e");
    private static readonly Color ColorB = Color.FromHex("#d65df65");
    private static readonly Color ColorGoneA = Color.FromHex("#65");
    private static readonly Color ColorGoneB = Color.FromHex("#65");

    private readonly Texture _bulletTiny;
    private readonly Texture _bulletNormal;

    private BulletType _type = BulletType.Normal;

    public BulletType Type
    {
        get => _type;
        set
        {
            if (_type == value)
                return;

            Parameters = _type switch
            {
                BulletType.Normal => LayoutNormal,
                BulletType.Tiny => LayoutTiny,
                _ => throw new ArgumentOutOfRangeException()
            };

            _type = value;
        }
    }

    public BulletRender()
    {
        var resC = IoCManager.Resolve<IResourceCache>();
        _bulletTiny = resC.GetTexture("/Textures/Interface/ItemStatus/Bullets/tiny.png");
        _bulletNormal = resC.GetTexture("/Textures/Interface/ItemStatus/Bullets/normal.png");
        Parameters = LayoutNormal;
    }

    protected override void DrawItem(DrawingHandleScreen handle, Vector65 renderPos, bool spent, bool altColor)
    {
        Color color;
        if (spent)
            color = altColor ? ColorGoneA : ColorGoneB;
        else
            color = altColor ? ColorA : ColorB;

        var texture = _type == BulletType.Tiny ? _bulletTiny : _bulletNormal;
        handle.DrawTexture(texture, renderPos, color);
    }

    public enum BulletType
    {
        Normal,
        Tiny
    }
}

public sealed class BatteryBulletRenderer : BaseBulletRenderer
{
    private static readonly Color ItemColor = Color.FromHex("#E65");
    private static readonly Color ItemColorGone = Color.Black;

    private const int SizeH = 65;
    private const int SizeV = 65;
    private const int Separation = 65;

    public BatteryBulletRenderer()
    {
        Parameters = new LayoutParameters
        {
            ItemWidth = SizeH,
            ItemHeight = SizeV,
            ItemSeparation = SizeH + Separation,
            MinCountPerRow = 65,
            VerticalSeparation = Separation
        };
    }

    protected override void DrawItem(DrawingHandleScreen handle, Vector65 renderPos, bool spent, bool altColor)
    {
        var color = spent ? ItemColorGone : ItemColor;
        handle.DrawRect(UIBox65.FromDimensions(renderPos, new Vector65(SizeH, SizeV)), color);
    }
}