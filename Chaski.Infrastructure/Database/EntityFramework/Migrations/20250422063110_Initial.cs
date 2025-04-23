using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chaski.Infrastructure.Database.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "USERS");

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "USERS",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    lastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "USERS",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    emailConfirmationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    emailConfirmationTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    passwordResetTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    passwordResetTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    lastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "USERS",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    revokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    replacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reasonRevoked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    lastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_userId",
                        column: x => x.userId,
                        principalSchema: "USERS",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "USERS",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    lastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastModifiedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_roleId",
                        column: x => x.roleId,
                        principalSchema: "USERS",
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_userId",
                        column: x => x.userId,
                        principalSchema: "USERS",
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "USERS",
                table: "Role",
                columns: new[] { "id", "createdAt", "createdBy", "description", "lastModifiedBy", "lastModifiedAt", "name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 22, 6, 31, 7, 596, DateTimeKind.Utc).AddTicks(7799), 1, "Administrator role", 1, new DateTime(2025, 4, 22, 6, 31, 7, 596, DateTimeKind.Utc).AddTicks(8602), "Admin" },
                    { 2, new DateTime(2025, 4, 22, 6, 31, 7, 596, DateTimeKind.Utc).AddTicks(8896), 1, "Standard user role", 1, new DateTime(2025, 4, 22, 6, 31, 7, 596, DateTimeKind.Utc).AddTicks(8897), "User" }
                });

            migrationBuilder.InsertData(
                schema: "USERS",
                table: "User",
                columns: new[] { "id", "createdAt", "createdBy", "email", "emailConfirmationToken", "emailConfirmationTokenExpiry", "isEmailConfirmed", "lastModifiedBy", "lastModifiedAt", "passwordHash", "passwordResetTokenExpiry", "passwordResetTokenHash", "status", "username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 22, 6, 31, 9, 123, DateTimeKind.Utc).AddTicks(4616), 1, "admin@chaski.com", null, null, true, 1, new DateTime(2025, 4, 22, 6, 31, 9, 123, DateTimeKind.Utc).AddTicks(4618), "4wNW1FamP/hcDSqJsJ/Ofw==:g5tT5vgj0Z6IGM9sumOmEkF0Qzw226R0QpjXc983RTs=", null, null, 1, "admin" },
                    { 2, new DateTime(2025, 4, 22, 6, 31, 9, 123, DateTimeKind.Utc).AddTicks(4649), 1, "user@chaski.com", null, null, true, 1, new DateTime(2025, 4, 22, 6, 31, 9, 123, DateTimeKind.Utc).AddTicks(4650), "t+7vJAtbb3+wBmD+ux67AQ==:aHK3Y4d3U8JYEZmLVD2deM8JiCGaF6I78MHp7jEkc/A=", null, null, 1, "user" }
                });

            migrationBuilder.InsertData(
                schema: "USERS",
                table: "UserRole",
                columns: new[] { "id", "createdAt", "createdBy", "lastModifiedBy", "lastModifiedAt", "roleId", "userId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 22, 6, 31, 9, 123, DateTimeKind.Utc).AddTicks(8525), 1, 1, new DateTime(2025, 4, 22, 6, 31, 9, 123, DateTimeKind.Utc).AddTicks(8527), 1, 1 },
                    { 2, new DateTime(2025, 4, 22, 6, 31, 9, 123, DateTimeKind.Utc).AddTicks(8544), 1, 1, new DateTime(2025, 4, 22, 6, 31, 9, 123, DateTimeKind.Utc).AddTicks(8545), 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_token",
                schema: "USERS",
                table: "RefreshToken",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_userId",
                schema: "USERS",
                table: "RefreshToken",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_userId_expires_revoked",
                schema: "USERS",
                table: "RefreshToken",
                columns: new[] { "userId", "expires", "revoked" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_roleId",
                schema: "USERS",
                table: "UserRole",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_userId",
                schema: "USERS",
                table: "UserRole",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "USERS");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "USERS");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "USERS");

            migrationBuilder.DropTable(
                name: "User",
                schema: "USERS");
        }
    }
}
