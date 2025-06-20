// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Lincoln McQueen <lincoln.mcqueen@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Common.MartialArts;

/// <summary>
///     Raised when a martial arts combo attack is performed. Contains information about
///     the performer, target, weapon used, and the type of combo attack.
/// </summary>
public sealed class ComboAttackPerformedEvent(
    EntityUid performer,
    EntityUid target,
    EntityUid weapon,
    ComboAttackType type)
    : CancellableEntityEventArgs
{
    public EntityUid Performer { get; } = performer;
    public EntityUid Target { get; } = target;
    public EntityUid Weapon { get; } = weapon;
    public ComboAttackType Type { get; } = type;
}

[Serializable,NetSerializable]
public enum ComboAttackType : byte
{
    Harm,
    HarmLight,
    Disarm,
    Grab,
    Hug,
}