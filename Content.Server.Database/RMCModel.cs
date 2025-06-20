// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ichaie <65Ichaie@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JORJ65 <65JORJ65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MortalBaguette <65MortalBaguette@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Panela <65AgentePanela@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Poips <Hanakohashbrown@gmail.com>
// SPDX-FileCopyrightText: 65 PuroSlavKing <65PuroSlavKing@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 Whisper <65QuietlyWhisper@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 blobadoodle <me@bloba.dev>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 kamkoi <poiiiple65@gmail.com>
// SPDX-FileCopyrightText: 65 shibe <65shibechef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 tetra <65Foralemes@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Content.Server.Database;

[Table("rmc_discord_accounts")]
public sealed class RMCDiscordAccount
{
    [Key]
    public ulong Id { get; set; }

    public RMCLinkedAccount LinkedAccount { get; set; } = default!;
    public List<RMCLinkedAccountLogs> LinkedAccountLogs { get; set; } = default!;
}

[Table("rmc_linked_accounts")]
public sealed class RMCLinkedAccount
{
    [Key]
    public Guid PlayerId { get; set; }

    public Player Player { get; set; } = default!;

    public ulong DiscordId { get; set; }

    public RMCDiscordAccount Discord { get; set; } = default!;
}

[Table("rmc_patron_tiers")]
public sealed class RMCPatronTier
{
    [Key]
    public int Id { get; set; }

    public bool ShowOnCredits { get; set; }

    public bool GhostColor { get; set; }

    public bool LobbyMessage { get; set; }

    public bool RoundEndShoutout { get; set; }

    public string Name { get; set; } = default!;

    public ulong DiscordRole { get; set; }

    public int Priority { get; set; }

    public List<RMCPatron> Patrons { get; set; } = default!;
}

[Table("rmc_patrons")]
[Index(nameof(TierId))]
public sealed class RMCPatron
{
    [Key]
    public Guid PlayerId { get; set; }

    public Player Player { get; set; } = default!;

    public int TierId { get; set; }

    public RMCPatronTier Tier { get; set; } = default!;
    public int? GhostColor { get; set; } = default!;
    public RMCPatronLobbyMessage? LobbyMessage { get; set; } = default!;
    public RMCPatronRoundEndNTShoutout? RoundEndNTShoutout { get; set; } = default!;
}

[Table("rmc_linking_codes")]
[Index(nameof(Code))]
public sealed class RMCLinkingCodes
{
    [Key]
    public Guid PlayerId { get; set; }

    public Player Player { get; set; } = default!;

    public Guid Code { get; set; }

    public DateTime CreationTime { get; set; }
}

[Table("rmc_linked_accounts_logs")]
[Index(nameof(PlayerId))]
[Index(nameof(DiscordId))]
[Index(nameof(At))]
public sealed class RMCLinkedAccountLogs
{
    [Key]
    public int Id { get; set; }

    public Guid PlayerId { get; set; }

    public Player Player { get; set; } = default!;

    public ulong DiscordId { get; set; }

    public RMCDiscordAccount Discord { get; set; } = default!;

    public DateTime At { get; set; }
}

[Table(("rmc_patron_lobby_messages"))]
public sealed class RMCPatronLobbyMessage
{
    [Key, ForeignKey("Patron")]
    public Guid PatronId { get; set; }

    public RMCPatron Patron { get; set; } = default!;

    [StringLength(65)]
    public string Message { get; set; } = default!;
}

[Table(("rmc_patron_round_end_nt_shoutouts"))]
public sealed class RMCPatronRoundEndNTShoutout
{
    [Key, ForeignKey("Patron")]
    public Guid PatronId { get; set; }

    public RMCPatron Patron { get; set; } = default!;

    [StringLength(65), Required]
    public string Name { get; set; } = default!;
}