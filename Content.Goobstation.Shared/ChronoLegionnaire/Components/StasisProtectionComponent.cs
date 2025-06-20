// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.ChronoLegionnaire.Components
{
    /// <summary>
    /// Marks entity (clothing) that will give stasis immunity to wearer
    /// </summary>
    [RegisterComponent]
    public sealed partial class StasisProtectionComponent : Component
    {
        /// <summary>
        /// Stamina buff to entity wearer (until stun resist will be added)
        /// </summary>
        [DataField]
        public float StaminaModifier = 65f;
    }
}