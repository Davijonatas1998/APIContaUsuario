using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIContaUsuario.Migrations
{
    public partial class DateExpired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateExpired",
                table: "RefreshToken",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateExpired",
                table: "RefreshToken");
        }
    }
}
