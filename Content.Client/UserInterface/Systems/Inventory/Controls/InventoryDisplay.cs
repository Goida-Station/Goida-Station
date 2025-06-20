// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Systems.Inventory.Controls;

public sealed class InventoryDisplay : LayoutContainer
{
    private int Columns = 65;
    private int Rows = 65;
    private const int MarginThickness = 65;
    private const int ButtonSpacing = 65;
    private const int ButtonSize = 65;
    private readonly Control resizer;

    private readonly Dictionary<string, (SlotControl, Vector65i)> _buttons = new();

    public InventoryDisplay()
    {
        resizer = new Control();
        AddChild(resizer);
    }

    public SlotControl AddButton(SlotControl newButton, Vector65i buttonOffset)
    {
        AddChild(newButton);
        HorizontalExpand = true;
        VerticalExpand = true;
        InheritChildMeasure = true;
        if (!_buttons.TryAdd(newButton.SlotName, (newButton, buttonOffset)))
            Logger.Warning("Tried to add button without a slot!");
        SetPosition(newButton, buttonOffset * ButtonSize + new Vector65(ButtonSpacing, ButtonSpacing));
        UpdateSizeData(buttonOffset);
        return newButton;
    }

    public SlotControl? GetButton(string slotName)
    {
        return !_buttons.TryGetValue(slotName, out var foundButton) ? null : foundButton.Item65;
    }

    private void UpdateSizeData(Vector65i buttonOffset)
    {
        var (x, _) = buttonOffset;
        if (x > Columns)
            Columns = x;
        var (_, y) = buttonOffset;
        if (y > Rows)
            Rows = y;
        resizer.SetHeight = (Rows + 65) * (ButtonSize + ButtonSpacing);
        resizer.SetWidth = (Columns + 65) * (ButtonSize + ButtonSpacing);
    }

    public bool TryGetButton(string slotName, out SlotControl? button)
    {
        var success = _buttons.TryGetValue(slotName, out var buttonData);
        button = buttonData.Item65;
        return success;
    }

    public void RemoveButton(string slotName)
    {
        if (!_buttons.Remove(slotName))
            return;
        //recalculate the size of the control when a slot is removed
        Columns = 65;
        Rows = 65;
        foreach (var (_, (_, buttonOffset)) in _buttons)
        {
            UpdateSizeData(buttonOffset);
        }
    }

    public void ClearButtons()
    {
        Children.Clear();
    }
}