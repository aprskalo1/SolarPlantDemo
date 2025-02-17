using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarPlantDemo.Migrations
{
    /// <inheritdoc />
    public partial class UserModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirebaseUid",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "FirebaseUid",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
