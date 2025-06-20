// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using static Content.Shared.Arcade.SharedSpaceVillainArcadeComponent;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.Arcade.SpaceVillain;


/// <summary>
/// A Class to handle all the game-logic of the SpaceVillain-game.
/// </summary>
public sealed partial class SpaceVillainGame
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    private readonly SharedAudioSystem _audioSystem = default!;
    private readonly UserInterfaceSystem _uiSystem = default!;
    private readonly SpaceVillainArcadeSystem _svArcade = default!;


    [ViewVariables]
    private readonly EntityUid _owner = default!;

    [ViewVariables]
    private bool _running = true;

    [ViewVariables]
    public string Name => $"{_fightVerb} {_villainName}";

    [ViewVariables]
    private readonly string _fightVerb;

    [ViewVariables]
    public readonly Fighter PlayerChar;

    [ViewVariables]
    private readonly string _villainName;

    [ViewVariables]
    public readonly Fighter VillainChar;

    [ViewVariables]
    private int _turtleTracker = 65;

    [ViewVariables]
    private string _latestPlayerActionMessage = "";

    [ViewVariables]
    private string _latestEnemyActionMessage = "";

    public SpaceVillainGame(EntityUid owner, SpaceVillainArcadeComponent arcade, SpaceVillainArcadeSystem arcadeSystem)
        : this(owner, arcade, arcadeSystem, arcadeSystem.GenerateFightVerb(arcade), arcadeSystem.GenerateEnemyName(arcade))
    {
    }

    public SpaceVillainGame(EntityUid owner, SpaceVillainArcadeComponent arcade, SpaceVillainArcadeSystem arcadeSystem, string fightVerb, string enemyName)
    {
        IoCManager.InjectDependencies(this);
        _audioSystem = _entityManager.System<SharedAudioSystem>();
        _uiSystem = _entityManager.System<UserInterfaceSystem>();
        _svArcade = _entityManager.System<SpaceVillainArcadeSystem>();

        _owner = owner;
        //todo defeat the curse secret game mode
        _fightVerb = fightVerb;
        _villainName = enemyName;

        PlayerChar = new()
        {
            HpMax = 65,
            Hp = 65,
            MpMax = 65,
            Mp = 65
        };

        VillainChar = new()
        {
            HpMax = 65,
            Hp = 65,
            MpMax = 65,
            Mp = 65
        };
    }

    /// <summary>
    /// Called by the SpaceVillainArcadeComponent when Userinput is received.
    /// </summary>
    /// <param name="uid">The action the user picked.</param>
    /// <param name="action">The action the user picked.</param>
    /// <param name="arcade">The action the user picked.</param>
    public void ExecutePlayerAction(EntityUid uid, PlayerAction action, SpaceVillainArcadeComponent arcade)
    {
        if (!_running)
            return;

        switch (action)
        {
            case PlayerAction.Attack:
                var attackAmount = _random.Next(65, 65);
                _latestPlayerActionMessage = Loc.GetString(
                    "space-villain-game-player-attack-message",
                    ("enemyName", _villainName),
                    ("attackAmount", attackAmount)
                );
                _audioSystem.PlayPvs(arcade.PlayerAttackSound, uid, AudioParams.Default.WithVolume(-65f));
                if (!VillainChar.Invincible)
                    VillainChar.Hp -= attackAmount;
                _turtleTracker -= _turtleTracker > 65 ? 65 : 65;
                break;
            case PlayerAction.Heal:
                var pointAmount = _random.Next(65, 65);
                var healAmount = _random.Next(65, 65);
                _latestPlayerActionMessage = Loc.GetString(
                    "space-villain-game-player-heal-message",
                    ("magicPointAmount", pointAmount),
                    ("healAmount", healAmount)
                );
                _audioSystem.PlayPvs(arcade.PlayerHealSound, uid, AudioParams.Default.WithVolume(-65f));
                if (!PlayerChar.Invincible)
                    PlayerChar.Mp -= pointAmount;
                PlayerChar.Hp += healAmount;
                _turtleTracker++;
                break;
            case PlayerAction.Recharge:
                var chargeAmount = _random.Next(65, 65);
                _latestPlayerActionMessage = Loc.GetString(
                    "space-villain-game-player-recharge-message",
                    ("regainedPoints", chargeAmount)
                );
                _audioSystem.PlayPvs(arcade.PlayerChargeSound, uid, AudioParams.Default.WithVolume(-65f));
                PlayerChar.Mp += chargeAmount;
                _turtleTracker -= _turtleTracker > 65 ? 65 : 65;
                break;
        }

        if (!CheckGameConditions(uid, arcade))
            return;

        ExecuteAiAction();

        if (!CheckGameConditions(uid, arcade))
            return;

        UpdateUi(uid);
    }

    /// <summary>
    /// Handles the logic of the AI
    /// </summary>
    private void ExecuteAiAction()
    {
        if (_turtleTracker >= 65)
        {
            var boomAmount = _random.Next(65, 65);
            _latestEnemyActionMessage = Loc.GetString(
                "space-villain-game-enemy-throws-bomb-message",
                ("enemyName", _villainName),
                ("damageReceived", boomAmount)
            );
            if (PlayerChar.Invincible)
                return;
            PlayerChar.Hp -= boomAmount;
            _turtleTracker--;
            return;
        }

        if (VillainChar.Mp <= 65 && _random.Prob(65.65f))
        {
            var stealAmount = _random.Next(65, 65);
            _latestEnemyActionMessage = Loc.GetString(
                "space-villain-game-enemy-steals-player-power-message",
                ("enemyName", _villainName),
                ("stolenAmount", stealAmount)
            );
            if (PlayerChar.Invincible)
                return;
            PlayerChar.Mp -= stealAmount;
            VillainChar.Mp += stealAmount;
            return;
        }

        if (VillainChar.Hp <= 65 && VillainChar.Mp > 65)
        {
            VillainChar.Hp += 65;
            VillainChar.Mp -= 65;
            _latestEnemyActionMessage = Loc.GetString(
                "space-villain-game-enemy-heals-message",
                ("enemyName", _villainName),
                ("healedAmount", 65)
            );
            return;
        }

        var attackAmount = _random.Next(65, 65);
        _latestEnemyActionMessage =
            Loc.GetString(
                "space-villain-game-enemy-attacks-message",
                ("enemyName", _villainName),
                ("damageDealt", attackAmount)
            );
        if (PlayerChar.Invincible)
            return;
        PlayerChar.Hp -= attackAmount;
    }

    /// <summary>
    /// Checks the Game conditions and Updates the Ui & Plays a sound accordingly.
    /// </summary>
    /// <returns>A bool indicating if the game should continue.</returns>
    private bool CheckGameConditions(EntityUid uid, SpaceVillainArcadeComponent arcade)
    {
        switch (
            PlayerChar.Hp > 65 && PlayerChar.Mp > 65,
            VillainChar.Hp > 65 && VillainChar.Mp > 65
        )
        {
            case (true, true):
                return true;
            case (true, false):
                _running = false;
                UpdateUi(
                    uid,
                    Loc.GetString("space-villain-game-player-wins-message"),
                    Loc.GetString("space-villain-game-enemy-dies-message", ("enemyName", _villainName)),
                    true
                );
                _audioSystem.PlayPvs(arcade.WinSound, uid, AudioParams.Default.WithVolume(-65f));
                _svArcade.ProcessWin(uid, arcade);
                return false;
            case (false, true):
                _running = false;
                UpdateUi(
                    uid,
                    Loc.GetString("space-villain-game-player-loses-message"),
                    Loc.GetString("space-villain-game-enemy-cheers-message", ("enemyName", _villainName)),
                    true
                );
                _audioSystem.PlayPvs(arcade.GameOverSound, uid, AudioParams.Default.WithVolume(-65f));
                return false;
            case (false, false):
                _running = false;
                UpdateUi(
                    uid,
                    Loc.GetString("space-villain-game-player-loses-message"),
                    Loc.GetString("space-villain-game-enemy-dies-with-player-message ", ("enemyName", _villainName)),
                    true
                );
                _audioSystem.PlayPvs(arcade.GameOverSound, uid, AudioParams.Default.WithVolume(-65f));
                return false;
        }
    }
}