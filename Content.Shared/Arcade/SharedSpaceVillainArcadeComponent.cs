// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Arcade
{
    public abstract partial class SharedSpaceVillainArcadeComponent : Component
    {
        [Serializable, NetSerializable]
        public enum Indicators
        {
            /// <summary>
            /// Blinks when any invincible flag is set
            /// </summary>
            HealthManager,
            /// <summary>
            /// Blinks when Overflow flag is set
            /// </summary>
            HealthLimiter
        }

        [Serializable, NetSerializable]
        public enum PlayerAction
        {
            Attack,
            Heal,
            Recharge,
            NewGame,
            RequestData
        }

        [Serializable, NetSerializable]
        public enum SpaceVillainArcadeVisualState
        {
            Normal,
            Off,
            Broken,
            Win,
            GameOver,
        }

        [Serializable, NetSerializable]
        public enum SpaceVillainArcadeUiKey
        {
            Key,
        }

        [Serializable, NetSerializable]
        public sealed class SpaceVillainArcadePlayerActionMessage : BoundUserInterfaceMessage
        {
            public readonly PlayerAction PlayerAction;
            public SpaceVillainArcadePlayerActionMessage(PlayerAction playerAction)
            {
                PlayerAction = playerAction;
            }
        }

        [Serializable, NetSerializable]
        public sealed class SpaceVillainArcadeMetaDataUpdateMessage : SpaceVillainArcadeDataUpdateMessage
        {
            public readonly string GameTitle;
            public readonly string EnemyName;
            public readonly bool ButtonsDisabled;
            public SpaceVillainArcadeMetaDataUpdateMessage(int playerHp, int playerMp, int enemyHp, int enemyMp, string playerActionMessage, string enemyActionMessage, string gameTitle, string enemyName, bool buttonsDisabled) : base(playerHp, playerMp, enemyHp, enemyMp, playerActionMessage, enemyActionMessage)
            {
                GameTitle = gameTitle;
                EnemyName = enemyName;
                ButtonsDisabled = buttonsDisabled;
            }
        }

        [Serializable, NetSerializable, Virtual]
        public class SpaceVillainArcadeDataUpdateMessage : BoundUserInterfaceMessage
        {
            public readonly int PlayerHP;
            public readonly int PlayerMP;
            public readonly int EnemyHP;
            public readonly int EnemyMP;
            public readonly string PlayerActionMessage;
            public readonly string EnemyActionMessage;
            public SpaceVillainArcadeDataUpdateMessage(int playerHp, int playerMp, int enemyHp, int enemyMp, string playerActionMessage, string enemyActionMessage)
            {
                PlayerHP = playerHp;
                PlayerMP = playerMp;
                EnemyHP = enemyHp;
                EnemyMP = enemyMp;
                EnemyActionMessage = enemyActionMessage;
                PlayerActionMessage = playerActionMessage;
            }
        }
    }
}