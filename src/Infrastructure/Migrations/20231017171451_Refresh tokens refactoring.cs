using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refreshtokensrefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WasUsed",
                table: "RefreshTokens",
                newName: "Used");

            migrationBuilder.RenameColumn(
                name: "Expires",
                table: "RefreshTokens",
                newName: "ExpirationDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Invalidated",
                table: "RefreshTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Invalidated",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "Used",
                table: "RefreshTokens",
                newName: "WasUsed");

            migrationBuilder.RenameColumn(
                name: "ExpirationDateTime",
                table: "RefreshTokens",
                newName: "Expires");
        }
    }
}
