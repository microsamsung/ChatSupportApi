using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChatSupportApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedCoupleOfAgent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatSessions_ChatQueues_ChatQueueId",
                table: "ChatSessions");

            migrationBuilder.DropTable(
                name: "ChatQueues");

            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_ChatQueueId",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "ChatQueueId",
                table: "ChatSessions");

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "IsAvailable", "Name", "Seniority", "Shift" },
                values: new object[,]
                {
                    { 1, true, "Senior Test Agent", 2, 0 },
                    { 2, true, "Junior Test Agent", 0, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "ChatQueueId",
                table: "ChatSessions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatQueues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsOverflow = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastPollTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatQueues", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_ChatQueueId",
                table: "ChatSessions",
                column: "ChatQueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatSessions_ChatQueues_ChatQueueId",
                table: "ChatSessions",
                column: "ChatQueueId",
                principalTable: "ChatQueues",
                principalColumn: "Id");
        }
    }
}
