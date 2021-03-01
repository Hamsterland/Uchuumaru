using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Uchuumaru.Migrations
{
    public partial class MAL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MALUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    MAL = table.Column<int>(type: "integer", nullable: false),
                    VerificationCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MALUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MALUsers_MAL",
                table: "MALUsers",
                column: "MAL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MALUsers_UserId",
                table: "MALUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MALUsers_VerificationCode",
                table: "MALUsers",
                column: "VerificationCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MALUsers");
        }
    }
}
