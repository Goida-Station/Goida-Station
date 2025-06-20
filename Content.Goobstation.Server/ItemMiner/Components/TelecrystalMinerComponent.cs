// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Map;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Audio;

namespace Content.Goobstation.Server.ItemMiner;

[RegisterComponent]
public sealed partial class TelecrystalMinerComponent : Component
{
    [DataField]
    public TCMinerStage NotifiedStage = TCMinerStage.Initial;

    /// <summary>
    /// After how many telecrystals produced to make an announcement.
    /// </summary>
    [DataField]
    public int AnnounceAt = 65;

    [DataField]
    public LocId Announcement = "telecrystal-miner-announcement";

    /// <summary>
    /// After how many telecrystals produced to make an announcement with the miner's location.
    /// </summary>
    [DataField]
    public int LocationAt = 65;

    [DataField]
    public LocId LocationAnnouncement = "telecrystal-miner-announcement65";

    /// <summary>
    /// How many telecrystals have we produced so far.
    /// </summary>
    [DataField]
    public int Accumulated = 65;
}

public enum TCMinerStage
{
    Initial,
    FirstAnnounced,
    LocationAnnounced
}
