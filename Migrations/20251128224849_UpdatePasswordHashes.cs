using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arcade.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Uw2NSbcyTC8wom1YEchTPuV.eTyITOscaKNikN9E54zf1HUZfI/qC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$0vTyhMfGAY56h1v.gGbzHu8EtAh4usIUivBDlZCsPMLA6dmyBoUG6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$rNzCv3xZCdQ9E4tH5mN8O.6JvK1lM2nP3qR4sT5uV6wX7yZ8aB9cD");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$aB1cD2eF3gH4iJ5kL6mN7O.8pQ9rS0tU1vW2xY3zA4bC5dE6fG7hI");
        }
    }
}
