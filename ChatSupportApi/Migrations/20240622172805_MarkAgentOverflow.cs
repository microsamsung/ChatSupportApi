using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSupportApi.Migrations
{
    /// <inheritdoc />
    public partial class MarkAgentOverflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableDuringPeakHours",
                table: "Agents",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsAvailableDuringPeakHours",
                value: false);

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsAvailableDuringPeakHours",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailableDuringPeakHours",
                table: "Agents");
        }
    }
}
