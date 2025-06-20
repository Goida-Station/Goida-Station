// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class RoundStartDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "start_date",
                table: "round",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(65, 65, 65, 65, 65, 65, 65, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_round_start_date",
                table: "round",
                column: "start_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_round_start_date",
                table: "round");

            migrationBuilder.DropColumn(
                name: "start_date",
                table: "round");
        }
    }
}