// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Repo <65Titian65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vascreeper@yahoo.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SX-65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Network;
using Robust.Shared.Serialization;
using Content.Shared.Mind;

namespace Content.Shared.Administration;

[Serializable, NetSerializable]
public sealed record PlayerInfo(
    string Username,
    string CharacterName,
    string IdentityName,
    string StartingJob,
    bool Antag,
    RoleTypePrototype RoleProto,
    LocId? Subtype,
    int SortWeight,
    NetEntity? NetEntity,
    NetUserId SessionId,
    bool Connected,
    bool ActiveThisRound,
    bool IsGhost, // Goobstation
    TimeSpan? OverallPlaytime)
{
    private string? _playtimeString;

    public bool IsPinned { get; set; }

    public string PlaytimeString => _playtimeString ??=
        OverallPlaytime?.ToString("%d':'hh':'mm") ?? Loc.GetString("generic-unknown-title");

    public bool Equals(PlayerInfo? other)
    {
        return other?.SessionId == SessionId;
    }

    public override int GetHashCode()
    {
        return SessionId.GetHashCode();
    }
}
