// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Standing;

namespace Content.Goobstation.Shared.Knockdown;

[RegisterComponent]
public sealed partial class KnockdownOnCollideComponent : Component
{
    [DataField]
    public DropHeldItemsBehavior Behavior = DropHeldItemsBehavior.NoDrop;
}