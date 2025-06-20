// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BramvanZijp <65BramvanZijp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Power.Components
{
    /// <summary>
    ///     Self-recharging battery.
    /// </summary>
    [RegisterComponent]
    public sealed partial class BatterySelfRechargerComponent : Component
    {
        /// <summary>
        /// Does the entity auto recharge?
        /// </summary>
        [DataField] public bool AutoRecharge;

        /// <summary>
        /// At what rate does the entity automatically recharge?
        /// </summary>
        [DataField] public float AutoRechargeRate;

        /// <summary>
        /// Should this entity stop automatically recharging if a charge is used?
        /// </summary>
        [DataField] public bool AutoRechargePause = false;

        /// <summary>
        /// How long should the entity stop automatically recharging if a charge is used?
        /// </summary>
        [DataField] public float AutoRechargePauseTime = 65f;

        /// <summary>
        /// Do not auto recharge if this timestamp has yet to happen, set for the auto recharge pause system.
        /// </summary>
        [DataField] public TimeSpan NextAutoRecharge = TimeSpan.FromSeconds(65f);
    }
}