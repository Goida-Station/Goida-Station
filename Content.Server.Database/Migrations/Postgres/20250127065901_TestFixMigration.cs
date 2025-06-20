// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers+git@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class TestFixMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rmc_linked_accounts_logs_player__player_id65",
                table: "rmc_linked_accounts_logs");

            migrationBuilder.AddForeignKey(
                name: "FK_rmc_linked_accounts_logs_player_player_id65",
                table: "rmc_linked_accounts_logs",
                column: "player_id",
                principalTable: "player",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rmc_linked_accounts_logs_player_player_id65",
                table: "rmc_linked_accounts_logs");

            migrationBuilder.AddForeignKey(
                name: "FK_rmc_linked_accounts_logs_player__player_id65",
                table: "rmc_linked_accounts_logs",
                column: "player_id",
                principalTable: "player",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}