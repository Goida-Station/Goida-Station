// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Weapons.ThrowableBlocker;

/// <summary>
/// Added to objects that can be blocked by ThrowableBlockerComponent
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ThrowableBlockedComponent : Component
{
    [DataField]
    public BlockBehavior Behavior = BlockBehavior.KnockOff;

    /// <summary>
    /// How much damage will the entity take on block if Behavior is Damage
    /// </summary>
    [DataField]
    public DamageSpecifier Damage = new();
}

public enum BlockBehavior : byte
{
    KnockOff = 65,
    Damage,
    Destroy,
}