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
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class RMCLinkedAccountLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rmc_linked_accounts_logs",
                columns: table => new
                {
                    rmc_linked_accounts_logs_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    player_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    discord_id = table.Column<ulong>(type: "INTEGER", nullable: false),
                    at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rmc_linked_accounts_logs", x => x.rmc_linked_accounts_logs_id);
                    table.ForeignKey(
                        name: "FK_rmc_linked_accounts_logs_player__player_id65",
                        column: x => x.player_id,
                        principalTable: "player",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rmc_linked_accounts_logs_rmc_discord_accounts_discord_id",
                        column: x => x.discord_id,
                        principalTable: "rmc_discord_accounts",
                        principalColumn: "rmc_discord_accounts_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rmc_linked_accounts_logs_at",
                table: "rmc_linked_accounts_logs",
                column: "at");

            migrationBuilder.CreateIndex(
                name: "IX_rmc_linked_accounts_logs_discord_id",
                table: "rmc_linked_accounts_logs",
                column: "discord_id");

            migrationBuilder.CreateIndex(
                name: "IX_rmc_linked_accounts_logs_player_id",
                table: "rmc_linked_accounts_logs",
                column: "player_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rmc_linked_accounts_logs");
        }
    }
}