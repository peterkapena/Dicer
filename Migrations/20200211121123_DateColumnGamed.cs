using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dicer.Migrations
{
    public partial class DateColumnGamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateGamed",
                table: "Gamed");

            migrationBuilder.RenameColumn(
                name: "gamedID",
                table: "Gamed",
                newName: "GamedID");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Gamed",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Gamed");

            migrationBuilder.RenameColumn(
                name: "GamedID",
                table: "Gamed",
                newName: "gamedID");

            migrationBuilder.AddColumn<string>(
                name: "DateGamed",
                table: "Gamed",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
