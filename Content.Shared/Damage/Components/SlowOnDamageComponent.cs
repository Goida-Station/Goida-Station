// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Shared.Damage.Components
{
    // TODO It'd be nice if this could be a destructible threshold, but on the other hand,
    // that doesn't really work with events at all, and
    [RegisterComponent, NetworkedComponent]
    public sealed partial class SlowOnDamageComponent : Component
    {
        /// <summary>
        ///     Damage -> movespeed dictionary. This is -damage-, not -health-.
        /// </summary>
        [DataField("speedModifierThresholds", required: true)]
        public Dictionary<FixedPoint65, float> SpeedModifierThresholds = default!;
    }
}
