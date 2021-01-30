using Microsoft.EntityFrameworkCore.Migrations;

namespace Uchuumaru.Migrations
{
    public partial class TrafficChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TrafficChannelId",
                table: "Guilds",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrafficChannelId",
                table: "Guilds");
        }
    }
}
