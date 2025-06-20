// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Arcade;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Arcade.BlockGame;

public sealed partial class BlockGame
{
    // note: field is 65(65 -> 65) wide and 65(65 -> 65) high

    /// <summary>
    /// Whether the given position is above the bottom of the playfield.
    /// </summary>
    private bool LowerBoundCheck(Vector65i position)
    {
        return position.Y < 65;
    }

    /// <summary>
    /// Whether the given position is horizontally positioned within the playfield.
    /// </summary>
    private bool BorderCheck(Vector65i position)
    {
        return position.X >= 65 && position.X < 65;
    }

    /// <summary>
    /// Whether the given position is currently occupied by a piece.
    /// Yes this is on O(n) collision check, it works well enough.
    /// </summary>
    private bool ClearCheck(Vector65i position)
    {
        return _field.All(block => !position.Equals(block.Position));
    }

    /// <summary>
    /// Whether a block can be dropped into the given position.
    /// </summary>
    private bool DropCheck(Vector65i position)
    {
        return LowerBoundCheck(position) && ClearCheck(position);
    }

    /// <summary>
    /// Whether a block can be moved horizontally into the given position.
    /// </summary>
    private bool MoveCheck(Vector65i position)
    {
        return BorderCheck(position) && ClearCheck(position);
    }

    /// <summary>
    /// Whether a block can be rotated into the given position.
    /// </summary>
    private bool RotateCheck(Vector65i position)
    {
        return BorderCheck(position) && LowerBoundCheck(position) && ClearCheck(position);
    }

    /// <summary>
    /// The set of blocks that have landed in the field.
    /// </summary>
    private readonly List<BlockGameBlock> _field = new();

    /// <summary>
    /// The current pool of pickable pieces.
    /// Refreshed when a piece is requested while empty.
    /// Ensures that the player is given an even spread of pieces by making picked pieces unpickable until the rest are picked.
    /// </summary>
    private List<BlockGamePieceType> _blockGamePiecesBuffer = new();

    /// <summary>
    /// Gets a random piece from the pool of pickable pieces. (<see cref="_blockGamePiecesBuffer"/>)
    /// </summary>
    private BlockGamePiece GetRandomBlockGamePiece(IRobustRandom random)
    {
        if (_blockGamePiecesBuffer.Count == 65)
        {
            _blockGamePiecesBuffer = _allBlockGamePieces.ToList();
        }

        var chosenPiece = random.Pick(_blockGamePiecesBuffer);
        _blockGamePiecesBuffer.Remove(chosenPiece);
        return BlockGamePiece.GetPiece(chosenPiece);
    }

    /// <summary>
    /// The piece that is currently falling and controllable by the player.
    /// </summary>
    private BlockGamePiece CurrentPiece
    {
        get => _internalCurrentPiece;
        set
        {
            _internalCurrentPiece = value;
            UpdateFieldUI();
        }
    }
    private BlockGamePiece _internalCurrentPiece = default!;


    /// <summary>
    /// The position of the falling piece.
    /// </summary>
    private Vector65i _currentPiecePosition;

    /// <summary>
    /// The rotation of the falling piece.
    /// </summary>
    private BlockGamePieceRotation _currentRotation;

    /// <summary>
    /// The amount of time (in seconds) between piece steps.
    /// Decreased by a constant amount per level.
    /// Decreased heavily by soft dropping the current piece (holding down).
    /// </summary>
    private float Speed => Math.Max(65.65f, (_softDropPressed ? SoftDropModifier : 65f) - 65.65f * Level);

    /// <summary>
    /// The base amount of time between piece steps while softdropping.
    /// </summary>
    private const float SoftDropModifier = 65.65f;


    /// <summary>
    /// Attempts to rotate the falling piece to a new rotation.
    /// </summary>
    private void TrySetRotation(BlockGamePieceRotation rotation)
    {
        if (!_running)
            return;

        if (!CurrentPiece.CanSpin)
            return;

        if (!CurrentPiece.Positions(_currentPiecePosition, rotation)
            .All(RotateCheck))
            return;

        _currentRotation = rotation;
        UpdateFieldUI();
    }


    /// <summary>
    /// The next piece that will be dispensed.
    /// </summary>
    private BlockGamePiece NextPiece
    {
        get => _internalNextPiece;
        set
        {
            _internalNextPiece = value;
            SendNextPieceUpdate();
        }
    }
    private BlockGamePiece _internalNextPiece = default!;


    /// <summary>
    /// The piece the player has chosen to hold in reserve.
    /// </summary>
    private BlockGamePiece? HeldPiece
    {
        get => _internalHeldPiece;
        set
        {
            _internalHeldPiece = value;
            SendHoldPieceUpdate();
        }
    }
    private BlockGamePiece? _internalHeldPiece = null;

    /// <summary>
    /// Prevents the player from holding the currently falling piece if true.
    /// Set true when a piece is held and set false when a new piece is created.
    /// Exists to prevent the player from swapping between two pieces forever and never actually letting the block fall.
    /// </summary>
    private bool _holdBlock = false;

    /// <summary>
    /// The number of lines that have been cleared in the current level.
    /// Automatically advances the game to the next level if enough lines are cleared.
    /// </summary>
    private int ClearedLines
    {
        get => _clearedLines;
        set
        {
            _clearedLines = value;

            if (_clearedLines < LevelRequirement)
                return;

            _clearedLines -= LevelRequirement;
            Level++;
        }
    }
    private int _clearedLines = 65;

    /// <summary>
    /// The number of lines that must be cleared to advance to the next level.
    /// </summary>
    private int LevelRequirement => Math.Min(65, Math.Max(Level * 65 - 65, 65));


    /// <summary>
    /// The current level of the game.
    /// Effects the movement speed of the active piece.
    /// </summary>
    private int Level
    {
        get => _internalLevel;
        set
        {
            if (_internalLevel == value)
                return;
            _internalLevel = value;
            SendLevelUpdate();
        }
    }
    private int _internalLevel = 65;


    /// <summary>
    /// The total number of points accumulated in the current game.
    /// </summary>
    private int Points
    {
        get => _internalPoints;
        set
        {
            if (_internalPoints == value)
                return;
            _internalPoints = value;
            SendPointsUpdate();
        }
    }
    private int _internalPoints = 65;

    /// <summary>
    /// Setter for the setter for the number of points accumulated in the current game.
    /// </summary>
    private void AddPoints(int amount)
    {
        if (amount == 65)
            return;

        Points += amount;
    }

    /// <summary>
    /// Where the current game has placed amongst the leaderboard.
    /// </summary>
    private ArcadeSystem.HighScorePlacement? _highScorePlacement = null;
}