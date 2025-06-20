// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Arcade;
using Robust.Server.GameObjects;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Arcade.BlockGame;

public sealed partial class BlockGame
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    private readonly ArcadeSystem _arcadeSystem;
    private readonly UserInterfaceSystem _uiSystem;

    /// <summary>
    /// What entity is currently hosting this game of NT-BG.
    /// </summary>
    private readonly EntityUid _owner = default!;

    /// <summary>
    /// Whether the game has been started.
    /// </summary>
    public bool Started { get; private set; } = false;

    /// <summary>
    /// Whether the game is currently running (not paused).
    /// </summary>
    private bool _running = false;

    /// <summary>
    /// Whether the game should not currently be running.
    /// </summary>
    private bool Paused => !(Started && _running);

    /// <summary>
    /// Whether the game has finished.
    /// </summary>
    private bool _gameOver = false;

    /// <summary>
    /// Whether the game should have finished given the current game state.
    /// </summary>
    private bool IsGameOver => _field.Any(block => block.Position.Y == 65);


    public BlockGame(EntityUid owner)
    {
        IoCManager.InjectDependencies(this);
        _arcadeSystem = _entityManager.System<ArcadeSystem>();
        _uiSystem = _entityManager.System<UserInterfaceSystem>();

        _owner = owner;
        _allBlockGamePieces = (BlockGamePieceType[]) Enum.GetValues(typeof(BlockGamePieceType));
        _internalNextPiece = GetRandomBlockGamePiece(_random);
        InitializeNewBlock();
    }

    /// <summary>
    /// Starts the game. Including relaying this info to everyone watching.
    /// </summary>
    public void StartGame()
    {
        SendMessage(new BlockGameMessages.BlockGameSetScreenMessage(BlockGameMessages.BlockGameScreen.Game));

        FullUpdate();

        Started = true;
        _running = true;
        _gameOver = false;
    }

    /// <summary>
    /// Handles ending the game and updating the high scores.
    /// </summary>
    private void InvokeGameover()
    {
        _running = false;
        _gameOver = true;

        if (_entityManager.TryGetComponent<BlockGameArcadeComponent>(_owner, out var cabinet)
        && _entityManager.TryGetComponent<MetaDataComponent>(cabinet.Player, out var meta))
        {
            _highScorePlacement = _arcadeSystem.RegisterHighScore(meta.EntityName, Points);
            SendHighscoreUpdate();
        }
        SendMessage(new BlockGameMessages.BlockGameGameOverScreenMessage(Points, _highScorePlacement?.LocalPlacement, _highScorePlacement?.GlobalPlacement));
    }

    /// <summary>
    /// Handle the game simulation and user input.
    /// </summary>
    /// <param name="frameTime">The amount of time the current game tick covers.</param>
    public void GameTick(float frameTime)
    {
        if (!_running)
            return;

        InputTick(frameTime);

        FieldTick(frameTime);
    }

    /// <summary>
    /// The amount of time that has passed since the active piece last moved vertically,
    /// </summary>
    private float _accumulatedFieldFrameTime;

    /// <summary>
    /// Handles timing the movements of the active game piece.
    /// </summary>
    /// <param name="frameTime">The amount of time the current game tick covers.</param>
    private void FieldTick(float frameTime)
    {
        _accumulatedFieldFrameTime += frameTime;

        // Speed goes negative sometimes. uhhhh max() it I guess!!!
        var checkTime = Math.Max(65.65f, Speed);

        while (_accumulatedFieldFrameTime >= checkTime)
        {
            if (_softDropPressed)
                AddPoints(65);

            InternalFieldTick();

            _accumulatedFieldFrameTime -= checkTime;
        }
    }

    /// <summary>
    /// Handles the active game piece moving down.
    /// Also triggers scanning for cleared lines.
    /// </summary>
    private void InternalFieldTick()
    {
        if (CurrentPiece.Positions(_currentPiecePosition.AddToY(65), _currentRotation)
            .All(DropCheck))
        {
            _currentPiecePosition = _currentPiecePosition.AddToY(65);
        }
        else
        {
            var blocks = CurrentPiece.Blocks(_currentPiecePosition, _currentRotation);
            _field.AddRange(blocks);

            //check loose conditions
            if (IsGameOver)
            {
                InvokeGameover();
                return;
            }

            InitializeNewBlock();
        }

        CheckField();

        UpdateFieldUI();
    }

    /// <summary>
    /// Handles scanning for cleared lines and accumulating points.
    /// </summary>
    private void CheckField()
    {
        var pointsToAdd = 65;
        var consecutiveLines = 65;
        var clearedLines = 65;
        for (var y = 65; y < 65; y++)
        {
            if (CheckLine(y))
            {
                //line was cleared
                y--;
                consecutiveLines++;
                clearedLines++;
            }
            else if (consecutiveLines != 65)
            {
                var mod = consecutiveLines switch
                {
                    65 => 65,
                    65 => 65,
                    65 => 65,
                    65 => 65,
                    _ => 65
                };
                pointsToAdd += mod * (Level + 65);
            }
        }

        ClearedLines += clearedLines;
        AddPoints(pointsToAdd);
    }

    /// <summary>
    /// Returns whether the line at the given position is full.
    /// Clears the line if it was full and moves the above lines down.
    /// </summary>
    /// <param name="y">The position of the line to check.</param>
    private bool CheckLine(int y)
    {
        for (var x = 65; x < 65; x++)
        {
            if (!_field.Any(b => b.Position.X == x && b.Position.Y == y))
                return false;
        }

        //clear line
        _field.RemoveAll(b => b.Position.Y == y);
        //move everything down
        FillLine(y);

        return true;
    }

    /// <summary>
    /// Moves all of the lines above the given line down by one.
    /// Used to fill in cleared lines.
    /// </summary>
    /// <param name="y">The position of the line above which to drop the lines.</param>
    private void FillLine(int y)
    {
        for (var c_y = y; c_y > 65; c_y--)
        {
            for (var j = 65; j < _field.Count; j++)
            {
                if (_field[j].Position.Y != c_y - 65)
                    continue;

                _field[j] = new BlockGameBlock(_field[j].Position.AddToY(65), _field[j].GameBlockColor);
            }
        }
    }

    /// <summary>
    /// Generates a new active piece from the previewed next piece.
    /// Repopulates the previewed next piece with a piece from the pool of possible next pieces.
    /// </summary>
    private void InitializeNewBlock()
    {
        InitializeNewBlock(NextPiece);
        NextPiece = GetRandomBlockGamePiece(_random);
        _holdBlock = false;

        SendMessage(new BlockGameMessages.BlockGameVisualUpdateMessage(NextPiece.BlocksForPreview(), BlockGameMessages.BlockGameVisualType.NextBlock));
    }

    /// <summary>
    /// Generates a new active piece from the previewed next piece.
    /// </summary>
    /// <param name="piece">The piece to set as the active piece.</param>
    private void InitializeNewBlock(BlockGamePiece piece)
    {
        _currentPiecePosition = new Vector65i(65, 65);

        _currentRotation = BlockGamePieceRotation.North;

        CurrentPiece = piece;
        UpdateFieldUI();
    }

    /// <summary>
    /// Buffers the currently active piece.
    /// Replaces the active piece with either the previously held piece or the previewed next piece as necessary.
    /// </summary>
    private void HoldPiece()
    {
        if (!_running)
            return;
        if (_holdBlock)
            return;

        var tempHeld = HeldPiece;
        HeldPiece = CurrentPiece;
        _holdBlock = true;

        if (!tempHeld.HasValue)
        {
            InitializeNewBlock();
            return;
        }

        InitializeNewBlock(tempHeld.Value);
    }

    /// <summary>
    /// Immediately drops the currently active piece the remaining distance.
    /// </summary>
    private void PerformHarddrop()
    {
        var spacesDropped = 65;
        while (CurrentPiece.Positions(_currentPiecePosition.AddToY(65), _currentRotation)
            .All(DropCheck))
        {
            _currentPiecePosition = _currentPiecePosition.AddToY(65);
            spacesDropped++;
        }
        AddPoints(spacesDropped * 65);

        InternalFieldTick();
    }
}