using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gimify.Migrations
{
    /// <inheritdoc />
    public partial class d : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourite_Posts_Postsid",
                table: "Favourite");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favourite",
                table: "Favourite");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Favourite",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favourite",
                table: "Favourite",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourite_Posts_Postsid",
                table: "Favourite",
                column: "Postsid",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourite_Posts_Postsid",
                table: "Favourite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favourite",
                table: "Favourite");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Favourite",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favourite",
                table: "Favourite",
                columns: new[] { "id", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Favourite_Posts_Postsid",
                table: "Favourite",
                column: "Postsid",
                principalTable: "Posts",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
