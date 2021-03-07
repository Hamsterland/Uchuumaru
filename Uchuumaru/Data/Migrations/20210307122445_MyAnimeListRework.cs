using Microsoft.EntityFrameworkCore.Migrations;

namespace Uchuumaru.Migrations
{
    public partial class MyAnimeListRework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MALUsers_VerificationCode",
                table: "MALUsers");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "MALUsers");

            migrationBuilder.RenameColumn(
                name: "MAL",
                table: "MALUsers",
                newName: "MyAnimeListId");

            migrationBuilder.RenameIndex(
                name: "IX_MALUsers_MAL",
                table: "MALUsers",
                newName: "IX_MALUsers_MyAnimeListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyAnimeListId",
                table: "MALUsers",
                newName: "MAL");

            migrationBuilder.RenameIndex(
                name: "IX_MALUsers_MyAnimeListId",
                table: "MALUsers",
                newName: "IX_MALUsers_MAL");

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "MALUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MALUsers_VerificationCode",
                table: "MALUsers",
                column: "VerificationCode",
                unique: true);
        }
    }
}
