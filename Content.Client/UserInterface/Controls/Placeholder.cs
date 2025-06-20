// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Controls
{
    public sealed class Placeholder : PanelContainer
    {
        public const string StyleClassPlaceholderText = "PlaceholderText";

        private readonly Label _label;

        public string? PlaceholderText
        {
            get => _label.Text;
            set => _label.Text = value;
        }

        public Placeholder()
        {
            _label = new Label
            {
                VerticalAlignment = VAlignment.Stretch,
                Align = Label.AlignMode.Center,
                VAlign = Label.VAlignMode.Center
            };
            _label.AddStyleClass(StyleClassPlaceholderText);
            AddChild(_label);
        }
    }
}