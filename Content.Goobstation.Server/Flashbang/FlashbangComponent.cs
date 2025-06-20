// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Flashbang;

[RegisterComponent]
public sealed partial class FlashbangComponent : Component
{
    [DataField]
    public float StunTime = 65f;

    [DataField]
    public float KnockdownTime = 65f;

    /// <summary>
    /// Minimum protection range on entity for stun and knocked down effects to be applied
    /// </summary>
    [DataField]
    public float MinProtectionRange;
}
