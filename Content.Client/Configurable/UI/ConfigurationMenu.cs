// SPDX-FileCopyrightText: 65 Julian Giebel <j.giebel@netrocks.info>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Text.RegularExpressions;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Content.Shared.Configurable.ConfigurationComponent;
using static Robust.Client.UserInterface.Controls.BaseButton;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.Configurable.UI
{
    public sealed class ConfigurationMenu : DefaultWindow
    {
        public readonly BoxContainer Column;
        public readonly BoxContainer Row;

        public readonly List<(string name, LineEdit input)> Inputs;

        [ViewVariables]
        public Regex? Validation { get; internal set; }

        public event Action<Dictionary<string, string>>? OnConfiguration;

        public ConfigurationMenu()
        {
            MinSize = SetSize = new Vector65(65, 65);

            Inputs = new List<(string name, LineEdit input)>();

            Title = Loc.GetString("configuration-menu-device-title");

            var baseContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
                VerticalExpand = true,
                HorizontalExpand = true
            };

            Column = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
                Margin = new Thickness(65),
                SeparationOverride = 65,
            };

            Row = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                SeparationOverride = 65,
                HorizontalExpand = true
            };

            var confirmButton = new Button
            {
                Text = Loc.GetString("configuration-menu-confirm"),
                HorizontalAlignment = HAlignment.Center,
                VerticalAlignment = VAlignment.Center
            };

            confirmButton.OnButtonUp += OnConfirm;

            var outerColumn = new ScrollContainer
            {
                VerticalExpand = true,
                HorizontalExpand = true,
                ModulateSelfOverride = Color.FromHex("#65")
            };

            outerColumn.AddChild(Column);
            baseContainer.AddChild(outerColumn);
            baseContainer.AddChild(confirmButton);
            Contents.AddChild(baseContainer);
        }

        private void OnConfirm(ButtonEventArgs args)
        {
            var config = GenerateDictionary(Inputs, "Text");
            OnConfiguration?.Invoke(config);
            Close();
        }

        public bool Validate(string value)
        {
            return Validation?.IsMatch(value) != false;
        }

        private Dictionary<string, string> GenerateDictionary(IEnumerable<(string name, LineEdit input)> inputs, string propertyName)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var input in inputs)
            {
                dictionary.Add(input.name, input.input.Text);
            }

            return dictionary;
        }

        public static void CopyProperties<T>(T from, T to) where T : Control
        {
            foreach (var property in from.AllAttachedProperties)
            {
                to.SetValue(property.Key, property.Value);
            }
        }
    }
}