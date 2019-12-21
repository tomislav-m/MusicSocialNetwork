using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class YearToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "YearReleased",
                table: "Album",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<int>(
                name: "YearStr",
                table: "Album",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearStr",
                table: "Album");

            migrationBuilder.AlterColumn<DateTime>(
                name: "YearReleased",
                table: "Album",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
