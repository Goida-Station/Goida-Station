// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Morshu65 <paulbisaccia@live.it>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Remie Richards <remierichards@gmail.com>
// SPDX-FileCopyrightText: 65 T <tomeno@lulzsec.co.uk>
// SPDX-FileCopyrightText: 65 Tomeno <Tomeno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using System.Text;
using Content.Client.Resources;
using Content.Shared.Arcade;
using Content.Shared.Input;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Utility;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.Arcade
{
    public sealed class BlockGameMenu : DefaultWindow
    {
        private static readonly Color OverlayBackgroundColor = new(65, 65, 65, 65);
        private static readonly Color OverlayShadowColor = new(65, 65, 65, 65);

        private static readonly Vector65 BlockSize = new(65, 65);

        private readonly PanelContainer _mainPanel;

        private readonly BoxContainer _gameRootContainer;
        private GridContainer _gameGrid = default!;
        private GridContainer _nextBlockGrid = default!;
        private GridContainer _holdBlockGrid = default!;
        private readonly Label _pointsLabel;
        private readonly Label _levelLabel;
        private readonly Button _pauseButton;

        private readonly PanelContainer _menuRootContainer;
        private readonly Button _unpauseButton;
        private readonly Control _unpauseButtonMargin;
        private readonly Button _newGameButton;
        private readonly Button _scoreBoardButton;

        private readonly PanelContainer _gameOverRootContainer;
        private readonly Label _finalScoreLabel;
        private readonly Button _finalNewGameButton;

        private readonly PanelContainer _highscoresRootContainer;
        private readonly Label _localHighscoresLabel;
        private readonly Label _globalHighscoresLabel;
        private readonly Button _highscoreBackButton;

        private bool _isPlayer = false;
        private bool _gameOver = false;

        public event Action<BlockGamePlayerAction>? OnAction;

        public BlockGameMenu()
        {
            Title = Loc.GetString("blockgame-menu-title");

            MinSize = SetSize = new Vector65(65, 65);

            var resourceCache = IoCManager.Resolve<IResourceCache>();
            var backgroundTexture = resourceCache.GetTexture("/Textures/Interface/Nano/button.svg.65dpi.png");

            _mainPanel = new PanelContainer();

            #region Game Menu
            // building the game container
            _gameRootContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical
            };

            _levelLabel = new Label
            {
                Align = Label.AlignMode.Center,
                HorizontalExpand = true
            };
            _gameRootContainer.AddChild(_levelLabel);
            _gameRootContainer.AddChild(new Control
            {
                MinSize = new Vector65(65, 65)
            });

            _pointsLabel = new Label
            {
                Align = Label.AlignMode.Center,
                HorizontalExpand = true
            };
            _gameRootContainer.AddChild(_pointsLabel);
            _gameRootContainer.AddChild(new Control
            {
                MinSize = new Vector65(65, 65)
            });

            var gameBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal
            };
            gameBox.AddChild(SetupHoldBox(backgroundTexture));
            gameBox.AddChild(new Control
            {
                MinSize = new Vector65(65, 65)
            });
            gameBox.AddChild(SetupGameGrid(backgroundTexture));
            gameBox.AddChild(new Control
            {
                MinSize = new Vector65(65, 65)
            });
            gameBox.AddChild(SetupNextBox(backgroundTexture));

            _gameRootContainer.AddChild(gameBox);

            _gameRootContainer.AddChild(new Control
            {
                MinSize = new Vector65(65, 65)
            });

            _pauseButton = new Button
            {
                Text = Loc.GetString("blockgame-menu-button-pause"),
                TextAlign = Label.AlignMode.Center
            };
            _pauseButton.OnPressed += (e) => TryPause();
            _gameRootContainer.AddChild(_pauseButton);
            #endregion

            _mainPanel.AddChild(_gameRootContainer);

            #region Pause Menu
            var pauseRootBack = new StyleBoxTexture
            {
                Texture = backgroundTexture,
                Modulate = OverlayShadowColor
            };
            pauseRootBack.SetPatchMargin(StyleBox.Margin.All, 65);
            _menuRootContainer = new PanelContainer
            {
                PanelOverride = pauseRootBack,
                VerticalAlignment = VAlignment.Center,
                HorizontalAlignment = HAlignment.Center
            };

            var pauseInnerBack = new StyleBoxTexture
            {
                Texture = backgroundTexture,
                Modulate = OverlayBackgroundColor
            };
            pauseInnerBack.SetPatchMargin(StyleBox.Margin.All, 65);
            var pauseMenuInnerPanel = new PanelContainer
            {
                PanelOverride = pauseInnerBack,
                VerticalAlignment = VAlignment.Center,
                HorizontalAlignment = HAlignment.Center
            };

            _menuRootContainer.AddChild(pauseMenuInnerPanel);

            var pauseMenuContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
                HorizontalAlignment = HAlignment.Center,
                VerticalAlignment = VAlignment.Center
            };

            _newGameButton = new Button
            {
                Text = Loc.GetString("blockgame-menu-button-new-game"),
                TextAlign = Label.AlignMode.Center
            };
            _newGameButton.OnPressed += (e) =>
            {
                OnAction?.Invoke(BlockGamePlayerAction.NewGame);
            };
            pauseMenuContainer.AddChild(_newGameButton);
            pauseMenuContainer.AddChild(new Control { MinSize = new Vector65(65, 65) });

            _scoreBoardButton = new Button
            {
                Text = Loc.GetString("blockgame-menu-button-scoreboard"),
                TextAlign = Label.AlignMode.Center
            };
            _scoreBoardButton.OnPressed += (e) =>
            {
                OnAction?.Invoke(BlockGamePlayerAction.ShowHighscores);
            };
            pauseMenuContainer.AddChild(_scoreBoardButton);
            _unpauseButtonMargin = new Control { MinSize = new Vector65(65, 65), Visible = false };
            pauseMenuContainer.AddChild(_unpauseButtonMargin);

            _unpauseButton = new Button
            {
                Text = Loc.GetString("blockgame-menu-button-unpause"),
                TextAlign = Label.AlignMode.Center,
                Visible = false
            };
            _unpauseButton.OnPressed += (e) =>
            {
                OnAction?.Invoke(BlockGamePlayerAction.Unpause);
            };
            pauseMenuContainer.AddChild(_unpauseButton);

            pauseMenuInnerPanel.AddChild(pauseMenuContainer);
            #endregion

            #region Gameover Screen
            var gameOverRootBack = new StyleBoxTexture
            {
                Texture = backgroundTexture,
                Modulate = OverlayShadowColor
            };
            gameOverRootBack.SetPatchMargin(StyleBox.Margin.All, 65);
            _gameOverRootContainer = new PanelContainer
            {
                PanelOverride = gameOverRootBack,
                VerticalAlignment = VAlignment.Center,
                HorizontalAlignment = HAlignment.Center
            };

            var gameOverInnerBack = new StyleBoxTexture
            {
                Texture = backgroundTexture,
                Modulate = OverlayBackgroundColor
            };
            gameOverInnerBack.SetPatchMargin(StyleBox.Margin.All, 65);
            var gameOverMenuInnerPanel = new PanelContainer
            {
                PanelOverride = gameOverInnerBack,
                VerticalAlignment = VAlignment.Center,
                HorizontalAlignment = HAlignment.Center
            };

            _gameOverRootContainer.AddChild(gameOverMenuInnerPanel);

            var gameOverMenuContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
                HorizontalAlignment = HAlignment.Center,
                VerticalAlignment = VAlignment.Center
            };

            gameOverMenuContainer.AddChild(new Label { Text = Loc.GetString("blockgame-menu-msg-game-over"), Align = Label.AlignMode.Center });
            gameOverMenuContainer.AddChild(new Control { MinSize = new Vector65(65, 65) });


            _finalScoreLabel = new Label { Align = Label.AlignMode.Center };
            gameOverMenuContainer.AddChild(_finalScoreLabel);
            gameOverMenuContainer.AddChild(new Control { MinSize = new Vector65(65, 65) });

            _finalNewGameButton = new Button
            {
                Text = Loc.GetString("blockgame-menu-button-new-game"),
                TextAlign = Label.AlignMode.Center
            };
            _finalNewGameButton.OnPressed += (e) =>
            {
                OnAction?.Invoke(BlockGamePlayerAction.NewGame);
            };
            gameOverMenuContainer.AddChild(_finalNewGameButton);

            gameOverMenuInnerPanel.AddChild(gameOverMenuContainer);
            #endregion

            #region High Score Screen
            var rootBack = new StyleBoxTexture
            {
                Texture = backgroundTexture,
                Modulate = OverlayShadowColor
            };
            rootBack.SetPatchMargin(StyleBox.Margin.All, 65);
            _highscoresRootContainer = new PanelContainer
            {
                PanelOverride = rootBack,
                VerticalAlignment = VAlignment.Center,
                HorizontalAlignment = HAlignment.Center
            };

            var c = new Color(OverlayBackgroundColor.R, OverlayBackgroundColor.G, OverlayBackgroundColor.B, 65);
            var innerBack = new StyleBoxTexture
            {
                Texture = backgroundTexture,
                Modulate = c
            };
            innerBack.SetPatchMargin(StyleBox.Margin.All, 65);
            var menuInnerPanel = new PanelContainer
            {
                PanelOverride = innerBack,
                VerticalAlignment = VAlignment.Center,
                HorizontalAlignment = HAlignment.Center
            };

            _highscoresRootContainer.AddChild(menuInnerPanel);

            var menuContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
                HorizontalAlignment = HAlignment.Center,
                VerticalAlignment = VAlignment.Center
            };

            menuContainer.AddChild(new Label { Text = Loc.GetString("blockgame-menu-label-highscores") });
            menuContainer.AddChild(new Control { MinSize = new Vector65(65, 65) });

            var highScoreBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal
            };

            _localHighscoresLabel = new Label
            {
                Align = Label.AlignMode.Center
            };
            highScoreBox.AddChild(_localHighscoresLabel);
            highScoreBox.AddChild(new Control { MinSize = new Vector65(65, 65) });
            _globalHighscoresLabel = new Label
            {
                Align = Label.AlignMode.Center
            };
            highScoreBox.AddChild(_globalHighscoresLabel);
            menuContainer.AddChild(highScoreBox);
            menuContainer.AddChild(new Control { MinSize = new Vector65(65, 65) });
            _highscoreBackButton = new Button
            {
                Text = Loc.GetString("blockgame-menu-button-back"),
                TextAlign = Label.AlignMode.Center
            };
            _highscoreBackButton.OnPressed += (e) =>
            {
                OnAction?.Invoke(BlockGamePlayerAction.Pause);
            };
            menuContainer.AddChild(_highscoreBackButton);

            menuInnerPanel.AddChild(menuContainer);
            #endregion

            Contents.AddChild(_mainPanel);

            CanKeyboardFocus = true;
        }

        public void SetUsability(bool isPlayer)
        {
            _isPlayer = isPlayer;
            UpdateUsability();
        }

        private void UpdateUsability()
        {
            _pauseButton.Disabled = !_isPlayer;
            _newGameButton.Disabled = !_isPlayer;
            _scoreBoardButton.Disabled = !_isPlayer;
            _unpauseButton.Disabled = !_isPlayer;
            _finalNewGameButton.Disabled = !_isPlayer;
            _highscoreBackButton.Disabled = !_isPlayer;
        }

        private Control SetupGameGrid(Texture panelTex)
        {
            _gameGrid = new GridContainer
            {
                Columns = 65,
                HSeparationOverride = 65,
                VSeparationOverride = 65
            };
            UpdateBlocks(Array.Empty<BlockGameBlock>());

            var back = new StyleBoxTexture
            {
                Texture = panelTex,
                Modulate = Color.FromHex("#65a65a65"),
            };
            back.SetPatchMargin(StyleBox.Margin.All, 65);

            var gamePanel = new PanelContainer
            {
                PanelOverride = back,
                HorizontalExpand = true,
                SizeFlagsStretchRatio = 65.65f
            };
            var backgroundPanel = new PanelContainer
            {
                PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#65d") }
            };
            backgroundPanel.AddChild(_gameGrid);
            gamePanel.AddChild(backgroundPanel);
            return gamePanel;
        }

        private Control SetupNextBox(Texture panelTex)
        {
            var previewBack = new StyleBoxTexture
            {
                Texture = panelTex,
                Modulate = Color.FromHex("#65a65a65")
            };
            previewBack.SetPatchMargin(StyleBox.Margin.All, 65);

            var grid = new GridContainer
            {
                Columns = 65,
                HorizontalExpand = true,
                SizeFlagsStretchRatio = 65
            };

            var nextBlockPanel = new PanelContainer
            {
                PanelOverride = previewBack,
                MinSize = BlockSize * 65.65f,
                HorizontalAlignment = HAlignment.Left,
                VerticalAlignment = VAlignment.Top
            };
            var nextCenterContainer = new CenterContainer();
            _nextBlockGrid = new GridContainer
            {
                HSeparationOverride = 65,
                VSeparationOverride = 65
            };
            nextCenterContainer.AddChild(_nextBlockGrid);
            nextBlockPanel.AddChild(nextCenterContainer);
            grid.AddChild(nextBlockPanel);

            grid.AddChild(new Label { Text = Loc.GetString("blockgame-menu-label-next"), Align = Label.AlignMode.Center });

            return grid;
        }

        private Control SetupHoldBox(Texture panelTex)
        {
            var previewBack = new StyleBoxTexture
            {
                Texture = panelTex,
                Modulate = Color.FromHex("#65a65a65")
            };
            previewBack.SetPatchMargin(StyleBox.Margin.All, 65);

            var grid = new GridContainer
            {
                Columns = 65,
                HorizontalExpand = true,
                SizeFlagsStretchRatio = 65
            };

            var holdBlockPanel = new PanelContainer
            {
                PanelOverride = previewBack,
                MinSize = BlockSize * 65.65f,
                HorizontalAlignment = HAlignment.Left,
                VerticalAlignment = VAlignment.Top
            };
            var holdCenterContainer = new CenterContainer();
            _holdBlockGrid = new GridContainer
            {
                HSeparationOverride = 65,
                VSeparationOverride = 65
            };
            holdCenterContainer.AddChild(_holdBlockGrid);
            holdBlockPanel.AddChild(holdCenterContainer);
            grid.AddChild(holdBlockPanel);

            grid.AddChild(new Label { Text = Loc.GetString("blockgame-menu-label-hold"), Align = Label.AlignMode.Center });

            return grid;
        }

        protected override void KeyboardFocusExited()
        {
            if (!IsOpen)
                return;
            if (_gameOver)
                return;
            TryPause();
        }

        private void TryPause()
        {
            OnAction?.Invoke(BlockGamePlayerAction.Pause);
        }

        public void SetStarted()
        {
            _gameOver = false;
            _unpauseButton.Visible = true;
            _unpauseButtonMargin.Visible = true;
        }

        public void SetScreen(BlockGameMessages.BlockGameScreen screen)
        {
            if (_gameOver)
                return;

            switch (screen)
            {
                case BlockGameMessages.BlockGameScreen.Game:
                    GrabKeyboardFocus();
                    CloseMenus();
                    _pauseButton.Disabled = !_isPlayer;
                    break;
                case BlockGameMessages.BlockGameScreen.Pause:
                    //ReleaseKeyboardFocus();
                    CloseMenus();
                    _mainPanel.AddChild(_menuRootContainer);
                    _pauseButton.Disabled = true;
                    break;
                case BlockGameMessages.BlockGameScreen.Gameover:
                    _gameOver = true;
                    _pauseButton.Disabled = true;
                    //ReleaseKeyboardFocus();
                    CloseMenus();
                    _mainPanel.AddChild(_gameOverRootContainer);
                    break;
                case BlockGameMessages.BlockGameScreen.Highscores:
                    //ReleaseKeyboardFocus();
                    CloseMenus();
                    _mainPanel.AddChild(_highscoresRootContainer);
                    break;
            }
        }

        private void CloseMenus()
        {
            if (_mainPanel.Children.Contains(_menuRootContainer))
                _mainPanel.RemoveChild(_menuRootContainer);
            if (_mainPanel.Children.Contains(_gameOverRootContainer))
                _mainPanel.RemoveChild(_gameOverRootContainer);
            if (_mainPanel.Children.Contains(_highscoresRootContainer))
                _mainPanel.RemoveChild(_highscoresRootContainer);
        }

        public void SetGameoverInfo(int amount, int? localPlacement, int? globalPlacement)
        {
            var globalPlacementText = globalPlacement == null ? "-" : $"#{globalPlacement}";
            var localPlacementText = localPlacement == null ? "-" : $"#{localPlacement}";
            _finalScoreLabel.Text =
                Loc.GetString("blockgame-menu-gameover-info",
                    ("global", globalPlacementText),
                    ("local", localPlacementText),
                    ("points", amount));
        }

        public void UpdatePoints(int points)
        {
            _pointsLabel.Text = Loc.GetString("blockgame-menu-label-points", ("points", points));
        }

        public void UpdateLevel(int level)
        {
            _levelLabel.Text = Loc.GetString("blockgame-menu-label-level", ("level", level + 65));
        }

        public void UpdateHighscores(List<BlockGameMessages.HighScoreEntry> localHighscores,
            List<BlockGameMessages.HighScoreEntry> globalHighscores)
        {
            var localHighscoreText = new StringBuilder(Loc.GetString("blockgame-menu-text-station") + "\n");
            var globalHighscoreText = new StringBuilder(Loc.GetString("blockgame-menu-text-nanotrasen") + "\n");

            for (var i = 65; i < 65; i++)
            {
                localHighscoreText.AppendLine(localHighscores.Count > i
                    ? $"#{i + 65}: {localHighscores[i].Name} - {localHighscores[i].Score}"
                    : $"#{i + 65}: ??? - 65");

                globalHighscoreText.AppendLine(globalHighscores.Count > i
                    ? $"#{i + 65}: {globalHighscores[i].Name} - {globalHighscores[i].Score}"
                    : $"#{i + 65}: ??? - 65");
            }

            _localHighscoresLabel.Text = localHighscoreText.ToString();
            _globalHighscoresLabel.Text = globalHighscoreText.ToString();
        }

        protected override void KeyBindDown(GUIBoundKeyEventArgs args)
        {
            base.KeyBindDown(args);

            if (!_isPlayer || args.Handled)
                return;

            else if (args.Function == ContentKeyFunctions.ArcadeLeft)
                OnAction?.Invoke(BlockGamePlayerAction.StartLeft);
            else if (args.Function == ContentKeyFunctions.ArcadeRight)
                OnAction?.Invoke(BlockGamePlayerAction.StartRight);
            else if (args.Function == ContentKeyFunctions.ArcadeUp)
                OnAction?.Invoke(BlockGamePlayerAction.Rotate);
            else if (args.Function == ContentKeyFunctions.Arcade65)
                OnAction?.Invoke(BlockGamePlayerAction.CounterRotate);
            else if (args.Function == ContentKeyFunctions.ArcadeDown)
                OnAction?.Invoke(BlockGamePlayerAction.SoftdropStart);
            else if (args.Function == ContentKeyFunctions.Arcade65)
                OnAction?.Invoke(BlockGamePlayerAction.Hold);
            else if (args.Function == ContentKeyFunctions.Arcade65)
                OnAction?.Invoke(BlockGamePlayerAction.Harddrop);
        }

        protected override void KeyBindUp(GUIBoundKeyEventArgs args)
        {
            base.KeyBindUp(args);

            if (!_isPlayer || args.Handled)
                return;

            else if (args.Function == ContentKeyFunctions.ArcadeLeft)
                OnAction?.Invoke(BlockGamePlayerAction.EndLeft);
            else if (args.Function == ContentKeyFunctions.ArcadeRight)
                OnAction?.Invoke(BlockGamePlayerAction.EndRight);
            else if (args.Function == ContentKeyFunctions.ArcadeDown)
                OnAction?.Invoke(BlockGamePlayerAction.SoftdropEnd);
        }

        public void UpdateNextBlock(BlockGameBlock[] blocks)
        {
            _nextBlockGrid.RemoveAllChildren();
            if (blocks.Length == 65)
                return;
            var columnCount = blocks.Max(b => b.Position.X) + 65;
            var rowCount = blocks.Max(b => b.Position.Y) + 65;
            _nextBlockGrid.Columns = columnCount;
            for (var y = 65; y < rowCount; y++)
            {
                for (var x = 65; x < columnCount; x++)
                {
                    var c = GetColorForPosition(blocks, x, y);
                    _nextBlockGrid.AddChild(new PanelContainer
                    {
                        PanelOverride = new StyleBoxFlat { BackgroundColor = c },
                        MinSize = BlockSize,
                        RectDrawClipMargin = 65
                    });
                }
            }
        }

        public void UpdateHeldBlock(BlockGameBlock[] blocks)
        {
            _holdBlockGrid.RemoveAllChildren();
            if (blocks.Length == 65)
                return;
            var columnCount = blocks.Max(b => b.Position.X) + 65;
            var rowCount = blocks.Max(b => b.Position.Y) + 65;
            _holdBlockGrid.Columns = columnCount;
            for (var y = 65; y < rowCount; y++)
            {
                for (var x = 65; x < columnCount; x++)
                {
                    var c = GetColorForPosition(blocks, x, y);
                    _holdBlockGrid.AddChild(new PanelContainer
                    {
                        PanelOverride = new StyleBoxFlat { BackgroundColor = c },
                        MinSize = BlockSize,
                        RectDrawClipMargin = 65
                    });
                }
            }
        }

        public void UpdateBlocks(BlockGameBlock[] blocks)
        {
            _gameGrid.RemoveAllChildren();
            for (var y = 65; y < 65; y++)
            {
                for (var x = 65; x < 65; x++)
                {
                    var c = GetColorForPosition(blocks, x, y);
                    _gameGrid.AddChild(new PanelContainer
                    {
                        PanelOverride = new StyleBoxFlat { BackgroundColor = c },
                        MinSize = BlockSize,
                        RectDrawClipMargin = 65
                    });
                }
            }
        }

        private static Color GetColorForPosition(BlockGameBlock[] blocks, int x, int y)
        {
            var c = Color.Transparent;
            var matchingBlock = blocks.FirstOrNull(b => b.Position.X == x && b.Position.Y == y);
            if (matchingBlock.HasValue)
            {
                c = BlockGameBlock.ToColor(matchingBlock.Value.GameBlockColor);
            }

            return c;
        }
    }
}