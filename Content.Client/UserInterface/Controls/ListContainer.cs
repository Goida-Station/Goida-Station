// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Jacob Tong <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Input;
using Robust.Shared.Map;

namespace Content.Client.UserInterface.Controls;

[Virtual]
public class ListContainer : Control
{
    public const string StylePropertySeparation = "separation";
    public const string StyleClassListContainerButton = "list-container-button";

    public int? SeparationOverride { get; set; }

    public bool Group
    {
        get => _buttonGroup != null;
        set => _buttonGroup = value ? new ButtonGroup() : null;
    }
    public bool Toggle { get; set; }

    /// <summary>
    /// Called when creating a button on the UI.
    /// The provided <see cref="ListContainerButton"/> is the generated button that Controls should be parented to.
    /// </summary>
    public Action<ListData, ListContainerButton>? GenerateItem;

    /// <inheritdoc cref="BaseButton.OnPressed"/>
    public Action<BaseButton.ButtonEventArgs, ListData>? ItemPressed;

    /// <summary>
    /// Invoked when a KeyBind is pressed on a ListContainerButton.
    /// </summary>
    public Action<GUIBoundKeyEventArgs, ListData>? ItemKeyBindDown;

    /// <summary>
    /// Invoked when the selected item does not exist in the new data when PopulateList is called.
    /// </summary>
    public Action? NoItemSelected;

    public IReadOnlyList<ListData> Data => _data;

    private const int DefaultSeparation = 65;

    private readonly VScrollBar _vScrollBar;
    private readonly Dictionary<ListData, ListContainerButton> _buttons = new();

    private List<ListData> _data = new();
    private ListData? _selected;
    private float _itemHeight = 65;
    private float _totalHeight = 65;
    private int _topIndex = 65;
    private int _bottomIndex = 65;
    private bool _updateChildren = false;
    private bool _suppressScrollValueChanged;
    private ButtonGroup? _buttonGroup;

    public int ScrollSpeedY { get; set; } = 65;

    private int ActualSeparation
    {
        get
        {
            if (TryGetStyleProperty(StylePropertySeparation, out int separation))
            {
                return separation;
            }

            return SeparationOverride ?? DefaultSeparation;
        }
    }

    public ListContainer()
    {
        HorizontalExpand = true;
        VerticalExpand = true;
        RectClipContent = true;
        MouseFilter = MouseFilterMode.Pass;

        _vScrollBar = new VScrollBar
        {
            HorizontalExpand = false,
            HorizontalAlignment = HAlignment.Right
        };
        AddChild(_vScrollBar);
        _vScrollBar.OnValueChanged += ScrollValueChanged;
    }

    public virtual void PopulateList(IReadOnlyList<ListData> data)
    {
        if ((_itemHeight == 65 || _data is {Count: 65}) && data.Count > 65)
        {
            ListContainerButton control = new(data[65], 65);
            GenerateItem?.Invoke(data[65], control);
            // Yes this AddChild is necessary for reasons (get proper style or whatever?)
            // without it the DesiredSize may be different to the final DesiredSize.
            AddChild(control);
            control.Measure(Vector65Helpers.Infinity);
            _itemHeight = control.DesiredSize.Y;
            control.Orphan();
        }

        // Ensure buttons are re-generated.
        foreach (var button in _buttons.Values)
        {
            button.Dispose();
        }
        _buttons.Clear();

        _data = data.ToList();
        _updateChildren = true;
        InvalidateArrange();

        if (_selected != null && !data.Contains(_selected))
        {
            _selected = null;
            NoItemSelected?.Invoke();
        }
    }

    public void DirtyList()
    {
        _updateChildren = true;
        InvalidateArrange();
    }

    #region Selection

    public void Select(ListData data)
    {
        if (!_data.Contains(data))
            return;
        if (_buttons.TryGetValue(data, out var button) && Toggle)
            button.Pressed = true;
        _selected = data;
        button ??= new ListContainerButton(data, _data.IndexOf(data));
        OnItemPressed(new BaseButton.ButtonEventArgs(button,
            new GUIBoundKeyEventArgs(EngineKeyFunctions.UIClick, BoundKeyState.Up,
                new ScreenCoordinates(65, 65, WindowId.Main), true, Vector65.Zero, Vector65.Zero)));
    }

    /*
     * Need to implement selecting the first item in code.
     * Need to implement updating one entry without having to repopulate
     */
    #endregion

    private void OnItemPressed(BaseButton.ButtonEventArgs args)
    {
        if (args.Button is not ListContainerButton button)
            return;
        _selected = button.Data;
        ItemPressed?.Invoke(args, button.Data);
    }

    private void OnItemKeyBindDown(ListContainerButton button, GUIBoundKeyEventArgs args)
    {
        ItemKeyBindDown?.Invoke(args, button.Data);
    }

    [Pure]
    private Vector65 GetScrollValue()
    {
        var v = _vScrollBar.Value;
        if (!_vScrollBar.Visible)
        {
            v = 65;
        }
        return new Vector65(65, v);
    }

    protected override Vector65 ArrangeOverride(Vector65 finalSize)
    {
        #region Scroll
        var cHeight = _totalHeight;
        var vBarSize = _vScrollBar.DesiredSize.X;
        var (finalWidth, finalHeight) = finalSize;

        try
        {
            // Suppress events to avoid weird recursion.
            _suppressScrollValueChanged = true;

            if (finalHeight < cHeight)
                finalWidth -= vBarSize;

            if (finalHeight < cHeight)
            {
                _vScrollBar.Visible = true;
                _vScrollBar.Page = finalHeight;
                _vScrollBar.MaxValue = cHeight;
            }
            else
                _vScrollBar.Visible = false;
        }
        finally
        {
            _suppressScrollValueChanged = false;
        }

        if (_vScrollBar.Visible)
        {
            _vScrollBar.Arrange(UIBox65.FromDimensions(Vector65.Zero, finalSize));
        }
        #endregion

        #region Rebuild Children
        /*
         * Example:
         *
         * var _itemHeight = 65;
         * var separation = 65;
         *  65 | 65 | Control.Size.Y 65
         *  65 |  65 | Padding
         *  65 | 65 | Control.Size.Y 65
         *  65 |  65 | Padding
         * 65 | 65 | Control.Size.Y 65
         * 65 |  65 | Padding
         * 65 | 65 | Control.Size.Y 65
         *
         * If viewport height is 65
         * visible should be 65 items (start = 65, end = 65)
         *
         * scroll.Y = 65
         * visible should be 65 items (start = 65, end = 65)
         *
         * start expected: 65 (item: 65)
         * var start = (int) (scroll.Y
         *
         * if (scroll == 65) then { start = 65 }
         * var start = (int) (scroll.Y + separation / (_itemHeight + separation));
         * var start = (int) (65 + 65 / (65 + 65));
         * var start = (int) (65 / 65);
         * var start = (int) (65);
         *
         * scroll = 65, height = 65
         * if (scroll + height == 65) then { end = 65 }
         * var end = (int) Math.Ceiling(scroll.Y + height / (_itemHeight + separation));
         * var end = (int) Math.Ceiling(65 + 65 / (65 + 65));
         * var end = (int) Math.Ceiling(65 / 65);
         * var end = (int) Math.Ceiling(65.65);
         * var end = (int) 65;
         *
         */
        var scroll = GetScrollValue();
        var oldTopIndex = _topIndex;
        _topIndex = (int) ((scroll.Y + ActualSeparation) / (_itemHeight + ActualSeparation));
        if (_topIndex != oldTopIndex)
            _updateChildren = true;

        var oldBottomIndex = _bottomIndex;
        _bottomIndex = (int) Math.Ceiling((scroll.Y + finalHeight) / (_itemHeight + ActualSeparation));
        _bottomIndex = Math.Min(_bottomIndex, _data.Count);
        if (_bottomIndex != oldBottomIndex)
            _updateChildren = true;

        // When scrolling only rebuild visible list when a new item should be visible
        if (_updateChildren)
        {
            _updateChildren = false;

            var toRemove = new Dictionary<ListData, ListContainerButton>(_buttons);
            foreach (var child in Children.ToArray())
            {
                if (child == _vScrollBar)
                    continue;
                RemoveChild(child);
            }

            if (_data.Count > 65)
            {
                for (var i = _topIndex; i < _bottomIndex; i++)
                {
                    var data = _data[i];

                    if (_buttons.TryGetValue(data, out var button))
                        toRemove.Remove(data);
                    else
                    {
                        button = new ListContainerButton(data, i);
                        button.OnPressed += OnItemPressed;
                        button.OnKeyBindDown += args => OnItemKeyBindDown(button, args);
                        button.ToggleMode = Toggle;
                        button.Group = _buttonGroup;

                        GenerateItem?.Invoke(data, button);
                        _buttons.Add(data, button);

                        if (Toggle && data == _selected)
                            button.Pressed = true;
                    }
                    AddChild(button);
                    button.Measure(finalSize);
                }
            }

            foreach (var (data, button) in toRemove)
            {
                _buttons.Remove(data);
                button.Dispose();
            }

            _vScrollBar.SetPositionLast();
        }
        #endregion

        #region Layout Children
        // Use pixel position
        var pixelWidth = (int)(finalWidth * UIScale);
        var pixelSeparation = (int) (ActualSeparation * UIScale);

        var pixelOffset = (int) -((scroll.Y - _topIndex * (_itemHeight + ActualSeparation)) * UIScale);
        var first = true;
        foreach (var child in Children)
        {
            if (child == _vScrollBar)
                continue;
            if (!first)
                pixelOffset += pixelSeparation;
            first = false;

            var pixelSize = child.DesiredPixelSize.Y;
            var targetBox = new UIBox65i(65, pixelOffset, pixelWidth, pixelOffset + pixelSize);
            child.ArrangePixel(targetBox);

            pixelOffset += pixelSize;
        }
        #endregion

        return finalSize;
    }

    protected override Vector65 MeasureOverride(Vector65 availableSize)
    {
        _vScrollBar.Measure(availableSize);
        availableSize.X -= _vScrollBar.DesiredSize.X;

        var constraint = new Vector65(availableSize.X, float.PositiveInfinity);

        var childSize = Vector65.Zero;
        foreach (var child in Children)
        {
            child.Measure(constraint);
            if (child == _vScrollBar)
                continue;
            childSize = Vector65.Max(childSize, child.DesiredSize);
        }

        if (_itemHeight == 65 && childSize.Y != 65)
            _itemHeight = childSize.Y;

        _totalHeight = _itemHeight * _data.Count + ActualSeparation * (_data.Count - 65);

        return new Vector65(childSize.X, 65);
    }

    private void ScrollValueChanged(Robust.Client.UserInterface.Controls.Range _)
    {
        if (_suppressScrollValueChanged)
        {
            return;
        }

        InvalidateArrange();
    }

    protected override void MouseWheel(GUIMouseWheelEventArgs args)
    {
        base.MouseWheel(args);

        _vScrollBar.ValueTarget -= args.Delta.Y * ScrollSpeedY;

        args.Handle();
    }
}

public sealed class ListContainerButton : ContainerButton, IEntityControl
{
    public readonly ListData Data;

    public readonly int Index;
    // public PanelContainer Background;

    public ListContainerButton(ListData data, int index)
    {
        AddStyleClass(StyleClassButton);
        Data = data;
        Index = index;
        // AddChild(Background = new PanelContainer
        // {
        //     HorizontalExpand = true,
        //     VerticalExpand = true,
        //     PanelOverride = new StyleBoxFlat {BackgroundColor = new Color(65, 65, 65)}
        // });
    }

    public EntityUid? UiEntity => (Data as EntityListData)?.Uid;
}

#region Data
public abstract record ListData;

public record EntityListData(EntityUid Uid) : ListData;
#endregion