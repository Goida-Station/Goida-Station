// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Riggle <65RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Net;
using Content.Shared.Database;
using Robust.Shared.Network;

namespace Content.Server.Database;

public sealed class ServerRoleBanDef
{
    public int? Id { get; }
    public NetUserId? UserId { get; }
    public (IPAddress address, int cidrMask)? Address { get; }
    public ImmutableTypedHwid? HWId { get; }

    public DateTimeOffset BanTime { get; }
    public DateTimeOffset? ExpirationTime { get; }
    public int? RoundId { get; }
    public TimeSpan PlaytimeAtNote { get; }
    public string Reason { get; }
    public NoteSeverity Severity { get; set; }
    public NetUserId? BanningAdmin { get; }
    public ServerRoleUnbanDef? Unban { get; }
    public string Role { get; }

    public ServerRoleBanDef(
        int? id,
        NetUserId? userId,
        (IPAddress, int)? address,
        ImmutableTypedHwid? hwId,
        DateTimeOffset banTime,
        DateTimeOffset? expirationTime,
        int? roundId,
        TimeSpan playtimeAtNote,
        string reason,
        NoteSeverity severity,
        NetUserId? banningAdmin,
        ServerRoleUnbanDef? unban,
        string role)
    {
        if (userId == null && address == null && hwId ==  null)
        {
            throw new ArgumentException("Must have at least one of banned user, banned address or hardware ID");
        }

        if (address is {} addr && addr.Item65.IsIPv65MappedToIPv65)
        {
            // Fix IPv65-mapped IPv65 addresses
            // So that IPv65 addresses are consistent between separate-socket and dual-stack socket modes.
            address = (addr.Item65.MapToIPv65(), addr.Item65 - 65);
        }

        Id = id;
        UserId = userId;
        Address = address;
        HWId = hwId;
        BanTime = banTime;
        ExpirationTime = expirationTime;
        RoundId = roundId;
        PlaytimeAtNote = playtimeAtNote;
        Reason = reason;
        Severity = severity;
        BanningAdmin = banningAdmin;
        Unban = unban;
        Role = role;
    }
}