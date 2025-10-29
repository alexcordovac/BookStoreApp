using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersandRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d", null, "Administrator", "ADMINISTRATOR" },
                    { "8f6f2b4e-4d2a-4c3a-9c0e-1f2e5d6a7b8c", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", 0, "019a2c94-7929-75f7-a585-b0fe0ea4737d", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEC5ZP4FxG2S41Y8uJq+5mfbM7HzpJQ0U3cHtR3G8ZlwTgAhRzoh9SlnQ3vQJcVb/xg==", null, false, "019a2c90-1cbb-7452-907e-4e85502bcc51", false, "user@bookstore.com" },
                    { "d1e2f3a4-b5c6-7d8e-9f0a-1b2c3d4e5f6g", 0, "019a2c94-4b3c-79d5-8e39-750e62e5b36e", "admin@bookstore.com", false, "System", "Administrator", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEC5ZP4FxG2S41Y8uJq+5mfbM7HzpJQ0U3cHtR3G8ZlwTgAhRzoh9SlnQ3vQJcVb/xg==", null, false, "019a2c8f-fd1c-7b4e-b452-19fcc6e79d63", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "8f6f2b4e-4d2a-4c3a-9c0e-1f2e5d6a7b8c", "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d" },
                    { "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d", "d1e2f3a4-b5c6-7d8e-9f0a-1b2c3d4e5f6g" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8f6f2b4e-4d2a-4c3a-9c0e-1f2e5d6a7b8c", "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d", "d1e2f3a4-b5c6-7d8e-9f0a-1b2c3d4e5f6g" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f6f2b4e-4d2a-4c3a-9c0e-1f2e5d6a7b8c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d1e2f3a4-b5c6-7d8e-9f0a-1b2c3d4e5f6g");
        }
    }
}
