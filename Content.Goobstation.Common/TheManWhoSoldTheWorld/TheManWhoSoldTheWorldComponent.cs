// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 Bandit <queenjess65@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;

namespace Content.Goobstation.Common.TheManWhoSoldTheWorld;

/// <summary>
/// This is used to identify a Holo Cigar User
/// </summary>
[RegisterComponent]
public sealed partial class TheManWhoSoldTheWorldComponent : Component
{
    [ViewVariables]
    public EntityUid? HoloCigarEntity = null;

    [ViewVariables]
    public SoundSpecifier DeathAudio = new SoundPathSpecifier("/Audio/_Goobstation/Items/TheManWhoSoldTheWorld/ouchies.ogg");

    [DataField]
    public bool AddedNoWieldNeeded;
}
