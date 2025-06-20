// SPDX-FileCopyrightText: 65 Baptr65b65t <65Baptr65b65t@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.RelayedDeathrattle;

[RegisterComponent]
public sealed partial class RelayedDeathrattleComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? Target;

    [DataField]
    public LocId CritMessage = "deathrattle-implant-critical-message";

    [DataField]
    public LocId DeathMessage = "deathrattle-implant-dead-message";

}
