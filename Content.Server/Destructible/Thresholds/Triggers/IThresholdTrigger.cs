// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Damage;

namespace Content.Server.Destructible.Thresholds.Triggers
{
    public interface IThresholdTrigger
    {
        /// <summary>
        ///     Checks if this trigger has been reached.
        /// </summary>
        /// <param name="damageable">The damageable component to check with.</param>
        /// <param name="system">
        ///     An instance of <see cref="DestructibleSystem"/> to pull
        ///     dependencies from, if any.
        /// </param>
        /// <returns>true if this trigger has been reached, false otherwise.</returns>
        bool Reached(DamageableComponent damageable, DestructibleSystem system);
    }
}