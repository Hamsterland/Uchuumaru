using Microsoft.EntityFrameworkCore.Migrations;

namespace Uchuumaru.Migrations
{
    public partial class ReportChannelAndModeratorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ModeratorRoleId",
                table: "Guilds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ReportChannelId",
                table: "Guilds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModeratorRoleId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "ReportChannelId",
                table: "Guilds");
        }
    }
}
