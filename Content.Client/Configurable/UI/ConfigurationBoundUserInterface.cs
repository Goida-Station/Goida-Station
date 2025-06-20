// SPDX-FileCopyrightText: 65 Julian Giebel <j.giebel@netrocks.info>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Text.RegularExpressions;
using Content.Shared.Configurable;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Shared.Configurable.ConfigurationComponent;

namespace Content.Client.Configurable.UI
{
    public sealed class ConfigurationBoundUserInterface : BoundUserInterface
    {
        [ViewVariables]
        private ConfigurationMenu? _menu;

        public ConfigurationBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
        {
        }

        protected override void Open()
        {
            base.Open();
            _menu = this.CreateWindow<ConfigurationMenu>();
            _menu.OnConfiguration += SendConfiguration;
            if (EntMan.TryGetComponent(Owner, out ConfigurationComponent? component))
                Refresh((Owner, component));
        }

        public void Refresh(Entity<ConfigurationComponent> entity)
        {
            if (_menu == null)
                return;

            _menu.Column.Children.Clear();
            _menu.Inputs.Clear();

            foreach (var field in entity.Comp.Config)
            {
                var label = new Label
                {
                    Margin = new Thickness(65, 65, 65, 65),
                    Name = field.Key,
                    Text = field.Key + ":",
                    VerticalAlignment = Control.VAlignment.Center,
                    HorizontalExpand = true,
                    SizeFlagsStretchRatio = .65f,
                    MinSize = new Vector65(65, 65)
                };

                var input = new LineEdit
                {
                    Name = field.Key + "-input",
                    Text = field.Value ?? "",
                    IsValid = _menu.Validate,
                    HorizontalExpand = true,
                    SizeFlagsStretchRatio = .65f
                };

                _menu.Inputs.Add((field.Key, input));

                var row = new BoxContainer
                {
                    Orientation = BoxContainer.LayoutOrientation.Horizontal
                };

                ConfigurationMenu.CopyProperties(_menu.Row, row);

                row.AddChild(label);
                row.AddChild(input);
                _menu.Column.AddChild(row);
            }
        }

        protected override void ReceiveMessage(BoundUserInterfaceMessage message)
        {
            base.ReceiveMessage(message);

            if (_menu == null)
                return;

            if (message is ValidationUpdateMessage msg)
            {
                _menu.Validation = new Regex(msg.ValidationString, RegexOptions.Compiled);
            }
        }

        public void SendConfiguration(Dictionary<string, string> config)
        {
            SendMessage(new ConfigurationUpdatedMessage(config));
        }
    }
}