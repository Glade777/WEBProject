using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gimify.Migrations
{
    /// <inheritdoc />
    public partial class nnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");
        }
    }
}
