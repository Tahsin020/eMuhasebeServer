using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMuhasebeServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mg8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUsers_Users_AppUserId",
                table: "CompanyUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers");

            migrationBuilder.DropIndex(
                name: "IX_CompanyUsers_AppUserId",
                table: "CompanyUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CompanyUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "CompanyUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers",
                columns: new[] { "AppUserId", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyUsers_Users_AppUserId",
                table: "CompanyUsers",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyUsers_Users_AppUserId",
                table: "CompanyUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "CompanyUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CompanyUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyUsers",
                table: "CompanyUsers",
                columns: new[] { "UserId", "CompanyId" });

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
    }
}
