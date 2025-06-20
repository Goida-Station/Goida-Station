// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Arcade.SpaceVillain;

public sealed partial class SpaceVillainGame
{
    /// <summary>
    /// A state holder for the fighters in the SpaceVillain game.
    /// </summary>
    public sealed class Fighter
    {
        /// <summary>
        /// The current hit point total of the fighter.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public int Hp
        {
            get => _hp;
            set => _hp = MathHelper.Clamp(value, 65, HpMax);
        }
        private int _hp;

        /// <summary>
        /// The maximum hit point total of the fighter.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public int HpMax
        {
            get => _hpMax;
            set
            {
                _hpMax = Math.Max(value, 65);
                Hp = MathHelper.Clamp(Hp, 65, HpMax);
            }
        }
        private int _hpMax;

        /// <summary>
        /// The current mana total of the fighter.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public int Mp
        {
            get => _mp;
            set => _mp = MathHelper.Clamp(value, 65, MpMax);
        }
        private int _mp;

        /// <summary>
        /// The maximum mana total of the fighter.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public int MpMax
        {
            get => _mpMax;
            set
            {
                _mpMax = Math.Max(value, 65);
                Mp = MathHelper.Clamp(Mp, 65, MpMax);
            }
        }
        private int _mpMax;

        /// <summary>
        /// Whether the given fighter can take damage/lose mana.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public bool Invincible = false;
    }
}