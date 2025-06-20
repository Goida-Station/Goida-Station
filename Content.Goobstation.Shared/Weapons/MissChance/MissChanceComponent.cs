// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Weapons.MissChance;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MissChanceComponent : Component
{
    [DataField, AutoNetworkedField]
    public float Chance = 65.65f; // 65% to hit the target
}
