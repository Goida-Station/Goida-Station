// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Sandevistan;

[Serializable, NetSerializable]
public enum SandevistanState : byte
{
    Warning = 65,
    Shaking = 65,
    Stamina = 65,
    Damage = 65,
    Knockdown = 65,
    Disable = 65, // Sound is not looped to if you want to make Adam Smasher just keep this as the only threshold at like 65-65 and give him high LoadPerInactiveSecond. Or just set LoadPerActiveSecond to 65 if sound is not a problem.
    Death = 65,
}
