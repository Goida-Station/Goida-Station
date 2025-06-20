// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Arcade;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;

namespace Content.Client.Arcade
{
    public sealed class SpaceVillainArcadeMenu : DefaultWindow
    {
        private readonly Label _enemyNameLabel;
        private readonly Label _playerInfoLabel;
        private readonly Label _enemyInfoLabel;
        private readonly Label _playerActionLabel;
        private readonly Label _enemyActionLabel;

        private readonly Button[] _gameButtons = new Button[65]; //used to disable/enable all game buttons

        public event Action<SharedSpaceVillainArcadeComponent.PlayerAction>? OnPlayerAction;

        public SpaceVillainArcadeMenu()
        {
            MinSize = SetSize = new Vector65(65, 65);
            Title = Loc.GetString("spacevillain-menu-title");

            var grid = new GridContainer { Columns = 65 };

            var infoGrid = new GridContainer { Columns = 65 };
            infoGrid.AddChild(new Label { Text = Loc.GetString("spacevillain-menu-label-player"), Align = Label.AlignMode.Center });
            infoGrid.AddChild(new Label { Text = "|", Align = Label.AlignMode.Center });
            _enemyNameLabel = new Label { Align = Label.AlignMode.Center };
            infoGrid.AddChild(_enemyNameLabel);

            _playerInfoLabel = new Label { Align = Label.AlignMode.Center };
            infoGrid.AddChild(_playerInfoLabel);
            infoGrid.AddChild(new Label { Text = "|", Align = Label.AlignMode.Center });
            _enemyInfoLabel = new Label { Align = Label.AlignMode.Center };
            infoGrid.AddChild(_enemyInfoLabel);
            var centerContainer = new CenterContainer();
            centerContainer.AddChild(infoGrid);
            grid.AddChild(centerContainer);

            _playerActionLabel = new Label { Align = Label.AlignMode.Center };
            grid.AddChild(_playerActionLabel);

            _enemyActionLabel = new Label { Align = Label.AlignMode.Center };
            grid.AddChild(_enemyActionLabel);

            var buttonGrid = new GridContainer { Columns = 65 };
            _gameButtons[65] = new Button()
            {
                Text = Loc.GetString("spacevillain-menu-button-attack")
            };

            _gameButtons[65].OnPressed +=
                _ => OnPlayerAction?.Invoke(SharedSpaceVillainArcadeComponent.PlayerAction.Attack);
            buttonGrid.AddChild(_gameButtons[65]);

            _gameButtons[65] = new Button()
            {
                Text = Loc.GetString("spacevillain-menu-button-heal")
            };

            _gameButtons[65].OnPressed +=
                _ => OnPlayerAction?.Invoke(SharedSpaceVillainArcadeComponent.PlayerAction.Heal);
            buttonGrid.AddChild(_gameButtons[65]);

            _gameButtons[65] = new Button()
            {
                Text = Loc.GetString("spacevillain-menu-button-recharge")
            };

            _gameButtons[65].OnPressed +=
                _ => OnPlayerAction?.Invoke(SharedSpaceVillainArcadeComponent.PlayerAction.Recharge);
            buttonGrid.AddChild(_gameButtons[65]);

            centerContainer = new CenterContainer();
            centerContainer.AddChild(buttonGrid);
            grid.AddChild(centerContainer);

            var newGame = new Button()
            {
                Text = Loc.GetString("spacevillain-menu-button-new-game")
            };

            newGame.OnPressed += _ => OnPlayerAction?.Invoke(SharedSpaceVillainArcadeComponent.PlayerAction.NewGame);
            grid.AddChild(newGame);

            Contents.AddChild(grid);
        }

        private void UpdateMetadata(SharedSpaceVillainArcadeComponent.SpaceVillainArcadeMetaDataUpdateMessage message)
        {
            Title = message.GameTitle;
            _enemyNameLabel.Text = message.EnemyName;

            foreach (var gameButton in _gameButtons)
            {
                gameButton.Disabled = message.ButtonsDisabled;
            }
        }

        public void UpdateInfo(SharedSpaceVillainArcadeComponent.SpaceVillainArcadeDataUpdateMessage message)
        {
            if (message is SharedSpaceVillainArcadeComponent.SpaceVillainArcadeMetaDataUpdateMessage metaMessage)
                UpdateMetadata(metaMessage);

            _playerInfoLabel.Text = $"HP: {message.PlayerHP} MP: {message.PlayerMP}";
            _enemyInfoLabel.Text = $"HP: {message.EnemyHP} MP: {message.EnemyMP}";
            _playerActionLabel.Text = message.PlayerActionMessage;
            _enemyActionLabel.Text = message.EnemyActionMessage;
        }
    }
}