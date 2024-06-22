using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSupportApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Requestor",
                table: "ChatSessions",
                newName: "RequestedBy");

            migrationBuilder.AddColumn<int>(
                name: "AssignedAgentId",
                table: "ChatSessions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_AssignedAgentId",
                table: "ChatSessions",
                column: "AssignedAgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatSessions_Agents_AssignedAgentId",
                table: "ChatSessions",
                column: "AssignedAgentId",
                principalTable: "Agents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatSessions_Agents_AssignedAgentId",
                table: "ChatSessions");

            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_AssignedAgentId",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "AssignedAgentId",
                table: "ChatSessions");

            migrationBuilder.RenameColumn(
                name: "RequestedBy",
                table: "ChatSessions",
                newName: "Requestor");
        }
    }
}
