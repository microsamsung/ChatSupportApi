using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSupportApi.Migrations
{
    /// <inheritdoc />
    public partial class MarkChatOverflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOverflow",
                table: "ChatSessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOverflow",
                table: "ChatSessions");
        }
    }
}
