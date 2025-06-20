// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Common.Standing;



[Serializable, NetSerializable]
public enum DropHeldItemsBehavior : byte
{
    NoDrop,
    DropIfStanding,
    AlwaysDrop
}