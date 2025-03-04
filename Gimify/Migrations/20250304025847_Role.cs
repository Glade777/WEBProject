using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gimify.Migrations
{
    /// <inheritdoc />
    public partial class Role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourite_Posts_Postsid",
                table: "Favourite");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourite_Posts_Postsid",
                table: "Favourite",
                column: "Postsid",
                principalTable: "Posts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourite_Posts_Postsid",
                table: "Favourite");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourite_Posts_Postsid",
                table: "Favourite",
                column: "Postsid",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
