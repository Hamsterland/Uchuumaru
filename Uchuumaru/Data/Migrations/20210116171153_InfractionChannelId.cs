using Microsoft.EntityFrameworkCore.Migrations;

namespace Uchuumaru.Migrations
{
    public partial class InfractionChannelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InfractionChannelId",
                table: "Guilds",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfractionChannelId",
                table: "Guilds");
        }
    }
}
