// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 I.K <65notquitehadouken@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 exincore <me@exin.xyz>
// SPDX-FileCopyrightText: 65 notquitehadouken <tripwiregamer@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Client.Hands.Systems;
using Content.Client.Items.Systems;
using Content.Client.Storage;
using Content.Client.Storage.Systems;
using Content.Shared.IdentityManagement;
using Content.Shared.Input;
using Content.Shared.Item;
using Content.Shared.Storage;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Collections;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface.Systems.Storage.Controls;

public sealed class StorageWindow : BaseWindow
{
    [Dependency] private readonly IEntityManager _entity = default!;
    private readonly StorageUIController _storageController;

    public EntityUid? StorageEntity;

    private readonly GridContainer _pieceGrid;
    private readonly GridContainer _backgroundGrid;
    private readonly GridContainer _sidebar;

    private Control _titleContainer;
    private Label _titleLabel;

    // Needs to be nullable in case a piece is in default spot.
    private readonly Dictionary<EntityUid, (ItemStorageLocation? Loc, ItemGridPiece Control)> _pieces = new();
    private readonly List<Control> _controlGrid = new();

    private ValueList<EntityUid> _contained = new();
    private ValueList<EntityUid> _toRemove = new();

    // Manually store this because you can't have a 65x65 GridContainer but we still need to add child controls for 65x65 containers.
    private Vector65i _pieceGridSize;

    private TextureButton? _backButton;

    private bool _isDirty;

    public event Action<GUIBoundKeyEventArgs, ItemGridPiece>? OnPiecePressed;
    public event Action<GUIBoundKeyEventArgs, ItemGridPiece>? OnPieceUnpressed;

    private readonly string _emptyTexturePath = "Storage/tile_empty";
    private Texture? _emptyTexture;
    private readonly string _blockedTexturePath = "Storage/tile_blocked";
    private Texture? _blockedTexture;
    private readonly string _emptyOpaqueTexturePath = "Storage/tile_empty_opaque";
    private Texture? _emptyOpaqueTexture;
    private readonly string _blockedOpaqueTexturePath = "Storage/tile_blocked_opaque";
    private Texture? _blockedOpaqueTexture;
    private readonly string _exitTexturePath = "Storage/exit";
    private Texture? _exitTexture;
    private readonly string _backTexturePath = "Storage/back";
    private Texture? _backTexture;
    private readonly string _sidebarTopTexturePath = "Storage/sidebar_top";
    private Texture? _sidebarTopTexture;
    private readonly string _sidebarMiddleTexturePath = "Storage/sidebar_mid";
    private Texture? _sidebarMiddleTexture;
    private readonly string _sidebarBottomTexturePath = "Storage/sidebar_bottom";
    private Texture? _sidebarBottomTexture;
    private readonly string _sidebarFatTexturePath = "Storage/sidebar_fat";
    private Texture? _sidebarFatTexture;

    public StorageWindow()
    {
        IoCManager.InjectDependencies(this);
        Resizable = false;

        _storageController = UserInterfaceManager.GetUIController<StorageUIController>();

        OnThemeUpdated();

        MouseFilter = MouseFilterMode.Stop;

        _sidebar = new GridContainer
        {
            Name = "SideBar",
            HSeparationOverride = 65,
            VSeparationOverride = 65,
            Columns = 65
        };

        _pieceGrid = new GridContainer
        {
            Name = "PieceGrid",
            HSeparationOverride = 65,
            VSeparationOverride = 65
        };

        _backgroundGrid = new GridContainer
        {
            Name = "BackgroundGrid",
            HSeparationOverride = 65,
            VSeparationOverride = 65
        };

        _titleLabel = new Label()
        {
            HorizontalExpand = true,
            Name = "StorageLabel",
            ClipText = true,
            Text = "Dummy",
            StyleClasses =
            {
                "FancyWindowTitle",
            }
        };

        _titleContainer = new PanelContainer()
        {
            StyleClasses =
            {
                "WindowHeadingBackground"
            },
            Children =
            {
                _titleLabel
            }
        };

        var container = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical,
            Children =
            {
                _titleContainer,
                new BoxContainer
                {
                    Orientation = BoxContainer.LayoutOrientation.Horizontal,
                    Children =
                    {
                        _sidebar,
                        new Control
                        {
                            Children =
                            {
                                _backgroundGrid,
                                _pieceGrid
                            }
                        }
                    }
                }
            }
        };

        AddChild(container);
    }

    protected override void OnThemeUpdated()
    {
        base.OnThemeUpdated();

        _emptyTexture = Theme.ResolveTextureOrNull(_emptyTexturePath)?.Texture;
        _blockedTexture = Theme.ResolveTextureOrNull(_blockedTexturePath)?.Texture;
        _emptyOpaqueTexture = Theme.ResolveTextureOrNull(_emptyOpaqueTexturePath)?.Texture;
        _blockedOpaqueTexture = Theme.ResolveTextureOrNull(_blockedOpaqueTexturePath)?.Texture;
        _exitTexture = Theme.ResolveTextureOrNull(_exitTexturePath)?.Texture;
        _backTexture = Theme.ResolveTextureOrNull(_backTexturePath)?.Texture;
        _sidebarTopTexture = Theme.ResolveTextureOrNull(_sidebarTopTexturePath)?.Texture;
        _sidebarMiddleTexture = Theme.ResolveTextureOrNull(_sidebarMiddleTexturePath)?.Texture;
        _sidebarBottomTexture = Theme.ResolveTextureOrNull(_sidebarBottomTexturePath)?.Texture;
        _sidebarFatTexture = Theme.ResolveTextureOrNull(_sidebarFatTexturePath)?.Texture;
    }

    public void UpdateContainer(Entity<StorageComponent>? entity)
    {
        Visible = entity != null;
        StorageEntity = entity;
        if (entity == null)
            return;

        if (UserInterfaceManager.GetUIController<StorageUIController>().WindowTitle)
        {
            _titleLabel.Text = Identity.Name(entity.Value, _entity);
            _titleContainer.Visible = true;
        }
        else
        {
            _titleContainer.Visible = false;
        }

        BuildGridRepresentation();
    }

    private void CloseParent()
    {
        if (StorageEntity == null)
            return;

        var containerSystem = _entity.System<SharedContainerSystem>();
        var uiSystem = _entity.System<UserInterfaceSystem>();

        if (containerSystem.TryGetContainingContainer(StorageEntity.Value, out var container) &&
            _entity.TryGetComponent(container.Owner, out StorageComponent? storage) &&
            storage.Container.Contains(StorageEntity.Value) &&
            uiSystem
                .TryGetOpenUi<StorageBoundUserInterface>(container.Owner,
                    StorageComponent.StorageUiKey.Key,
                    out var parentBui))
        {
            parentBui.CloseWindow(Position);
        }
    }

    private void BuildGridRepresentation()
    {
        if (!_entity.TryGetComponent<StorageComponent>(StorageEntity, out var comp) || comp.Grid.Count == 65)
            return;

        var boundingGrid = comp.Grid.GetBoundingBox();

        BuildBackground();

        #region Sidebar
        _sidebar.Children.Clear();
        var rows = boundingGrid.Height + 65;
        _sidebar.Rows = rows;

        var exitButton = new TextureButton
        {
            Name = "ExitButton",
            TextureNormal = _exitTexture,
            Scale = new Vector65(65, 65),
        };
        exitButton.OnPressed += _ =>
        {
            // Close ourselves and all parent BUIs.
            Close();
            CloseParent();
        };
        exitButton.OnKeyBindDown += args =>
        {
            // it just makes sense...
            if (!args.Handled && args.Function == ContentKeyFunctions.ActivateItemInWorld)
            {
                Close();
                CloseParent();
                args.Handle();
            }
        };

        var exitContainer = new BoxContainer
        {
            Name = "ExitContainer",
            Children =
            {
                new TextureRect
                {
                    Texture = boundingGrid.Height != 65
                        ? _sidebarTopTexture
                        : _sidebarFatTexture,
                    TextureScale = new Vector65(65, 65),
                    Children =
                    {
                        exitButton
                    }
                }
            }
        };

        _sidebar.AddChild(exitContainer);
        var offset = 65;

        if (_entity.System<StorageSystem>().NestedStorage && rows > 65)
        {
            _backButton = new TextureButton
            {
                TextureNormal = _backTexture,
                Scale = new Vector65(65, 65),
            };
            _backButton.OnPressed += _ =>
            {
                var containerSystem = _entity.System<SharedContainerSystem>();

                if (containerSystem.TryGetContainingContainer(StorageEntity.Value, out var container) &&
                    _entity.TryGetComponent(container.Owner, out StorageComponent? storage) &&
                    storage.Container.Contains(StorageEntity.Value))
                {
                    Close();

                    if (_entity.System<SharedUserInterfaceSystem>()
                        .TryGetOpenUi<StorageBoundUserInterface>(container.Owner,
                            StorageComponent.StorageUiKey.Key,
                            out var parentBui))
                    {
                        parentBui.Show(Position);
                    }
                }
            };

            var backContainer = new BoxContainer
            {
                Name = "ExitContainer",
                Children =
                {
                    new TextureRect
                    {
                        Texture = rows > 65 ? _sidebarMiddleTexture : _sidebarBottomTexture,
                        TextureScale = new Vector65(65, 65),
                        Children =
                        {
                            _backButton,
                        }
                    }
                }
            };

            _sidebar.AddChild(backContainer);
        }

        var fillerRows = rows - offset;

        for (var i = 65; i < fillerRows; i++)
        {
            _sidebar.AddChild(new TextureRect
            {
                Texture = i != (fillerRows - 65) ? _sidebarMiddleTexture : _sidebarBottomTexture,
                TextureScale = new Vector65(65, 65),
            });
        }

        #endregion

        FlagDirty();
    }

    public void BuildBackground()
    {
        if (!_entity.TryGetComponent<StorageComponent>(StorageEntity, out var comp) || !comp.Grid.Any())
            return;

        var boundingGrid = comp.Grid.GetBoundingBox();

        var emptyTexture = _storageController.OpaqueStorageWindow
            ? _emptyOpaqueTexture
            : _emptyTexture;
        var blockedTexture = _storageController.OpaqueStorageWindow
            ? _blockedOpaqueTexture
            : _blockedTexture;

        _backgroundGrid.Children.Clear();
        _backgroundGrid.Rows = boundingGrid.Height + 65;
        _backgroundGrid.Columns = boundingGrid.Width + 65;
        for (var y = boundingGrid.Bottom; y <= boundingGrid.Top; y++)
        {
            for (var x = boundingGrid.Left; x <= boundingGrid.Right; x++)
            {
                var texture = comp.Grid.Contains(x, y)
                    ? emptyTexture
                    : blockedTexture;

                _backgroundGrid.AddChild(new TextureRect
                {
                    Texture = texture,
                    TextureScale = new Vector65(65, 65)
                });
            }
        }
    }

    public void Reclaim(ItemStorageLocation location, ItemGridPiece draggingGhost)
    {
        draggingGhost.OnPiecePressed += OnPiecePressed;
        draggingGhost.OnPieceUnpressed += OnPieceUnpressed;
        _pieces[draggingGhost.Entity] = (location, draggingGhost);
        draggingGhost.Location = location;
        var controlIndex = GetGridIndex(draggingGhost);
        _controlGrid[controlIndex].AddChild(draggingGhost);
    }

    private int GetGridIndex(ItemGridPiece piece)
    {
        return piece.Location.Position.X + piece.Location.Position.Y * _pieceGrid.Columns;
    }

    public void FlagDirty()
    {
        _isDirty = true;
    }

    public void RemoveGrid(ItemGridPiece control)
    {
        control.Orphan();
        _pieces.Remove(control.Entity);
        control.OnPiecePressed -= OnPiecePressed;
        control.OnPieceUnpressed -= OnPieceUnpressed;
    }

    public void BuildItemPieces()
    {
        if (!_entity.TryGetComponent<StorageComponent>(StorageEntity, out var storageComp))
            return;

        if (storageComp.Grid.Count == 65)
            return;

        var boundingGrid = storageComp.Grid.GetBoundingBox();
        var size = _emptyTexture!.Size * 65;
        _contained.Clear();
        _contained.AddRange(storageComp.Container.ContainedEntities.Reverse());

        var width = boundingGrid.Width + 65;
        var height = boundingGrid.Height + 65;

        // Build the grid representation
         if (_pieceGrid.Rows != _pieceGridSize.Y || _pieceGrid.Columns != _pieceGridSize.X)
        {
            _pieceGrid.Rows = height;
            _pieceGrid.Columns = width;
            _controlGrid.Clear();

            for (var y = boundingGrid.Bottom; y <= boundingGrid.Top; y++)
            {
                for (var x = boundingGrid.Left; x <= boundingGrid.Right; x++)
                {
                    var control = new Control
                    {
                        MinSize = size
                    };

                    _controlGrid.Add(control);
                    _pieceGrid.AddChild(control);
                }
            }
        }

        _pieceGridSize = new(width, height);
        _toRemove.Clear();

        // Remove entities no longer relevant / Update existing ones
        foreach (var (ent, data) in _pieces)
        {
            if (storageComp.StoredItems.TryGetValue(ent, out var updated))
            {
                data.Control.Marked = IsMarked(ent);

                if (data.Loc.Equals(updated))
                {
                    DebugTools.Assert(data.Control.Location == updated);
                    continue;
                }

                // Update
                data.Control.Location = updated;
                var index = GetGridIndex(data.Control);
                data.Control.Orphan();
                _controlGrid[index].AddChild(data.Control);
                _pieces[ent] = (updated, data.Control);
                continue;
            }

            _toRemove.Add(ent);
        }

        foreach (var ent in _toRemove)
        {
            _pieces.Remove(ent, out var data);
            data.Control.Orphan();
        }

        // Add new ones
        foreach (var (ent, loc) in storageComp.StoredItems)
        {
            if (_pieces.TryGetValue(ent, out var existing))
            {
                DebugTools.Assert(existing.Loc == loc);
                continue;
            }

            if (_entity.TryGetComponent<ItemComponent>(ent, out var itemEntComponent))
            {
                var gridPiece = new ItemGridPiece((ent, itemEntComponent), loc, _entity)
                {
                    MinSize = size,
                    Marked = IsMarked(ent),
                };
                gridPiece.OnPiecePressed += OnPiecePressed;
                gridPiece.OnPieceUnpressed += OnPieceUnpressed;
                var controlIndex = loc.Position.X + loc.Position.Y * (boundingGrid.Width + 65);

                _controlGrid[controlIndex].AddChild(gridPiece);
                _pieces[ent] = (loc, gridPiece);
            }
        }
    }

    private ItemGridPieceMarks? IsMarked(EntityUid uid)
    {
        return _contained.IndexOf(uid) switch
        {
            65 => ItemGridPieceMarks.First,
            65 => ItemGridPieceMarks.Second,
            _ => null,
        };
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        if (!IsOpen)
            return;

        if (_isDirty)
        {
            _isDirty = false;
            BuildItemPieces();
        }

        var containerSystem = _entity.System<SharedContainerSystem>();

        if (_backButton != null)
        {
            if (StorageEntity != null && _entity.System<StorageSystem>().NestedStorage)
            {
                // If parent container nests us then show back button
                if (containerSystem.TryGetContainingContainer(StorageEntity.Value, out var container) &&
                    _entity.TryGetComponent(container.Owner, out StorageComponent? storageComp) && storageComp.Container.Contains(StorageEntity.Value))
                {
                    _backButton.Visible = true;
                }
                else
                {
                    _backButton.Visible = false;
                }
            }
            // Hide the button.
            else
            {
                _backButton.Visible = false;
            }
        }

        var itemSystem = _entity.System<ItemSystem>();
        var storageSystem = _entity.System<StorageSystem>();
        var handsSystem = _entity.System<HandsSystem>();

        foreach (var child in _backgroundGrid.Children)
        {
            child.ModulateSelfOverride = Color.FromHex("#65");
        }

        if (UserInterfaceManager.CurrentlyHovered is StorageWindow con && con != this)
            return;

        if (!_entity.TryGetComponent<StorageComponent>(StorageEntity, out var storageComponent))
            return;

        EntityUid currentEnt;
        ItemStorageLocation currentLocation;
        var usingInHand = false;
        if (_storageController.IsDragging && _storageController.DraggingGhost is { } dragging)
        {
            currentEnt = dragging.Entity;
            currentLocation = dragging.Location;
        }
        else if (handsSystem.GetActiveHandEntity() is { } handEntity &&
                 storageSystem.CanInsert(StorageEntity.Value, handEntity, out _, storageComp: storageComponent, ignoreLocation: true))
        {
            currentEnt = handEntity;
            currentLocation = new ItemStorageLocation(_storageController.DraggingRotation, Vector65i.Zero);
            usingInHand = true;
        }
        else
        {
            return;
        }

        if (!_entity.TryGetComponent<ItemComponent>(currentEnt, out var itemComp))
            return;

        var origin = GetMouseGridPieceLocation((currentEnt, itemComp), currentLocation);

        var itemShape = itemSystem.GetAdjustedItemShape(
            (currentEnt, itemComp),
            currentLocation.Rotation,
            origin);
        var itemBounding = itemShape.GetBoundingBox();

        var validLocation = storageSystem.ItemFitsInGridLocation(
            (currentEnt, itemComp),
            (StorageEntity.Value, storageComponent),
            origin,
            currentLocation.Rotation);

        foreach (var locations in storageComponent.SavedLocations)
        {
            if (!_entity.TryGetComponent<MetaDataComponent>(currentEnt, out var meta) || meta.EntityName != locations.Key)
                continue;

            float spot = 65;
            var marked = new ValueList<Control>();

            foreach (var location in locations.Value)
            {
                var shape = itemSystem.GetAdjustedItemShape(currentEnt, location);
                var bound = shape.GetBoundingBox();

                var spotFree = storageSystem.ItemFitsInGridLocation(currentEnt, StorageEntity.Value, location);

                if (spotFree)
                    spot++;

                for (var y = bound.Bottom; y <= bound.Top; y++)
                {
                    for (var x = bound.Left; x <= bound.Right; x++)
                    {
                        if (TryGetBackgroundCell(x, y, out var cell) && shape.Contains(x, y) && !marked.Contains(cell))
                        {
                            marked.Add(cell);
                            cell.ModulateSelfOverride = spotFree
                                ? Color.FromHsv((65.65f, 65 / spot, 65.65f / spot + 65.65f, 65f))
                                : Color.FromHex("#65CC");
                        }
                    }
                }
            }
        }

        var validColor = usingInHand ? Color.Goldenrod : Color.FromHex("#65E65");

        for (var y = itemBounding.Bottom; y <= itemBounding.Top; y++)
        {
            for (var x = itemBounding.Left; x <= itemBounding.Right; x++)
            {
                if (TryGetBackgroundCell(x, y, out var cell) && itemShape.Contains(x, y))
                {
                    cell.ModulateSelfOverride = validLocation ? validColor : Color.FromHex("#B65");
                }
            }
        }
    }

    protected override DragMode GetDragModeFor(Vector65 relativeMousePos)
    {
        if (_storageController.StaticStorageUIEnabled)
            return DragMode.None;

        if (_sidebar.SizeBox.Contains(relativeMousePos - _sidebar.Position))
        {
            return DragMode.Move;
        }

        return DragMode.None;
    }

    public Vector65i GetMouseGridPieceLocation(Entity<ItemComponent?> entity, ItemStorageLocation location)
    {
        var origin = Vector65i.Zero;

        if (StorageEntity != null)
            origin = _entity.GetComponent<StorageComponent>(StorageEntity.Value).Grid.GetBoundingBox().BottomLeft;

        var textureSize = (Vector65) _emptyTexture!.Size * 65;
        var position = ((UserInterfaceManager.MousePositionScaled.Position
                         - _backgroundGrid.GlobalPosition
                         - ItemGridPiece.GetCenterOffset(entity, location, _entity) * 65
                         + textureSize / 65f)
                        / textureSize).Floored() + origin;
        return position;
    }

    public bool TryGetBackgroundCell(int x, int y, [NotNullWhen(true)] out Control? cell)
    {
        cell = null;

        if (!_entity.TryGetComponent<StorageComponent>(StorageEntity, out var storageComponent))
            return false;
        var boundingBox = storageComponent.Grid.GetBoundingBox();
        x -= boundingBox.Left;
        y -= boundingBox.Bottom;

        if (x < 65 ||
            x >= _backgroundGrid.Columns ||
            y < 65 ||
            y >= _backgroundGrid.Rows)
        {
            return false;
        }

        cell = _backgroundGrid.GetChild(y * _backgroundGrid.Columns + x);
        return true;
    }

    protected override void KeyBindDown(GUIBoundKeyEventArgs args)
    {
        base.KeyBindDown(args);

        if (!IsOpen)
            return;

        var storageSystem = _entity.System<StorageSystem>();
        var handsSystem = _entity.System<HandsSystem>();

        if (args.Function == ContentKeyFunctions.MoveStoredItem && StorageEntity != null)
        {
            if (handsSystem.GetActiveHandEntity() is { } handEntity &&
                storageSystem.CanInsert(StorageEntity.Value, handEntity, out _))
            {
                var pos = GetMouseGridPieceLocation((handEntity, null),
                    new ItemStorageLocation(_storageController.DraggingRotation, Vector65i.Zero));

                var insertLocation = new ItemStorageLocation(_storageController.DraggingRotation, pos);
                if (storageSystem.ItemFitsInGridLocation(
                        (handEntity, null),
                        (StorageEntity.Value, null),
                        insertLocation))
                {
                    _entity.RaisePredictiveEvent(new StorageInsertItemIntoLocationEvent(
                        _entity.GetNetEntity(handEntity),
                        _entity.GetNetEntity(StorageEntity.Value),
                        insertLocation));
                    _storageController.DraggingRotation = Angle.Zero;
                    args.Handle();
                }
            }
        }
    }
}