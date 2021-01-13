using Microsoft.EntityFrameworkCore.Migrations;

namespace Uchuumaru.Migrations
{
    public partial class DbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filter_Guild_GuildId",
                table: "Filter");

            migrationBuilder.DropForeignKey(
                name: "FK_Infraction_Guild_GuildId",
                table: "Infraction");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Guild_GuildId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guild",
                table: "Guild");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channel",
                table: "Channel");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Guild",
                newName: "Guilds");

            migrationBuilder.RenameTable(
                name: "Channel",
                newName: "Channels");

            migrationBuilder.RenameIndex(
                name: "IX_User_UserId",
                table: "Users",
                newName: "IX_Users_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_User_GuildId",
                table: "Users",
                newName: "IX_Users_GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_Guild_GuildId",
                table: "Guilds",
                newName: "IX_Guilds_GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_GuildId",
                table: "Channels",
                newName: "IX_Channels_GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_ChannelId",
                table: "Channels",
                newName: "IX_Channels_ChannelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guilds",
                table: "Guilds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Filter_Guilds_GuildId",
                table: "Filter",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Infraction_Guilds_GuildId",
                table: "Infraction",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Guilds_GuildId",
                table: "Users",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filter_Guilds_GuildId",
                table: "Filter");

            migrationBuilder.DropForeignKey(
                name: "FK_Infraction_Guilds_GuildId",
                table: "Infraction");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Guilds_GuildId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guilds",
                table: "Guilds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channels",
                table: "Channels");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Guilds",
                newName: "Guild");

            migrationBuilder.RenameTable(
                name: "Channels",
                newName: "Channel");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserId",
                table: "User",
                newName: "IX_User_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GuildId",
                table: "User",
                newName: "IX_User_GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_Guilds_GuildId",
                table: "Guild",
                newName: "IX_Guild_GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_Channels_GuildId",
                table: "Channel",
                newName: "IX_Channel_GuildId");

            migrationBuilder.RenameIndex(
                name: "IX_Channels_ChannelId",
                table: "Channel",
                newName: "IX_Channel_ChannelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guild",
                table: "Guild",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channel",
                table: "Channel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Filter_Guild_GuildId",
                table: "Filter",
                column: "GuildId",
                principalTable: "Guild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Infraction_Guild_GuildId",
                table: "Infraction",
                column: "GuildId",
                principalTable: "Guild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Guild_GuildId",
                table: "User",
                column: "GuildId",
                principalTable: "Guild",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
