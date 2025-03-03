using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gimify.Migrations
{
    /// <inheritdoc />
    public partial class PasswordAudate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PasswordSalt",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "User");
        }
    }
}
