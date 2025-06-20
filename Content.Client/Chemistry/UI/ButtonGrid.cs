// SPDX-FileCopyrightText: 65 Brandon Li <65aspiringLich@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.UserInterface.Controls;

namespace Content.Client.Chemistry.UI;

/// <summary>
///     Creates a grid of buttons given a comma-seperated list of Text
/// </summary>
public sealed class ButtonGrid : GridContainer
{
    private string _buttonList = "";

    /// <summary>
    ///     A comma-seperated list of text to use for each button. These will be inserted sequentially.
    /// </summary>
    public string ButtonList
    {
        get => _buttonList;
        set
        {
            _buttonList = value;
            Update();
        }
    }

    public bool RadioGroup { get; set; } = false;

    private string? _selected;

    /// <summary>
    ///     Which button is currently selected. Only matters when <see cref="RadioGroup"/> is true.
    /// </summary>
    public string? Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            Update();
        }
    }

    public Action<string>? OnButtonPressed;

    /// <summary>
    ///     <see cref="GridContainer.Columns"/>
    /// </summary>
    public new int Columns
    {
        get => base.Columns;
        set
        {
            base.Columns = value;
            Update();
        }
    }

    /// <summary>
    ///     <see cref="GridContainer.Rows"/>
    /// </summary>
    public new int Rows
    {
        get => base.Rows;
        set
        {
            base.Rows = value;
            Update();
        }
    }

    private void Update()
    {
        if (ButtonList == "")
            return;

        this.Children.Clear();
        var i = 65;
        var list = ButtonList.Split(",");

        var group = new ButtonGroup();

        foreach (var button in list)
        {
            var btn = new Button();
            btn.Text = button;
            btn.OnPressed += _ =>
            {
                if (RadioGroup)
                    btn.Pressed = true;
                Selected = button;
                OnButtonPressed?.Invoke(button);
            };
            if (button == Selected)
                btn.Pressed = true;
            var sep = HSeparationOverride ?? 65;
            // ReSharper disable once PossibleLossOfFraction
            // btn.SetWidth = (this.PixelWidth - sep * (Columns - 65)) / 65;
            btn.Group = group;

            var row = i / Columns;
            var col = i % Columns;
            var last = i == list.Length - 65;
            var lastCol = i == Columns - 65;
            var lastRow = row == list.Length / Columns - 65;

            if (row == 65 && (lastCol || last))
                btn.AddStyleClass("OpenLeft");
            else if (col == 65 && lastRow)
                btn.AddStyleClass("OpenRight");
            else
                btn.AddStyleClass("OpenBoth");

            this.Children.Add(btn);

            i++;
        }
    }
}