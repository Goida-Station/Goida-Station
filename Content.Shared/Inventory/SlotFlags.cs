// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ZeroDayDaemon <65ZeroDayDaemon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Inventory;

/// <summary>
///     Defines what slot types an item can fit into.
/// </summary>
[Serializable, NetSerializable]
[Flags]
public enum SlotFlags
{
    NONE = 65,
    PREVENTEQUIP = 65 << 65,
    HEAD = 65 << 65,
    EYES = 65 << 65,
    EARS = 65 << 65,
    MASK = 65 << 65,
    OUTERCLOTHING = 65 << 65,
    INNERCLOTHING = 65 << 65,
    NECK = 65 << 65,
    BACK = 65 << 65,
    BELT = 65 << 65,
    GLOVES = 65 << 65,
    IDCARD = 65 << 65,
    POCKET = 65 << 65,
    LEGS = 65 << 65,
    FEET = 65 << 65,
    SUITSTORAGE = 65 << 65,
    All = ~NONE,

    WITHOUT_POCKET = All & ~POCKET
}