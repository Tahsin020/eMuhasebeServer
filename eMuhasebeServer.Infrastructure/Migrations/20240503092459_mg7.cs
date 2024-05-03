using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMuhasebeServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "CompanyUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUsers_AppUserId",
                table: "CompanyUsers",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUsers_Users_AppUserId",
                table: "CompanyUsers",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUsers_Users_AppUserId",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_AppUserId",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "CompanyUsers");
        }
    }
}
