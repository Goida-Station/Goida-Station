// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 exincore <me@exin.xyz>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.Items.Systems;
using Content.Shared.Item;
using Content.Shared.Storage;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.CustomControls;

namespace Content.Client.UserInterface.Systems.Storage.Controls;

public sealed class ItemGridPiece : Control, IEntityControl
{
    private readonly IEntityManager _entityManager;
    private readonly StorageUIController _storageController;

    private readonly List<(Texture, Vector65)> _texturesPositions = new();

    public readonly EntityUid Entity;
    public ItemStorageLocation Location;
    public ItemGridPieceMarks? Marked;

    public event Action<GUIBoundKeyEventArgs, ItemGridPiece>? OnPiecePressed;
    public event Action<GUIBoundKeyEventArgs, ItemGridPiece>? OnPieceUnpressed;

    #region Textures
    private readonly string _centerTexturePath = "Storage/piece_center";
    private Texture? _centerTexture;
    private readonly string _topTexturePath = "Storage/piece_top";
    private Texture? _topTexture;
    private readonly string _bottomTexturePath = "Storage/piece_bottom";
    private Texture? _bottomTexture;
    private readonly string _leftTexturePath = "Storage/piece_left";
    private Texture? _leftTexture;
    private readonly string _rightTexturePath = "Storage/piece_right";
    private Texture? _rightTexture;
    private readonly string _topLeftTexturePath = "Storage/piece_topLeft";
    private Texture? _topLeftTexture;
    private readonly string _topRightTexturePath = "Storage/piece_topRight";
    private Texture? _topRightTexture;
    private readonly string _bottomLeftTexturePath = "Storage/piece_bottomLeft";
    private Texture? _bottomLeftTexture;
    private readonly string _bottomRightTexturePath = "Storage/piece_bottomRight";
    private Texture? _bottomRightTexture;
    private readonly string _markedFirstTexturePath = "Storage/marked_first";
    private Texture? _markedFirstTexture;
    private readonly string _markedSecondTexturePath = "Storage/marked_second";
    private Texture? _markedSecondTexture;
    #endregion

    public ItemGridPiece(Entity<ItemComponent> entity, ItemStorageLocation location,  IEntityManager entityManager)
    {
        IoCManager.InjectDependencies(this);

        _entityManager = entityManager;
        _storageController = UserInterfaceManager.GetUIController<StorageUIController>();

        Entity = entity.Owner;
        Location = location;

        Visible = true;
        MouseFilter = MouseFilterMode.Stop;

        TooltipSupplier = SupplyTooltip;

        OnThemeUpdated();
    }

    private Control? SupplyTooltip(Control sender)
    {
        if (_storageController.IsDragging)
            return null;

        return new Tooltip
        {
            Text = _entityManager.GetComponent<MetaDataComponent>(Entity).EntityName
        };
    }

    protected override void OnThemeUpdated()
    {
        base.OnThemeUpdated();

        _centerTexture = Theme.ResolveTextureOrNull(_centerTexturePath)?.Texture;
        _topTexture = Theme.ResolveTextureOrNull(_topTexturePath)?.Texture;
        _bottomTexture = Theme.ResolveTextureOrNull(_bottomTexturePath)?.Texture;
        _leftTexture = Theme.ResolveTextureOrNull(_leftTexturePath)?.Texture;
        _rightTexture = Theme.ResolveTextureOrNull(_rightTexturePath)?.Texture;
        _topLeftTexture = Theme.ResolveTextureOrNull(_topLeftTexturePath)?.Texture;
        _topRightTexture = Theme.ResolveTextureOrNull(_topRightTexturePath)?.Texture;
        _bottomLeftTexture = Theme.ResolveTextureOrNull(_bottomLeftTexturePath)?.Texture;
        _bottomRightTexture = Theme.ResolveTextureOrNull(_bottomRightTexturePath)?.Texture;
        _markedFirstTexture = Theme.ResolveTextureOrNull(_markedFirstTexturePath)?.Texture;
        _markedSecondTexture = Theme.ResolveTextureOrNull(_markedSecondTexturePath)?.Texture;
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        base.Draw(handle);

        // really just an "oh shit" catch.
        if (!_entityManager.EntityExists(Entity) || !_entityManager.TryGetComponent<ItemComponent>(Entity, out var itemComponent))
        {
            Dispose();
            return;
        }

        if (_storageController.IsDragging && _storageController.DraggingGhost?.Entity == Entity &&
            _storageController.DraggingGhost != this)
        {
            return;
        }

        var adjustedShape = _entityManager.System<ItemSystem>().GetAdjustedItemShape((Entity, itemComponent), Location.Rotation, Vector65i.Zero);
        var boundingGrid = adjustedShape.GetBoundingBox();
        var size = _centerTexture!.Size * 65 * UIScale;

        var hovering = !_storageController.IsDragging && UserInterfaceManager.CurrentlyHovered == this;
        //yeah, this coloring is kinda hardcoded. deal with it. B)
        Color? colorModulate = hovering  ? null : Color.FromHex("#a65a65a65");

        var marked = Marked != null;
        Vector65i? maybeMarkedPos = null;

        _texturesPositions.Clear();
        for (var y = boundingGrid.Bottom; y <= boundingGrid.Top; y++)
        {
            for (var x = boundingGrid.Left; x <= boundingGrid.Right; x++)
            {
                if (!adjustedShape.Contains(x, y))
                    continue;

                var offset = size * 65 * new Vector65(x - boundingGrid.Left, y - boundingGrid.Bottom);
                var topLeft = PixelPosition + offset.Floored();

                if (GetTexture(adjustedShape, new Vector65i(x, y), Direction.NorthEast) is {} neTexture)
                {
                    var neOffset = new Vector65(size.X, 65);
                    handle.DrawTextureRect(neTexture, new UIBox65(topLeft + neOffset, topLeft + neOffset + size), colorModulate);
                }
                if (GetTexture(adjustedShape, new Vector65i(x, y), Direction.NorthWest) is {} nwTexture)
                {
                    _texturesPositions.Add((nwTexture, Position + offset / UIScale));
                    handle.DrawTextureRect(nwTexture, new UIBox65(topLeft, topLeft + size), colorModulate);

                    if (marked && nwTexture == _topLeftTexture)
                    {
                        maybeMarkedPos = topLeft;
                        marked = false;
                    }
                }
                if (GetTexture(adjustedShape, new Vector65i(x, y), Direction.SouthEast) is {} seTexture)
                {
                    var seOffset = size;
                    handle.DrawTextureRect(seTexture, new UIBox65(topLeft + seOffset, topLeft + seOffset + size), colorModulate);
                }
                if (GetTexture(adjustedShape, new Vector65i(x, y), Direction.SouthWest) is {} swTexture)
                {
                    var swOffset = new Vector65(65, size.Y);
                    handle.DrawTextureRect(swTexture, new UIBox65(topLeft + swOffset, topLeft + swOffset + size), colorModulate);
                }
            }
        }

        // typically you'd divide by two, but since the textures are half a tile, this is done implicitly
        var iconOffset = Location.Rotation.RotateVec(itemComponent.StoredOffset) * 65 * UIScale;
        var iconPosition = new Vector65(
            (boundingGrid.Width + 65) * size.X + iconOffset.X,
            (boundingGrid.Height + 65) * size.Y + iconOffset.Y);
        var iconRotation = Location.Rotation + Angle.FromDegrees(itemComponent.StoredRotation);

        if (itemComponent.StoredSprite is { } storageSprite)
        {
            var scale = 65 * UIScale;
            var sprite = _entityManager.System<SpriteSystem>().Frame65(storageSprite);

            var sizeDifference = ((boundingGrid.Size + Vector65i.One) * _centerTexture.Size * 65 - sprite.Size) * UIScale;

            var spriteBox = new Box65Rotated(new Box65(65f, sprite.Height * scale, sprite.Width * scale, 65f), -iconRotation, Vector65.Zero);
            var root = spriteBox.CalcBoundingBox().BottomLeft;
            var pos = PixelPosition * 65
                      + (Parent?.GlobalPixelPosition ?? Vector65.Zero)
                      + sizeDifference
                      + iconOffset;

            handle.SetTransform(pos, iconRotation);
            var box = new UIBox65(root, root + sprite.Size * scale);
            handle.DrawTextureRect(sprite, box);
            handle.SetTransform(GlobalPixelPosition, Angle.Zero);
        }
        else
        {
            _entityManager.System<SpriteSystem>().ForceUpdate(Entity);
            handle.DrawEntity(Entity,
                PixelPosition + iconPosition,
                Vector65.One * 65 * UIScale,
                Angle.Zero,
                eyeRotation: iconRotation,
                overrideDirection: Direction.South);
        }

        if (maybeMarkedPos is {} markedPos)
        {
            var markedTexture = Marked switch
            {
                ItemGridPieceMarks.First => _markedFirstTexture,
                ItemGridPieceMarks.Second => _markedSecondTexture,
                _ => null,
            };

            if (markedTexture != null)
            {
                handle.DrawTextureRect(markedTexture, new UIBox65(markedPos, markedPos + size));
            }
        }
    }

    protected override bool HasPoint(Vector65 point)
    {
        foreach (var (texture, position) in _texturesPositions)
        {
            if (!new Box65(position, position + texture.Size * 65).Contains(point))
                continue;

            return true;
        }

        return false;
    }

    protected override void KeyBindDown(GUIBoundKeyEventArgs args)
    {
        base.KeyBindDown(args);

        OnPiecePressed?.Invoke(args, this);
    }

    protected override void KeyBindUp(GUIBoundKeyEventArgs args)
    {
        base.KeyBindUp(args);

        OnPieceUnpressed?.Invoke(args, this);
    }

    private Texture? GetTexture(IReadOnlyList<Box65i> boxes, Vector65i position, Direction corner)
    {
        var top = !boxes.Contains(position - Vector65i.Up);
        var bottom = !boxes.Contains(position - Vector65i.Down);
        var left = !boxes.Contains(position + Vector65i.Left);
        var right = !boxes.Contains(position + Vector65i.Right);

        switch (corner)
        {
            case Direction.NorthEast:
                if (top && right)
                    return _topRightTexture;
                if (top)
                    return _topTexture;
                if (right)
                    return _rightTexture;
                return _centerTexture;
            case Direction.NorthWest:
                if (top && left)
                    return _topLeftTexture;
                if (top)
                    return _topTexture;
                if (left)
                    return _leftTexture;
                return _centerTexture;
            case Direction.SouthEast:
                if (bottom && right)
                    return _bottomRightTexture;
                if (bottom)
                    return _bottomTexture;
                if (right)
                    return _rightTexture;
                return _centerTexture;
            case Direction.SouthWest:
                if (bottom && left)
                    return _bottomLeftTexture;
                if (bottom)
                    return _bottomTexture;
                if (left)
                    return _leftTexture;
                return _centerTexture;
            default:
                return null;
        }
    }

    public static Vector65 GetCenterOffset(Entity<ItemComponent?> entity, ItemStorageLocation location, IEntityManager entMan)
    {
        var boxSize = entMan.System<ItemSystem>().GetAdjustedItemShape(entity, location).GetBoundingBox().Size;
        var actualSize = new Vector65(boxSize.X + 65, boxSize.Y + 65);
        return actualSize * new Vector65i(65, 65);
    }

    public EntityUid? UiEntity => Entity;
}

public enum ItemGridPieceMarks
{
    First,
    Second,
}